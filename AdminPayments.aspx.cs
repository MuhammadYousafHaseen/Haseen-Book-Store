using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using BookStoreWebForms.Data;

namespace BookStoreWebForms
{
    public partial class AdminPayments : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Never load payment information before authorization.
            if (!IsAdmin())
            {
                pnlAdminContent.Visible = false;
                pnlAccessDenied.Visible = true;
                return;
            }

            pnlAccessDenied.Visible = false;
            pnlAdminContent.Visible = true;

            if (!IsPostBack)
            {
                LoadDashboard();
            }
        }

        private bool IsAdmin()
        {
            if (Context.User == null ||
                Context.User.Identity == null ||
                !Context.User.Identity.IsAuthenticated)
            {
                return false;
            }

            string userId = Context.User.Identity.GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            const string sql = @"
                SELECT COUNT(1)
                FROM dbo.AspNetUserRoles ur
                INNER JOIN dbo.AspNetRoles r
                    ON r.Id = ur.RoleId
                WHERE ur.UserId = @UserId
                  AND r.Name = @RoleName;";

            using (SqlConnection connection = Db.GetConnection())
            using (SqlCommand command =
                new SqlCommand(sql, connection))
            {
                command.Parameters.Add(
                    "@UserId", SqlDbType.NVarChar, 128
                ).Value = userId;

                command.Parameters.Add(
                    "@RoleName", SqlDbType.NVarChar, 256
                ).Value = "Admin";

                connection.Open();

                return Convert.ToInt32(
                    command.ExecuteScalar()
                ) > 0;
            }
        }

        private void LoadDashboard()
        {
            lblMessage.Visible = false;

            try
            {
                LoadSummary();
                LoadPayments();
                LoadPaymentHistory();
            }
            catch (Exception)
            {
                // Avoid exposing database details to the browser.
                lblMessage.Text =
                    "Unable to load payment information. " +
                    "Please check the database connection and " +
                    "SQL schema, then try again.";

                lblMessage.Visible = true;
            }
        }

        private void LoadSummary()
        {
            const string sql = @"
                SELECT
                    COUNT(*) AS TotalPayments,
                    COALESCE(SUM(Amount), 0) AS TotalAmount,

                    COALESCE(SUM(
                        CASE WHEN PaymentStatus = 'Pending'
                        THEN 1 ELSE 0 END
                    ), 0) AS PendingCount,

                    COALESCE(SUM(
                        CASE WHEN PaymentStatus = 'Paid'
                        THEN 1 ELSE 0 END
                    ), 0) AS PaidCount,

                    COALESCE(SUM(
                        CASE WHEN PaymentStatus = 'Failed'
                        THEN 1 ELSE 0 END
                    ), 0) AS FailedCount,

                    COALESCE(SUM(
                        CASE WHEN PaymentStatus = 'Refunded'
                        THEN 1 ELSE 0 END
                    ), 0) AS RefundedCount

                FROM dbo.SellerFulfillmentPayments;";

            using (SqlConnection connection = Db.GetConnection())
            using (SqlCommand command =
                new SqlCommand(sql, connection))
            {
                connection.Open();

                using (SqlDataReader reader =
                    command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblTotalPayments.Text =
                            Convert.ToInt32(
                                reader["TotalPayments"]
                            ).ToString("N0");

                        lblTotalAmount.Text =
                            "Rs. " + Convert.ToDecimal(
                                reader["TotalAmount"]
                            ).ToString("N2");

                        lblPending.Text =
                            Convert.ToInt32(
                                reader["PendingCount"]
                            ).ToString("N0");

                        lblPaid.Text =
                            Convert.ToInt32(
                                reader["PaidCount"]
                            ).ToString("N0");

                        lblFailed.Text =
                            Convert.ToInt32(
                                reader["FailedCount"]
                            ).ToString("N0");

                        lblRefunded.Text =
                            Convert.ToInt32(
                                reader["RefundedCount"]
                            ).ToString("N0");
                    }
                }
            }
        }

        private void LoadPayments()
        {
            const string sql = @"
                SELECT
                    f.OrderId,
                    customer.UserName AS CustomerName,
                    seller.UserName AS SellerName,
                    sp.Amount,
                    sp.PaymentMethod,
                    sp.PaymentStatus,
                    sp.TransactionReference,
                    sp.UpdatedAt,
                    latest.ChangeNote AS LastChangeNote

                FROM dbo.SellerFulfillmentPayments sp

                INNER JOIN dbo.SellerOrderFulfillments f
                    ON f.FulfillmentId = sp.FulfillmentId

                INNER JOIN dbo.Orders o
                    ON o.OrderId = f.OrderId

                LEFT JOIN dbo.AspNetUsers customer
                    ON customer.Id = o.CustomerUserId

                LEFT JOIN dbo.AspNetUsers seller
                    ON seller.Id = f.SellerUserId

                OUTER APPLY
                (
                    SELECT TOP (1)
                        h.ChangeNote
                    FROM dbo.SellerFulfillmentPaymentStatusHistory h
                    WHERE h.SellerFulfillmentPaymentId =
                        sp.SellerFulfillmentPaymentId
                    ORDER BY h.ChangedAt DESC, h.HistoryId DESC
                ) latest

                WHERE
                    (@Status = ''
                     OR sp.PaymentStatus = @Status)

                ORDER BY
                    sp.UpdatedAt DESC,
                    f.OrderId DESC;";

            DataTable table = new DataTable();

            using (SqlConnection connection = Db.GetConnection())
            using (SqlCommand command =
                new SqlCommand(sql, connection))
            using (SqlDataAdapter adapter =
                new SqlDataAdapter(command))
            {
                command.Parameters.Add(
                    "@Status", SqlDbType.NVarChar, 20
                ).Value = ddlStatus.SelectedValue;

                adapter.Fill(table);
            }

            gvPayments.DataSource = table;
            gvPayments.DataBind();
        }

        private void LoadPaymentHistory()
        {
            const string sql = @"
                SELECT TOP (100)
                    f.OrderId,
                    seller.UserName AS SellerName,
                    h.PreviousStatus,
                    h.NewStatus,
                    changer.UserName AS ChangedBy,
                    h.ChangeNote,
                    h.ChangedAt

                FROM dbo.SellerFulfillmentPaymentStatusHistory h

                INNER JOIN dbo.SellerFulfillmentPayments sp
                    ON sp.SellerFulfillmentPaymentId =
                        h.SellerFulfillmentPaymentId

                INNER JOIN dbo.SellerOrderFulfillments f
                    ON f.FulfillmentId = sp.FulfillmentId

                LEFT JOIN dbo.AspNetUsers seller
                    ON seller.Id = f.SellerUserId

                LEFT JOIN dbo.AspNetUsers changer
                    ON changer.Id = h.ChangedByUserId

                ORDER BY
                    h.ChangedAt DESC,
                    h.HistoryId DESC;";

            DataTable table = new DataTable();

            using (SqlConnection connection = Db.GetConnection())
            using (SqlCommand command =
                new SqlCommand(sql, connection))
            using (SqlDataAdapter adapter =
                new SqlDataAdapter(command))
            {
                adapter.Fill(table);
            }

            gvHistory.DataSource = table;
            gvHistory.DataBind();
        }

        protected void btnFilter_Click(
            object sender, EventArgs e)
        {
            if (!IsAdmin())
            {
                pnlAdminContent.Visible = false;
                pnlAccessDenied.Visible = true;
                return;
            }

            pnlAdminContent.Visible = true;
            pnlAccessDenied.Visible = false;

            try
            {
                LoadPayments();
                lblMessage.Visible = false;
            }
            catch (Exception)
            {
                lblMessage.Text =
                    "Unable to filter payment records.";

                lblMessage.Visible = true;
            }
        }

        protected void btnReset_Click(
            object sender, EventArgs e)
        {
            if (!IsAdmin())
            {
                pnlAdminContent.Visible = false;
                pnlAccessDenied.Visible = true;
                return;
            }

            ddlStatus.SelectedValue = "";

            try
            {
                LoadPayments();
                lblMessage.Visible = false;
            }
            catch (Exception)
            {
                lblMessage.Text =
                    "Unable to load payment records.";

                lblMessage.Visible = true;
            }
        }

        protected void gvPayments_RowDataBound(
            object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            Label statusLabel =
                e.Row.FindControl("lblRowStatus") as Label;

            if (statusLabel == null)
            {
                return;
            }

            string status = Convert.ToString(
                DataBinder.Eval(
                    e.Row.DataItem, "PaymentStatus"
                )
            );

            // Use fixed CSS classes, never a database value
            // as a CSS class or HTML fragment.
            switch (status)
            {
                case "Paid":
                    statusLabel.CssClass = "badge status-paid";
                    break;

                case "Pending":
                    statusLabel.CssClass = "badge status-pending";
                    break;

                case "Failed":
                    statusLabel.CssClass = "badge status-failed";
                    break;

                case "Refunded":
                    statusLabel.CssClass = "badge status-refunded";
                    break;

                default:
                    statusLabel.CssClass = "badge";
                    break;
            }
        }
    }
}