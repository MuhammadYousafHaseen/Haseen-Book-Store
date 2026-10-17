
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Web;

namespace BookStoreWebForms
{
    public partial class SignOut : System.Web.UI.Page
    {
        protected void SignOutButton_Click(object sender, EventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(
                DefaultAuthenticationTypes.ApplicationCookie);

            Response.Redirect("SignIn.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
