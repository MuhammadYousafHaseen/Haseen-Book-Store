
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace BookStoreWebForms
{
    public partial class SignIn : Page
    {
        protected void SignInButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            MessageLabel.Text = string.Empty;
            MessageLabel.CssClass = "signin-message";

            try
            {
                SignInUser();
            }
            catch (SqlException)
            {
                ShowMessage(
                    "The database is currently unavailable. Please check the database connection configuration and try again.",
                    true);
            }
            catch (System.Configuration.ConfigurationErrorsException)
            {
                ShowMessage(
                    "The application database configuration is missing or invalid. Please contact the administrator.",
                    true);
            }
            catch (InvalidOperationException)
            {
                ShowMessage(
                    "Sign-in is temporarily unavailable. Please check the application configuration.",
                    true);
            }
        }

        private void SignInUser()
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordTextBox.Text;

            var owinContext = Context.GetOwinContext();

            var manager = owinContext.GetUserManager<
                Models.ApplicationUserManager>();

            if (manager == null)
            {
                ShowMessage(
                    "The sign-in service is not configured correctly.",
                    true);
                return;
            }

            var user = manager.FindByEmailAsync(email)
                .GetAwaiter()
                .GetResult();

            if (user == null)
            {
                ShowInvalidCredentials();
                return;
            }

            bool validPassword = manager.CheckPasswordAsync(
                user, password)
                .GetAwaiter()
                .GetResult();

            if (!validPassword)
            {
                ShowInvalidCredentials();
                return;
            }

            bool isAdmin = manager.IsInRoleAsync(
                user.Id, "Admin")
                .GetAwaiter()
                .GetResult();

            bool isSeller = manager.IsInRoleAsync(
                user.Id, "Seller")
                .GetAwaiter()
                .GetResult();

            bool isCustomer = manager.IsInRoleAsync(
                user.Id, "Customer")
                .GetAwaiter()
                .GetResult();

            // Refuse sign-in if the account has no recognized role.
            if (!isAdmin && !isSeller && !isCustomer)
            {
                ShowMessage(
                    "Your account does not have an assigned role. Please contact the administrator.",
                    true);
                return;
            }

            // Create the authenticated identity only after validation.
            var identity = manager.CreateIdentity(
                user,
                DefaultAuthenticationTypes.ApplicationCookie);

            if (identity == null)
            {
                ShowMessage(
                    "Unable to create your sign-in session. Please try again.",
                    true);
                return;
            }

            owinContext.Authentication.SignIn(
                new AuthenticationProperties
                {
                    IsPersistent = false
                },
                identity);

            // Admin takes priority if an account has multiple roles.
            string destination;

            if (isAdmin)
            {
                destination = "~/AdminDashboard.aspx";
            }
            else if (isSeller)
            {
                destination = "~/SellerDashboard.aspx";
            }
            else
            {
                destination = "~/MyAccount.aspx";
            }

            Response.Redirect(destination, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void ShowInvalidCredentials()
        {
            ShowMessage("Invalid email or password.", true);
        }

        private void ShowMessage(string message, bool isError)
        {
            MessageLabel.Text = HttpUtility.HtmlEncode(message);

            MessageLabel.CssClass = isError
                ? "signin-message text-danger bg-light border border-danger"
                : "signin-message";
        }
    }
}
