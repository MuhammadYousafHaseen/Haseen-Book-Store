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
    public partial class SellerPayments : System.Web.UI.Page
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
                MessageLabel.Text =
                    "Only sellers can access this page.";

                PaymentsGridView.Visible = false;
                EmptyPaymentsPanel.Visible = false;
                return;
            }

            if (!IsPostBack)
            {
                BindPayments();
            }
        }

        private void BindPayments()
        {
            string sellerId = User.Identity.GetUserId();

            const string sql = @"
                SELECT
                    sp.SellerFulfillmentPaymentId,
                    f.FulfillmentId,
                    o.OrderId,
                    o.OrderDate,
                    sp.PaymentMethod,
                    sp.PaymentStatus,
                    sp.Amount,
                    sp.TransactionReference,

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

                FROM dbo.SellerFulfillmentPayments sp
                INNER JOIN dbo.SellerOrderFulfillments f
                    ON f.FulfillmentId = sp.FulfillmentId
                INNER JOIN dbo.Orders o
                    ON o.OrderId = f.OrderId

                WHERE f.SellerUserId = @SellerId

                ORDER BY o.OrderDate DESC, o.OrderId DESC;";

            try
            {
                DataTable table = new DataTable();

                using (SqlConnection connection = Db.GetConnection())
                using (SqlCommand command =
                    new SqlCommand(sql, connection))
                using (SqlDataAdapter adapter =
                    new SqlDataAdapter(command))
                {
                    command.Parameters.Add(
                        "@SellerId", SqlDbType.NVarChar, 128
                    ).Value = sellerId;

                    adapter.Fill(table);
                }

                PaymentsGridView.DataSource = table;
                PaymentsGridView.DataBind();

                EmptyPaymentsPanel.Visible =
                    table.Rows.Count == 0;

                MessagePanel.Visible = false;
            }
            catch (Exception)
            {
                PaymentsGridView.DataSource = null;
                PaymentsGridView.DataBind();

                EmptyPaymentsPanel.Visible = false;
                MessagePanel.Visible = true;

                MessageLabel.Text =
                    "Could not load payments. Please try again.";
            }
        }

        protected void PaymentsGridView_RowDataBound(
            object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            DataRowView row = (DataRowView)e.Row.DataItem;

            string currentStatus =
                Convert.ToString(row["PaymentStatus"]);

            string method =
                Convert.ToString(row["PaymentMethod"]);

            DropDownList statusDropDown =
                (DropDownList)e.Row.FindControl(
                    "PaymentStatusDropDown");

            Button updateButton =
                (Button)e.Row.FindControl(
                    "UpdatePaymentButton");

            if (statusDropDown != null)
            {
                ListItem selectedItem =
                    statusDropDown.Items.FindByValue(currentStatus);

                if (selectedItem != null)
                {
                    statusDropDown.ClearSelection();
                    selectedItem.Selected = true;
                }
            }

            // Until gateway integration is implemented, sellers can
            // manage only Cash on Delivery payment records.
            if (updateButton != null &&
                !String.Equals(
                    method,
                    "CashOnDelivery",
                    StringComparison.OrdinalIgnoreCase))
            {
                updateButton.Enabled = false;
                updateButton.ToolTip =
                    "Online payment status must be verified by its payment provider.";
            }
        }

        protected void PaymentsGridView_RowCommand(
            object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "UpdatePayment")
            {
                return;
            }

            if (!User.Identity.IsAuthenticated ||
                !User.IsInRole("Seller"))
            {
                Response.StatusCode = 403;
                Response.TrySkipIisCustomErrors = true;

                ShowMessage(
                    "You are not authorized to update payments.");
                return;
            }

            int paymentId;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument),
                out paymentId) ||
                paymentId <= 0)
            {
                ShowMessage("Invalid payment number.");
                return;
            }

            GridViewRow row =
                (GridViewRow)((Control)e.CommandSource).NamingContainer;

            DropDownList statusDropDown =
                (DropDownList)row.FindControl(
                    "PaymentStatusDropDown");

            TextBox noteTextBox =
                (TextBox)row.FindControl("ChangeNoteTextBox");

            if (statusDropDown == null || noteTextBox == null)
            {
                ShowMessage(
                    "Could not read the payment update information.");
                return;
            }

            string newStatus = statusDropDown.SelectedValue;

            string[] allowedStatuses =
            {
                "Pending",
                "Paid",
                "Failed",
                "Refunded"
            };

            if (Array.IndexOf(allowedStatuses, newStatus) < 0)
            {
                ShowMessage("Invalid payment status.");
                return;
            }

            string changeNote =
                (noteTextBox.Text ?? String.Empty).Trim();

            if (changeNote.Length > 500)
            {
                ShowMessage(
                    "The payment note cannot exceed 500 characters.");
                return;
            }

            string sellerId = User.Identity.GetUserId();

            if (String.IsNullOrWhiteSpace(sellerId))
            {
                ShowMessage(
                    "Your user account could not be verified.");
                return;
            }

            string resultMessage;

            const string selectSql = @"
                SELECT
                    sp.PaymentStatus,
                    sp.PaymentMethod,
                    f.OrderId
                FROM dbo.SellerFulfillmentPayments sp
                    WITH (UPDLOCK, HOLDLOCK)
                INNER JOIN dbo.SellerOrderFulfillments f
                    ON f.FulfillmentId = sp.FulfillmentId
                WHERE sp.SellerFulfillmentPaymentId = @PaymentId
                  AND f.SellerUserId = @SellerId;";

            const string updateSql = @"
                UPDATE sp
                SET
                    PaymentStatus = @NewStatus,
                    UpdatedAt = SYSDATETIME()
                FROM dbo.SellerFulfillmentPayments sp
                INNER JOIN dbo.SellerOrderFulfillments f
                    ON f.FulfillmentId = sp.FulfillmentId
                WHERE sp.SellerFulfillmentPaymentId = @PaymentId
                  AND f.SellerUserId = @SellerId;";

            const string historySql = @"
                INSERT INTO dbo.SellerFulfillmentPaymentStatusHistory
                (
                    SellerFulfillmentPaymentId,
                    PreviousStatus,
                    NewStatus,
                    ChangedByUserId,
                    ChangeNote,
                    ChangedAt
                )
                VALUES
                (
                    @PaymentId,
                    @PreviousStatus,
                    @NewStatus,
                    @SellerId,
                    @ChangeNote,
                    SYSDATETIME()
                );";

            // Keep the legacy order-level payment record synchronized.
            // The existing Payments table cannot represent "Partially Paid".
            const string syncOrderPaymentSql = @"
                UPDATE dbo.Payments
                SET
                    PaymentStatus =
                        CASE
                            WHEN EXISTS
                            (
                                SELECT 1
                                FROM dbo.SellerFulfillmentPayments sp
                                INNER JOIN dbo.SellerOrderFulfillments f
                                    ON f.FulfillmentId = sp.FulfillmentId
                                WHERE f.OrderId = @OrderId
                            )
                            AND NOT EXISTS
                            (
                                SELECT 1
                                FROM dbo.SellerFulfillmentPayments sp
                                INNER JOIN dbo.SellerOrderFulfillments f
                                    ON f.FulfillmentId = sp.FulfillmentId
                                WHERE f.OrderId = @OrderId
                                  AND sp.PaymentStatus <> 'Paid'
                            )
                                THEN 'Paid'

                            WHEN EXISTS
                            (
                                SELECT 1
                                FROM dbo.SellerFulfillmentPayments sp
                                INNER JOIN dbo.SellerOrderFulfillments f
                                    ON f.FulfillmentId = sp.FulfillmentId
                                WHERE f.OrderId = @OrderId
                            )
                            AND NOT EXISTS
                            (
                                SELECT 1
                                FROM dbo.SellerFulfillmentPayments sp
                                INNER JOIN dbo.SellerOrderFulfillments f
                                    ON f.FulfillmentId = sp.FulfillmentId
                                WHERE f.OrderId = @OrderId
                                  AND sp.PaymentStatus <> 'Refunded'
                            )
                                THEN 'Refunded'

                            WHEN EXISTS
                            (
                                SELECT 1
                                FROM dbo.SellerFulfillmentPayments sp
                                INNER JOIN dbo.SellerOrderFulfillments f
                                    ON f.FulfillmentId = sp.FulfillmentId
                                WHERE f.OrderId = @OrderId
                                  AND sp.PaymentStatus = 'Failed'
                            )
                                THEN 'Failed'

                            ELSE 'Pending'
                        END,
                    UpdatedAt = SYSDATETIME()
                WHERE OrderId = @OrderId;";

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
                            string paymentMethod = null;
                            int orderId = 0;

                            // Verify ownership and lock the payment row.
                            using (SqlCommand command =
                                new SqlCommand(
                                    selectSql,
                                    connection,
                                    transaction))
                            {
                                command.Parameters.Add(
                                    "@PaymentId", SqlDbType.Int
                                ).Value = paymentId;

                                command.Parameters.Add(
                                    "@SellerId", SqlDbType.NVarChar, 128
                                ).Value = sellerId;

                                using (SqlDataReader reader =
                                    command.ExecuteReader())
                                {
                                    if (!reader.Read())
                                    {
                                        throw new UnauthorizedAccessException(
                                            "Payment not found for this seller.");
                                    }

                                    previousStatus =
                                        Convert.ToString(
                                            reader["PaymentStatus"]);

                                    paymentMethod =
                                        Convert.ToString(
                                            reader["PaymentMethod"]);

                                    orderId = Convert.ToInt32(
                                        reader["OrderId"]);
                                }
                            }

                            if (!String.Equals(
                                paymentMethod,
                                "CashOnDelivery",
                                StringComparison.OrdinalIgnoreCase))
                            {
                                throw new InvalidOperationException(
                                    "Only Cash on Delivery payments can be " +
                                    "updated by sellers at this stage.");
                            }

                            if (previousStatus == newStatus)
                            {
                                transaction.Commit();

                                resultMessage =
                                    "The payment already has this status. " +
                                    "No change was recorded.";

                                BindPayments();
                                ShowMessage(resultMessage);
                                return;
                            }

                            // Enforce valid transitions on the server.
                            // Pending -> Paid or Failed.
                            // Paid -> Refunded.
                            bool validTransition =
                                (previousStatus == "Pending" &&
                                    (newStatus == "Paid" ||
                                     newStatus == "Failed"))
                                ||
                                (previousStatus == "Paid" &&
                                    newStatus == "Refunded");

                            if (!validTransition)
                            {
                                throw new InvalidOperationException(
                                    "This payment status transition is not allowed. " +
                                    "Pending payments can become Paid or Failed; " +
                                    "Paid payments can become Refunded.");
                            }

                            using (SqlCommand command =
                                new SqlCommand(
                                    updateSql,
                                    connection,
                                    transaction))
                            {
                                command.Parameters.Add(
                                    "@NewStatus", SqlDbType.NVarChar, 30
                                ).Value = newStatus;

                                command.Parameters.Add(
                                    "@PaymentId", SqlDbType.Int
                                ).Value = paymentId;

                                command.Parameters.Add(
                                    "@SellerId", SqlDbType.NVarChar, 128
                                ).Value = sellerId;

                                int affectedRows =
                                    command.ExecuteNonQuery();

                                if (affectedRows != 1)
                                {
                                    throw new InvalidOperationException(
                                        "The payment status was not updated.");
                                }
                            }

                            // Record the change in the audit history.
                            using (SqlCommand command =
                                new SqlCommand(
                                    historySql,
                                    connection,
                                    transaction))
                            {
                                command.Parameters.Add(
                                    "@PaymentId", SqlDbType.Int
                                ).Value = paymentId;

                                command.Parameters.Add(
                                    "@PreviousStatus", SqlDbType.NVarChar, 30
                                ).Value = previousStatus;

                                command.Parameters.Add(
                                    "@NewStatus", SqlDbType.NVarChar, 30
                                ).Value = newStatus;

                                command.Parameters.Add(
                                    "@SellerId", SqlDbType.NVarChar, 128
                                ).Value = sellerId;

                                command.Parameters.Add(
                                    "@ChangeNote", SqlDbType.NVarChar, 500
                                ).Value = String.IsNullOrWhiteSpace(changeNote)
                                    ? (object)DBNull.Value
                                    : changeNote;

                                command.ExecuteNonQuery();
                            }

                            // Recalculate the order-level payment status.
                            using (SqlCommand command =
                                new SqlCommand(
                                    syncOrderPaymentSql,
                                    connection,
                                    transaction))
                            {
                                command.Parameters.Add(
                                    "@OrderId", SqlDbType.Int
                                ).Value = orderId;

                                command.ExecuteNonQuery();
                            }

                            // Status, audit history, and overall payment
                            // status commit together.
                            transaction.Commit();

                            resultMessage =
                                "Payment status updated successfully. " +
                                "The audit history and overall payment " +
                                "status have also been updated.";
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
            catch (UnauthorizedAccessException)
            {
                resultMessage =
                    "The payment could not be found under your seller account.";
            }
            catch (InvalidOperationException ex)
            {
                resultMessage = ex.Message;
            }
            catch (Exception)
            {
                resultMessage =
                    "Could not update the payment. No changes were saved. " +
                    "Please try again.";
            }

            BindPayments();
            ShowMessage(resultMessage);
        }

        private void ShowMessage(string message)
        {
            MessagePanel.Visible = true;
            MessageLabel.Text = HttpUtility.HtmlEncode(message);
        }
    }
}