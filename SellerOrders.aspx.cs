
using BookStoreWebForms.Data;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BookStoreWebForms
{
    public partial class SellerOrders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/SignIn.aspx");
                return;
            }

            if (!User.IsInRole("Seller"))
            {
                Response.StatusCode = 403;
                Response.TrySkipIisCustomErrors = true;

                MessagePanel.Visible = true;
                MessageLabel.Text = "Only sellers can access this page.";
                OrdersGridView.Visible = false;
                EmptyOrdersPanel.Visible = false;
                return;
            }

            if (!IsPostBack)
            {
                BindOrders();
            }
        }

        private void BindOrders()
        {
            string sellerId = User.Identity.GetUserId();

            const string sql = @"
                SELECT
                    f.FulfillmentId,
                    o.OrderId,
                    o.OrderDate,
                    o.ShippingAddress,
                    f.FulfillmentStatus,

                    STUFF(
                        (
                            SELECT
                                ', ' + b.Title
                                + ' (x'
                                + CAST(oi.Quantity AS VARCHAR(10))
                                + ')'
                            FROM dbo.SellerOrderFulfillmentItems fi
                            INNER JOIN dbo.OrderItems oi
                                ON oi.OrderItemId = fi.OrderItemId
                            INNER JOIN dbo.Books b
                                ON b.BookId = oi.BookId
                            WHERE fi.FulfillmentId = f.FulfillmentId
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'),
                        1, 2, ''
                    ) AS Books

                FROM dbo.SellerOrderFulfillments f
                INNER JOIN dbo.Orders o
                    ON o.OrderId = f.OrderId

                WHERE f.SellerUserId = @SellerId

                ORDER BY o.OrderDate DESC, o.OrderId DESC;";

            try
            {
                DataTable table = new DataTable();

                using (SqlConnection connection = Db.GetConnection())
                using (SqlCommand command = new SqlCommand(sql, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.Parameters.Add(
                        "@SellerId", SqlDbType.NVarChar, 128
                    ).Value = sellerId;

                    adapter.Fill(table);
                }

                OrdersGridView.DataSource = table;
                OrdersGridView.DataBind();

                EmptyOrdersPanel.Visible = table.Rows.Count == 0;
                MessagePanel.Visible = false;
            }
            catch (Exception)
            {
                OrdersGridView.DataSource = null;
                OrdersGridView.DataBind();

                EmptyOrdersPanel.Visible = false;
                MessagePanel.Visible = true;

                MessageLabel.Text =
                    "Could not load orders. Please try again.";
            }
        }

        protected void OrdersGridView_RowDataBound(
            object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            DataRowView row = (DataRowView)e.Row.DataItem;

            string currentStatus =
                Convert.ToString(row["FulfillmentStatus"]);

            DropDownList statusDropDown =
                (DropDownList)e.Row.FindControl("StatusDropDown");

            if (statusDropDown == null)
            {
                return;
            }

            ListItem selectedItem =
                statusDropDown.Items.FindByValue(currentStatus);

            if (selectedItem != null)
            {
                statusDropDown.ClearSelection();
                selectedItem.Selected = true;
            }
        }

        protected void OrdersGridView_RowCommand(
            object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "UpdateStatus")
            {
                return;
            }

            if (!User.Identity.IsAuthenticated ||
                !User.IsInRole("Seller"))
            {
                Response.StatusCode = 403;
                Response.TrySkipIisCustomErrors = true;

                ShowMessage(
                    "You are not authorized to update fulfillment status.");
                return;
            }

            int fulfillmentId;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument),
                out fulfillmentId))
            {
                ShowMessage("Invalid fulfillment number.");
                return;
            }

            GridViewRow row =
                (GridViewRow)((Control)e.CommandSource).NamingContainer;

            DropDownList statusDropDown =
                (DropDownList)row.FindControl("StatusDropDown");

            if (statusDropDown == null)
            {
                ShowMessage("Could not read the selected status.");
                return;
            }

            string newStatus = statusDropDown.SelectedValue;

            string[] allowedStatuses =
            {
                "Pending",
                "Confirmed",
                "Processing",
                "Shipped",
                "Delivered",
                "Cancelled"
            };

            if (Array.IndexOf(allowedStatuses, newStatus) < 0)
            {
                ShowMessage("Invalid fulfillment status.");
                return;
            }

            string sellerId = User.Identity.GetUserId();

            if (String.IsNullOrWhiteSpace(sellerId))
            {
                ShowMessage("Your user account could not be verified.");
                return;
            }

            string resultMessage;

            const string selectSql = @"
                SELECT FulfillmentStatus, OrderId
                FROM dbo.SellerOrderFulfillments
                    WITH (UPDLOCK, HOLDLOCK)
                WHERE FulfillmentId = @FulfillmentId
                  AND SellerUserId = @SellerId;";

            const string updateSql = @"
                UPDATE dbo.SellerOrderFulfillments
                SET
                    FulfillmentStatus = @NewStatus,
                    UpdatedAt = SYSDATETIME()
                WHERE FulfillmentId = @FulfillmentId
                  AND SellerUserId = @SellerId;";

            const string historySql = @"
                INSERT INTO dbo.SellerOrderFulfillmentStatusHistory
                (
                    FulfillmentId,
                    PreviousStatus,
                    NewStatus,
                    ChangedByUserId,
                    ChangeNote,
                    ChangedAt
                )
                VALUES
                (
                    @FulfillmentId,
                    @PreviousStatus,
                    @NewStatus,
                    @SellerId,
                    @ChangeNote,
                    SYSDATETIME()
                );";

            // Recalculate the overall order status using ALL seller
            // fulfillments belonging to the same order.
            const string recalculateOrderStatusSql = @"
                UPDATE o
                SET OrderStatus =
                    CASE
                        -- Every seller has cancelled.
                        WHEN sc.TotalCount > 0
                             AND sc.CancelledCount = sc.TotalCount
                            THEN 'Cancelled'

                        -- Every seller has delivered.
                        WHEN sc.TotalCount > 0
                             AND sc.DeliveredCount = sc.TotalCount
                            THEN 'Delivered'

                        -- All fulfillments are shipped or delivered,
                        -- and at least one is still shipped.
                        WHEN sc.TotalCount > 0
                             AND sc.ShippedCount > 0
                             AND sc.ShippedCount + sc.DeliveredCount
                                 = sc.TotalCount
                            THEN 'Shipped'

                        -- At least one seller has started processing
                        -- or confirmed the fulfillment.
                        WHEN sc.ConfirmedCount > 0
                             OR sc.ProcessingCount > 0
                            THEN 'Processing'

                        -- Every seller fulfillment is still pending.
                        WHEN sc.TotalCount > 0
                             AND sc.PendingCount = sc.TotalCount
                            THEN 'Pending'

                        -- Includes mixed outcomes, such as one seller
                        -- cancelling while another ships or delivers.
                        ELSE 'Processing'
                    END
                FROM dbo.Orders o
                CROSS APPLY
                (
                    SELECT
                        COUNT(*) AS TotalCount,

                        COUNT(CASE
                            WHEN FulfillmentStatus = 'Pending'
                            THEN 1 END) AS PendingCount,

                        COUNT(CASE
                            WHEN FulfillmentStatus = 'Confirmed'
                            THEN 1 END) AS ConfirmedCount,

                        COUNT(CASE
                            WHEN FulfillmentStatus = 'Processing'
                            THEN 1 END) AS ProcessingCount,

                        COUNT(CASE
                            WHEN FulfillmentStatus = 'Shipped'
                            THEN 1 END) AS ShippedCount,

                        COUNT(CASE
                            WHEN FulfillmentStatus = 'Delivered'
                            THEN 1 END) AS DeliveredCount,

                        COUNT(CASE
                            WHEN FulfillmentStatus = 'Cancelled'
                            THEN 1 END) AS CancelledCount

                    FROM dbo.SellerOrderFulfillments
                    WHERE OrderId = @OrderId
                ) sc
                WHERE o.OrderId = @OrderId;";

            try
            {
                using (SqlConnection connection = Db.GetConnection())
                {
                    connection.Open();

                    using (SqlTransaction transaction =
                        connection.BeginTransaction(
                            IsolationLevel.Serializable))
                    {
                        try
                        {
                            string previousStatus = null;
                            int orderId = 0;

                            // Verify that this fulfillment belongs to
                            // the currently signed-in seller.
                            using (SqlCommand selectCommand =
                                new SqlCommand(
                                    selectSql, connection, transaction))
                            {
                                selectCommand.Parameters.Add(
                                    "@FulfillmentId", SqlDbType.Int
                                ).Value = fulfillmentId;

                                selectCommand.Parameters.Add(
                                    "@SellerId", SqlDbType.NVarChar, 128
                                ).Value = sellerId;

                                using (SqlDataReader reader =
                                    selectCommand.ExecuteReader())
                                {
                                    if (!reader.Read())
                                    {
                                        reader.Close();
                                        transaction.Rollback();

                                        resultMessage =
                                            "The fulfillment could not be found " +
                                            "under your seller account.";

                                        BindOrders();
                                        ShowMessage(resultMessage);
                                        return;
                                    }

                                    previousStatus =
                                        Convert.ToString(
                                            reader["FulfillmentStatus"]);

                                    orderId = Convert.ToInt32(
                                        reader["OrderId"]);
                                }
                            }

                            // Avoid duplicate history entries when the
                            // requested status has not changed.
                            if (previousStatus == newStatus)
                            {
                                transaction.Commit();

                                resultMessage =
                                    "The fulfillment already has this status. " +
                                    "No change was recorded.";

                                BindOrders();
                                ShowMessage(resultMessage);
                                return;
                            }

                            // Update only this seller's fulfillment.
                            using (SqlCommand updateCommand =
                                new SqlCommand(
                                    updateSql, connection, transaction))
                            {
                                updateCommand.Parameters.Add(
                                    "@NewStatus", SqlDbType.NVarChar, 30
                                ).Value = newStatus;

                                updateCommand.Parameters.Add(
                                    "@FulfillmentId", SqlDbType.Int
                                ).Value = fulfillmentId;

                                updateCommand.Parameters.Add(
                                    "@SellerId", SqlDbType.NVarChar, 128
                                ).Value = sellerId;

                                int affectedRows =
                                    updateCommand.ExecuteNonQuery();

                                if (affectedRows != 1)
                                {
                                    throw new InvalidOperationException(
                                        "The fulfillment status was not updated.");
                                }
                            }

                            // Record the status change in the audit history.
                            using (SqlCommand historyCommand =
                                new SqlCommand(
                                    historySql, connection, transaction))
                            {
                                historyCommand.Parameters.Add(
                                    "@FulfillmentId", SqlDbType.Int
                                ).Value = fulfillmentId;

                                historyCommand.Parameters.Add(
                                    "@PreviousStatus", SqlDbType.NVarChar, 30
                                ).Value = previousStatus;

                                historyCommand.Parameters.Add(
                                    "@NewStatus", SqlDbType.NVarChar, 30
                                ).Value = newStatus;

                                historyCommand.Parameters.Add(
                                    "@SellerId", SqlDbType.NVarChar, 128
                                ).Value = sellerId;

                                historyCommand.Parameters.Add(
                                    "@ChangeNote", SqlDbType.NVarChar, 500
                                ).Value =
                                    "Status updated by seller through SellerOrders.aspx.";

                                historyCommand.ExecuteNonQuery();
                            }

                            // Update the overall customer-facing order status.
                            using (SqlCommand orderStatusCommand =
                                new SqlCommand(
                                    recalculateOrderStatusSql,
                                    connection,
                                    transaction))
                            {
                                orderStatusCommand.Parameters.Add(
                                    "@OrderId", SqlDbType.Int
                                ).Value = orderId;

                                int affectedOrders =
                                    orderStatusCommand.ExecuteNonQuery();

                                if (affectedOrders != 1)
                                {
                                    throw new InvalidOperationException(
                                        "The overall order status was not updated.");
                                }
                            }

                            // All three changes succeed or all are rolled back:
                            // fulfillment, audit history, and overall order status.
                            transaction.Commit();

                            resultMessage =
                                "Fulfillment status updated successfully. " +
                                "The audit history and overall order status " +
                                "have also been updated.";
                        }
                        catch
                        {
                            try
                            {
                                transaction.Rollback();
                            }
                            catch
                            {
                                // Preserve the original exception.
                            }

                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                resultMessage =
                    "Could not update fulfillment status, history, or " +
                    "overall order status. Please try again.";
            }

            BindOrders();
            ShowMessage(resultMessage);
        }

        private void ShowMessage(string message)
        {
            MessagePanel.Visible = true;
            MessageLabel.Text = HttpUtility.HtmlEncode(message);
        }
    }
}
