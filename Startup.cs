
using System;
using BookStoreWebForms.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(BookStoreWebForms.Startup))]

namespace BookStoreWebForms
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<ApplicationDbContext>(
                () => new ApplicationDbContext());


            app.CreatePerOwinContext<ApplicationRoleManager>(
                ApplicationRoleManager.Create);


            app.CreatePerOwinContext<ApplicationUserManager>(
                ApplicationUserManager.Create);

            app.UseCookieAuthentication(
                new CookieAuthenticationOptions
                {
                    AuthenticationType =
                        DefaultAuthenticationTypes.ApplicationCookie,

                    LoginPath = new PathString("/SignIn.aspx"),

                    ExpireTimeSpan = TimeSpan.FromHours(8)
                });
        }
    }
}
