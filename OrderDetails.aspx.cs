
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BookStoreWebForms.Data;
using Microsoft.AspNet.Identity;

namespace BookStoreWebForms
{
    public partial class OrderDetails : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.User == null ||
                Context.User.Identity == null ||
                !Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/SignIn.aspx");
                return;
            }

            if (!IsPostBack)
            {
                int orderId;

                if (!int.TryParse(
                    Request.QueryString["id"], out orderId) ||
                    orderId <= 0)
                {
                    ShowError("Invalid order number.");
                    return;
                }

                LoadOrder(orderId);
            }
        }

        private void LoadOrder(int orderId)
        {
            string userId = User.Identity.GetUserId();

            try
            {
                const string orderSql = @"
                    SELECT
                        o.OrderId,
                        o.OrderDate,
                        o.OrderStatus,
                        o.TotalAmount,
                        o.ShippingAddress,
                        ISNULL(p.PaymentMethod, 'Not recorded')
                            AS PaymentMethod,
                        ISNULL(p.PaymentStatus, 'Not recorded')
                            AS PaymentStatus
                    FROM dbo.Orders o
                    LEFT JOIN dbo.Payments p
                        ON p.OrderId = o.OrderId
                    WHERE o.OrderId = @OrderId
                      AND o.CustomerUserId = @UserId;";

                DataTable orderTable = new DataTable();

                using (SqlConnection connection = Db.GetConnection())
                using (SqlCommand command =
                    new SqlCommand(orderSql, connection))
                using (SqlDataAdapter adapter =
                    new SqlDataAdapter(command))
                {
                    command.Parameters.Add(
                        "@OrderId", SqlDbType.Int).Value = orderId;

                    command.Parameters.Add(
                        "@UserId", SqlDbType.NVarChar, 128
                    ).Value = userId;

                    adapter.Fill(orderTable);
                }

                // Do not reveal whether another customer's order exists.
                if (orderTable.Rows.Count == 0)
                {
                    ShowError(
                        "This order could not be found in your account.");
                    return;
                }

                DataRow order = orderTable.Rows[0];

                lblOrderId.Text = order["OrderId"].ToString();

                lblOrderDate.Text =
                    Convert.ToDateTime(order["OrderDate"])
                        .ToString("dd MMM yyyy, hh:mm tt");

                lblOrderStatus.Text =
                    HttpUtility.HtmlEncode(
                        Convert.ToString(order["OrderStatus"]));

                lblTotalAmount.Text =
                    Convert.ToDecimal(order["TotalAmount"])
                        .ToString("N2");

                lblPaymentMethod.Text =
                    HttpUtility.HtmlEncode(
                        Convert.ToString(order["PaymentMethod"]));

                lblPaymentStatus.Text =
                    HttpUtility.HtmlEncode(
                        Convert.ToString(order["PaymentStatus"]));
                lblOrderStatus.CssClass =
    GetOrderStatusCss(Convert.ToString(order["OrderStatus"]));

                lblPaymentStatus.CssClass =
                    GetPaymentStatusCss(Convert.ToString(order["PaymentStatus"]));

                lblShippingAddress.Text =
                    HttpUtility.HtmlEncode(
                        Convert.ToString(order["ShippingAddress"]));
                LoadOrderItems(orderId);
                LoadSellerPayments(orderId);

                // Show cancellation only when all cancellation conditions are met.
                CancelOrderButton.Visible = CanCustomerCancelOrder(orderId, userId);

                OrderPanel.Visible = true;
                MessagePanel.Visible = false;
            }
            catch (Exception)
            {
                ShowError(
                    "Unable to load the order details. Please try again.");
            }
        }

        private void LoadOrderItems(int orderId)

        {
            const string sql = @"
                SELECT
                    b.Title,
                    b.Author,
                    oi.Quantity,
                    oi.UnitPrice,
                    CAST(oi.Quantity AS DECIMAL(18,2))
                        * oi.UnitPrice AS LineTotal,
                    ISNULL(f.FulfillmentStatus, 'Not available')
                        AS FulfillmentStatus
                FROM dbo.OrderItems oi
                INNER JOIN dbo.Books b
                    ON b.BookId = oi.BookId
                LEFT JOIN dbo.SellerOrderFulfillmentItems fi
                    ON fi.OrderItemId = oi.OrderItemId
                LEFT JOIN dbo.SellerOrderFulfillments f
                    ON f.FulfillmentId = fi.FulfillmentId
                   AND f.OrderId = oi.OrderId
                WHERE oi.OrderId = @OrderId
                ORDER BY oi.OrderItemId;";

            DataTable table = new DataTable();

            using (SqlConnection connection = Db.GetConnection())
            using (SqlCommand command =
                new SqlCommand(sql, connection))
            using (SqlDataAdapter adapter =
                new SqlDataAdapter(command))
            {
                command.Parameters.Add(
                    "@OrderId", SqlDbType.Int).Value = orderId;

                adapter.Fill(table);
            }

            gvItems.DataSource = table;
            gvItems.DataBind();
        }

        private void LoadSellerPayments(int orderId)
        {
            const string sql = @"
                SELECT
                    f.FulfillmentId,
                    seller.UserName AS SellerName,
                    sp.Amount,
                    sp.PaymentMethod,
                    sp.PaymentStatus,
                    sp.UpdatedAt
                FROM dbo.SellerOrderFulfillments f
                INNER JOIN dbo.SellerFulfillmentPayments sp
                    ON sp.FulfillmentId = f.FulfillmentId
                LEFT JOIN dbo.AspNetUsers seller
                    ON seller.Id = f.SellerUserId
                WHERE f.OrderId = @OrderId
                  AND EXISTS
                  (
                      SELECT 1
                      FROM dbo.Orders o
                      WHERE o.OrderId = f.OrderId
                        AND o.CustomerUserId = @UserId
                  )
                ORDER BY f.FulfillmentId;";

            DataTable table = new DataTable();

            using (SqlConnection connection = Db.GetConnection())
            using (SqlCommand command =
                new SqlCommand(sql, connection))
            using (SqlDataAdapter adapter =
                new SqlDataAdapter(command))
            {
                command.Parameters.Add(
                    "@OrderId", SqlDbType.Int).Value = orderId;

                command.Parameters.Add(
                    "@UserId", SqlDbType.NVarChar, 128
                ).Value = User.Identity.GetUserId();

                adapter.Fill(table);
            }

            gvSellerPayments.DataSource = table;
            gvSellerPayments.DataBind();
        }

        private void ShowError(string message)
        {
            OrderPanel.Visible = false;
            MessagePanel.Visible = true;

            // Encode any text before displaying it as HTML.
            MessageLabel.Text = HttpUtility.HtmlEncode(message);
        }

        protected void CancelOrderButton_Click(object sender, EventArgs e)
        {
            if (Context.User == null ||
                Context.User.Identity == null ||
                !Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/SignIn.aspx");
                return;
            }

            int orderId;

            if (!int.TryParse(Request.QueryString["id"], out orderId) ||
                orderId <= 0)
            {
                ShowError("Invalid order number.");
                return;
            }

            string userId = User.Identity.GetUserId();

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
                            // Lock the customer's order so two requests cannot
                            // cancel it and restore its stock simultaneously.
                            string orderStatus = null;

                            const string orderSql = @"
                        SELECT OrderStatus
                        FROM dbo.Orders WITH (UPDLOCK, HOLDLOCK)
                        WHERE OrderId = @OrderId
                          AND CustomerUserId = @UserId;";

                            using (SqlCommand command = new SqlCommand(
                                orderSql, connection, transaction))
                            {
                                command.Parameters.Add(
                                    "@OrderId", SqlDbType.Int).Value = orderId;

                                command.Parameters.Add(
                                    "@UserId", SqlDbType.NVarChar, 128
                                ).Value = userId;

                                object result = command.ExecuteScalar();

                                if (result != null && result != DBNull.Value)
                                {
                                    orderStatus = Convert.ToString(result);
                                }
                            }

                            if (orderStatus == null)
                            {
                                throw new InvalidOperationException(
                                    "This order could not be found in your account.");
                            }

                            if (orderStatus != "Pending" &&
                                orderStatus != "Confirmed")
                            {
                                throw new InvalidOperationException(
                                    "This order can no longer be cancelled online.");
                            }

                            // Lock and inspect all seller fulfillments.
                            DataTable fulfillments = new DataTable();

                            const string fulfillmentSql = @"
                        SELECT FulfillmentId, FulfillmentStatus
                        FROM dbo.SellerOrderFulfillments
                            WITH (UPDLOCK, HOLDLOCK)
                        WHERE OrderId = @OrderId
                        ORDER BY FulfillmentId;";

                            using (SqlCommand command = new SqlCommand(
                                fulfillmentSql, connection, transaction))
                            using (SqlDataAdapter adapter =
                                new SqlDataAdapter(command))
                            {
                                command.Parameters.Add(
                                    "@OrderId", SqlDbType.Int).Value = orderId;

                                adapter.Fill(fulfillments);
                            }

                            if (fulfillments.Rows.Count == 0)
                            {
                                throw new InvalidOperationException(
                                    "Seller fulfillment records were not found. " +
                                    "Please contact the administrator.");
                            }

                            foreach (DataRow row in fulfillments.Rows)
                            {
                                string status = Convert.ToString(
                                    row["FulfillmentStatus"]);

                                if (status != "Pending" &&
                                    status != "Confirmed")
                                {
                                    throw new InvalidOperationException(
                                        "At least one seller has started processing " +
                                        "this order. Online cancellation is not allowed.");
                                }
                            }

                            // Do not cancel automatically if a payment has
                            // already been received or refunded.
                            const string paymentSql = @"
                        SELECT sp.PaymentStatus
                        FROM dbo.SellerFulfillmentPayments sp
                            WITH (UPDLOCK, HOLDLOCK)
                        INNER JOIN dbo.SellerOrderFulfillments f
                            ON f.FulfillmentId = sp.FulfillmentId
                        WHERE f.OrderId = @OrderId;";

                            using (SqlCommand command = new SqlCommand(
                                paymentSql, connection, transaction))
                            {
                                command.Parameters.Add(
                                    "@OrderId", SqlDbType.Int).Value = orderId;

                                using (SqlDataReader reader =
                                    command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        string paymentStatus =
                                            Convert.ToString(reader["PaymentStatus"]);

                                        if (paymentStatus == "Paid" ||
                                            paymentStatus == "Refunded")
                                        {
                                            throw new InvalidOperationException(
                                                "This order has a paid or refunded " +
                                                "seller payment. Please contact the " +
                                                "administrator to resolve the payment " +
                                                "before cancellation.");
                                        }
                                    }
                                }
                            }

                            // Restore each ordered quantity exactly once.
                            // The order lock and transaction protect this operation.
                            const string stockSql = @"
                        UPDATE b
                        SET b.Stock = b.Stock + items.Quantity
                        FROM dbo.Books b
                        INNER JOIN
                        (
                            SELECT BookId, SUM(Quantity) AS Quantity
                            FROM dbo.OrderItems
                            WHERE OrderId = @OrderId
                            GROUP BY BookId
                        ) items
                            ON items.BookId = b.BookId;";

                            using (SqlCommand command = new SqlCommand(
                                stockSql, connection, transaction))
                            {
                                command.Parameters.Add(
                                    "@OrderId", SqlDbType.Int).Value = orderId;

                                command.ExecuteNonQuery();
                            }

                            // Record every seller fulfillment status change.
                            foreach (DataRow row in fulfillments.Rows)
                            {
                                int fulfillmentId = Convert.ToInt32(
                                    row["FulfillmentId"]);

                                string previousStatus = Convert.ToString(
                                    row["FulfillmentStatus"]);

                                const string historySql = @"
                            INSERT INTO
                                dbo.SellerOrderFulfillmentStatusHistory
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
                                    @UserId,
                                    @ChangeNote,
                                    SYSDATETIME()
                                );";

                                using (SqlCommand command = new SqlCommand(
                                    historySql, connection, transaction))
                                {
                                    command.Parameters.Add(
                                        "@FulfillmentId", SqlDbType.Int
                                    ).Value = fulfillmentId;

                                    command.Parameters.Add(
                                        "@PreviousStatus", SqlDbType.NVarChar, 30
                                    ).Value = previousStatus;

                                    command.Parameters.Add(
                                        "@NewStatus", SqlDbType.NVarChar, 30
                                    ).Value = "Cancelled";

                                    command.Parameters.Add(
                                        "@UserId", SqlDbType.NVarChar, 128
                                    ).Value = userId;

                                    command.Parameters.Add(
                                        "@ChangeNote", SqlDbType.NVarChar, 500
                                    ).Value = "Cancelled by customer.";

                                    command.ExecuteNonQuery();
                                }

                                const string updateFulfillmentSql = @"
                            UPDATE dbo.SellerOrderFulfillments
                            SET FulfillmentStatus = 'Cancelled',
                                UpdatedAt = SYSDATETIME()
                            WHERE FulfillmentId = @FulfillmentId
                              AND FulfillmentStatus = @PreviousStatus;";

                                using (SqlCommand command = new SqlCommand(
                                    updateFulfillmentSql, connection, transaction))
                                {
                                    command.Parameters.Add(
                                        "@FulfillmentId", SqlDbType.Int
                                    ).Value = fulfillmentId;

                                    command.Parameters.Add(
                                        "@PreviousStatus", SqlDbType.NVarChar, 30
                                    ).Value = previousStatus;

                                    if (command.ExecuteNonQuery() != 1)
                                    {
                                        throw new InvalidOperationException(
                                            "A seller fulfillment changed during " +
                                            "cancellation. Please try again.");
                                    }
                                }
                            }

                            // Mark the main order cancelled only after all
                            // stock and fulfillment operations have succeeded.
                            const string updateOrderSql = @"
                        UPDATE dbo.Orders
                        SET OrderStatus = 'Cancelled'
                        WHERE OrderId = @OrderId
                          AND CustomerUserId = @UserId
                          AND OrderStatus IN ('Pending', 'Confirmed');";

                            using (SqlCommand command = new SqlCommand(
                                updateOrderSql, connection, transaction))
                            {
                                command.Parameters.Add(
                                    "@OrderId", SqlDbType.Int).Value = orderId;

                                command.Parameters.Add(
                                    "@UserId", SqlDbType.NVarChar, 128
                                ).Value = userId;

                                if (command.ExecuteNonQuery() != 1)
                                {
                                    throw new InvalidOperationException(
                                        "The order status changed. Please reload " +
                                        "the page and try again.");
                                }
                            }

                            transaction.Commit();
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

                // Reload the page after the transaction commits.
                Response.Redirect(
                    "OrderDetails.aspx?id=" + orderId, false);

                Context.ApplicationInstance.CompleteRequest();
            }
            catch (InvalidOperationException ex)
            {
                CancellationInfoLabel.Text =
                    Server.HtmlEncode(ex.Message);
            }
            catch (Exception)
            {
                CancellationInfoLabel.Text =
                    "Cancellation could not be completed. " +
                    "Please reload your order and check its status.";
            }
        }



        private bool CanCustomerCancelOrder(int orderId, string userId)
        {
            const string sql = @"
        SELECT CASE
            WHEN o.OrderStatus IN ('Pending', 'Confirmed')

             AND EXISTS
             (
                 SELECT 1
                 FROM dbo.SellerOrderFulfillments f
                 WHERE f.OrderId = o.OrderId
             )

             AND NOT EXISTS
             (
                 SELECT 1
                 FROM dbo.SellerOrderFulfillments f
                 WHERE f.OrderId = o.OrderId
                   AND f.FulfillmentStatus NOT IN
                       ('Pending', 'Confirmed')
             )

             AND NOT EXISTS
             (
                 SELECT 1
                 FROM dbo.SellerOrderFulfillments f
                 INNER JOIN dbo.SellerFulfillmentPayments sp
                     ON sp.FulfillmentId = f.FulfillmentId
                 WHERE f.OrderId = o.OrderId
                   AND sp.PaymentStatus IN ('Paid', 'Refunded')
             )

            THEN 1
            ELSE 0
        END
        FROM dbo.Orders o
        WHERE o.OrderId = @OrderId
          AND o.CustomerUserId = @UserId;";

            using (SqlConnection connection = Db.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add(
                    "@OrderId", SqlDbType.Int).Value = orderId;

                command.Parameters.Add(
                    "@UserId", SqlDbType.NVarChar, 128).Value = userId;

                connection.Open();

                object result = command.ExecuteScalar();

                return result != null &&
                       result != DBNull.Value &&
                       Convert.ToInt32(result) == 1;
            }
        }


        private string GetOrderStatusCss(string status)
        {
            switch (status)
            {
                case "Delivered":
                    return "status-badge status-success";

                case "Cancelled":
                    return "status-badge status-danger";

                case "Shipped":
                case "Processing":
                    return "status-badge status-info";

                case "Pending":
                case "Confirmed":
                    return "status-badge status-warning";

                default:
                    return "status-badge status-neutral";
            }
        }

        private string GetPaymentStatusCss(string status)
        {
            switch (status)
            {
                case "Paid":
                    return "status-badge status-success";

                case "Failed":
                case "Refunded":
                    return "status-badge status-danger";

                case "Pending":
                    return "status-badge status-warning";

                default:
                    return "status-badge status-neutral";
            }
        }


    }
}
