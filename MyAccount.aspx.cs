
using BookStoreWebForms.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Web;

namespace BookStoreWebForms
{
    public partial class MyAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("SignIn.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                var manager = Context.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();

                var userId = User.Identity.GetUserId();

                var user = manager.FindByIdAsync(userId)
                    .GetAwaiter().GetResult();

                if (user == null)
                {
                    Context.GetOwinContext().Authentication.SignOut(
                        DefaultAuthenticationTypes.ApplicationCookie);

                    Response.Redirect("SignIn.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                FullNameLabel.Text = Server.HtmlEncode(user.FullName);
                EmailLabel.Text = Server.HtmlEncode(user.Email);
            }
        }
    }
}
