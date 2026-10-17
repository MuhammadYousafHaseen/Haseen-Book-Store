using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using BookStoreWebForms.Data;
using Microsoft.AspNet.Identity;

namespace BookStoreWebForms
{
    public partial class MyOrders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/SignIn.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindOrders();
            }
        }

        private void BindOrders()
        {
            string userId = User.Identity.GetUserId();

            const string sql = @"
                SELECT
                    o.OrderId,
                    o.OrderDate,
                    o.TotalAmount,
                    o.ShippingAddress,
                    o.OrderStatus,

                    ISNULL(p.PaymentMethod, 'Not recorded')
                        AS PaymentMethod,

                    ISNULL(p.PaymentStatus, 'Not recorded')
                        AS PaymentStatus,

                    ISNULL(
                        STUFF(
                            (
                                SELECT
                                    N'; ' + b.Title
                                    + N' (x'
                                    + CAST(oi.Quantity AS NVARCHAR(10))
                                    + N') - Fulfillment: '
                                    + ISNULL(
                                        f.FulfillmentStatus,
                                        N'Not available'
                                    )
                                FROM dbo.OrderItems oi
                                INNER JOIN dbo.Books b
                                    ON b.BookId = oi.BookId

                                LEFT JOIN dbo.SellerOrderFulfillmentItems fi
                                    ON fi.OrderItemId = oi.OrderItemId

                                LEFT JOIN dbo.SellerOrderFulfillments f
                                    ON f.FulfillmentId = fi.FulfillmentId
                                   AND f.OrderId = oi.OrderId

                                WHERE oi.OrderId = o.OrderId

                                ORDER BY oi.OrderItemId

                                FOR XML PATH(''), TYPE
                            ).value('.', 'NVARCHAR(MAX)'),
                            1, 2, ''
                        ),
                        'Book details unavailable'
                    ) AS Books

                FROM dbo.Orders o

                LEFT JOIN dbo.Payments p
                    ON p.OrderId = o.OrderId

                WHERE o.CustomerUserId = @UserId

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
                        "@UserId", SqlDbType.NVarChar, 128
                    ).Value = userId;

                    adapter.Fill(table);

                    // Add the payment breakdown column before using it.
                    table.Columns.Add(
                        "SellerPaymentsHtml",
                        typeof(string));
                }

                // Retrieve seller-level payments for these orders.
                // Only orders belonging to the signed-in customer are used.
                const string paymentSql = @"
                    SELECT
                        f.OrderId,
                        f.FulfillmentId,
                        sp.Amount,
                        sp.PaymentMethod,
                        sp.PaymentStatus
                    FROM dbo.SellerFulfillmentPayments sp
                    INNER JOIN dbo.SellerOrderFulfillments f
                        ON f.FulfillmentId = sp.FulfillmentId
                    INNER JOIN dbo.Orders o
                        ON o.OrderId = f.OrderId
                    WHERE o.CustomerUserId = @UserId
                    ORDER BY f.OrderId DESC, f.FulfillmentId;";

                DataTable paymentTable = new DataTable();

                using (SqlConnection connection = Db.GetConnection())
                using (SqlCommand command =
                    new SqlCommand(paymentSql, connection))
                using (SqlDataAdapter adapter =
                    new SqlDataAdapter(command))
                {
                    command.Parameters.Add(
                        "@UserId", SqlDbType.NVarChar, 128
                    ).Value = userId;

                    adapter.Fill(paymentTable);
                }

                // Prepare one safe HTML breakdown per order.
                foreach (DataRow orderRow in table.Rows)
                {
                    int orderId = Convert.ToInt32(
                        orderRow["OrderId"]);

                    StringBuilder html = new StringBuilder();
                    bool foundPayment = false;

                    foreach (DataRow paymentRow in paymentTable.Rows)
                    {
                        int paymentOrderId = Convert.ToInt32(
                            paymentRow["OrderId"]);

                        if (paymentOrderId != orderId)
                        {
                            continue;
                        }

                        foundPayment = true;

                        html.Append("<tr><td>");
                        html.Append(HttpUtility.HtmlEncode(
                            Convert.ToString(
                                paymentRow["FulfillmentId"])));
                        html.Append("</td><td>Rs. ");

                        html.Append(
                            Convert.ToDecimal(
                                paymentRow["Amount"]).ToString("N2"));

                        html.Append("</td><td>");
                        html.Append(HttpUtility.HtmlEncode(
                            Convert.ToString(
                                paymentRow["PaymentMethod"])));

                        html.Append("</td><td>");

                        string status = Convert.ToString(
                            paymentRow["PaymentStatus"]);

                        string badgeClass = "bg-secondary";

                        switch (status)
                        {
                            case "Paid":
                                badgeClass = "bg-success";
                                break;

                            case "Pending":
                                badgeClass = "bg-warning text-dark";
                                break;

                            case "Failed":
                                badgeClass = "bg-danger";
                                break;

                            case "Refunded":
                                badgeClass = "bg-info text-dark";
                                break;
                        }

                        html.Append("<span class=\"badge ");
                        html.Append(badgeClass);
                        html.Append("\">");

                        html.Append(HttpUtility.HtmlEncode(status));

                        html.Append("</span></td></tr>");
                    }

                    if (!foundPayment)
                    {
                        html.Append(
                            "<tr><td colspan=\"4\" class=\"text-muted\">" +
                            "Seller-level payment details are not available " +
                            "for this order.</td></tr>");
                    }

                    orderRow["SellerPaymentsHtml"] = html.ToString();
                }


                OrdersRepeater.DataSource = table;
                OrdersRepeater.DataBind();

                EmptyOrdersPanel.Visible = table.Rows.Count == 0;
                MessagePanel.Visible = false;
            }
            catch (Exception)
            {
                OrdersRepeater.DataSource = null;
                OrdersRepeater.DataBind();

                EmptyOrdersPanel.Visible = false;
                MessagePanel.Visible = true;

                MessageLabel.Text =
                    "We could not load your orders. Please try again.";
            }
        }

        protected string GetOrderStatusCss(object value)
        {
            string status = Convert.ToString(value);

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

        protected string GetPaymentStatusCss(object value)
        {
            string status = Convert.ToString(value);

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