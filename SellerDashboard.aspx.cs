
using System;

namespace BookStoreWebForms
{
    public partial class SellerDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/SignIn.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!User.IsInRole("Seller"))
            {
                Response.StatusCode = 403;
                Response.TrySkipIisCustomErrors = true;

                MessageLabel.CssClass = "alert alert-danger d-block";
                MessageLabel.Text =
                    "Access denied. You must have the Seller role " +
                    "to access this page.";

                return;
            }

            if (!IsPostBack)
            {
                MessageLabel.CssClass = "alert alert-success d-block";
                MessageLabel.Text = "Welcome to your Seller Dashboard!";
            }
        }
    }
}
