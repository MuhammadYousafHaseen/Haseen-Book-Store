
using BookStoreWebForms.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Web;

namespace BookStoreWebForms
{
    public partial class SignUp : System.Web.UI.Page
    {
        protected void SignUpButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            var roleManager = Context.GetOwinContext()
                .GetUserManager<ApplicationRoleManager>();

            // Ensure the Customer and Seller roles exist.
            if (!roleManager.RoleExists("Customer"))
            {
                var roleResult = roleManager.Create(
                    new IdentityRole("Customer"));

                if (!roleResult.Succeeded)
                {
                    ShowError("Unable to create the Customer role.");
                    return;
                }
            }

            if (!roleManager.RoleExists("Seller"))
            {
                var roleResult = roleManager.Create(
                    new IdentityRole("Seller"));

                if (!roleResult.Succeeded)
                {
                    ShowError("Unable to create the Seller role.");
                    return;
                }
            }

            string name = NameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();

            var manager = Context.GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = name
            };

            var result = manager.CreateAsync(
                user,
                PasswordTextBox.Text
            ).GetAwaiter().GetResult();

            if (result.Succeeded)
            {
                var roleResult = manager.AddToRole(
                    user.Id,
                    "Customer"
                );

                if (!roleResult.Succeeded)
                {
                    ShowError(
                        "Your account was created, but Customer role " +
                        "assignment failed. Please contact support."
                    );
                    return;
                }

                Response.Redirect("SignIn.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            ShowError(string.Join(" ", result.Errors));
        }

        private void ShowError(string message)
        {
            MessageLabel.CssClass = "text-danger d-block mb-3";
            MessageLabel.Text = Server.HtmlEncode(message);
        }
    }
}
