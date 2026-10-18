
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace BookStoreWebForms
{
    public partial class AdminDashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Only authenticated administrators may access this page.
            if (Context.User == null ||
                Context.User.Identity == null ||
                !Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/SignIn.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!Context.User.IsInRole("Admin"))
            {
                Response.Redirect("~/Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                LoadDashboard();
            }
        }

        private void LoadDashboard()
        {
            try
            {
                lblTotalBooks.Text =
                    GetCount("SELECT COUNT(*) FROM dbo.Books").ToString("N0");

                lblTotalCategories.Text =
                    GetCount("SELECT COUNT(*) FROM dbo.Categories").ToString("N0");

                lblTotalOrders.Text =
                    GetCount("SELECT COUNT(*) FROM dbo.Orders").ToString("N0");

                lblTotalUsers.Text =
                    GetCount("SELECT COUNT(*) FROM dbo.AspNetUsers").ToString("N0");

                lblTotalPayments.Text =
                    GetCount("SELECT COUNT(*) FROM dbo.Payments").ToString("N0");
            }
            catch (Exception)
            {
                // Do not expose database details to website visitors.
                lblError.Text =
                    "Dashboard data could not be loaded. Please check the database configuration and table names.";
                lblError.Visible = true;
            }
        }

        private int GetCount(string query)
        {
            // Uses the existing shared database helper.
            using (SqlConnection connection =
                Data.Db.GetConnection())
            using (SqlCommand command =
                new SqlCommand(query, connection))
            {
                connection.Open();

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
}
