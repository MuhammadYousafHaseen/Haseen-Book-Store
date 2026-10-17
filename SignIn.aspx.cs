
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Web;

namespace BookStoreWebForms
{
    public partial class SignIn : System.Web.UI.Page
    {
        protected void SignInButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string email = EmailTextBox.Text.Trim();
            string password = PasswordTextBox.Text;

            var manager = Context.GetOwinContext()
                .GetUserManager<Models.ApplicationUserManager>();

            var user = manager.FindByEmailAsync(email)
                .GetAwaiter().GetResult();

            if (user == null)
            {
                ShowError();
                return;
            }

            var validPassword = manager.CheckPasswordAsync(user, password)
                .GetAwaiter().GetResult();

            if (!validPassword)
            {
                ShowError();
                return;
            }

            // Check roles before signing in.
            bool isAdmin = manager.IsInRoleAsync(user.Id, "Admin")
                .GetAwaiter().GetResult();

            bool isSeller = manager.IsInRoleAsync(user.Id, "Seller")
                .GetAwaiter().GetResult();

            bool isCustomer = manager.IsInRoleAsync(user.Id, "Customer")
                .GetAwaiter().GetResult();

            // Sign in using the existing ASP.NET Identity cookie.
            var identity = manager.CreateIdentity(
                user,
                DefaultAuthenticationTypes.ApplicationCookie);

            Context.GetOwinContext().Authentication.SignIn(
                new AuthenticationProperties
                {
                    IsPersistent = false
                },
                identity);

            // Admin takes priority if an account has multiple roles.
            string destination;

            if (isAdmin)
            {
                destination = "AdminDashboard.aspx";
            }
            else if (isSeller)
            {
                destination = "SellerDashboard.aspx";
            }
            else if (isCustomer)
            {
                destination = "MyAccount.aspx";
            }
            else
            {
                // Safe fallback for accounts without an assigned role.
                destination = "Default.aspx";
            }

            Response.Redirect(destination, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void ShowError()
        {
            MessageLabel.CssClass = "text-danger d-block mb-3";
            MessageLabel.Text = "Invalid email or password.";
        }
    }
}
