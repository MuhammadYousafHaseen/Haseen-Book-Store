
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.ComponentModel.DataAnnotations;

namespace BookStoreWebForms.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string FullName { get; set; }
    }

    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("BookStoreDB")
        {
        }
    }

    public class ApplicationUserManager
        : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(
            IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(
            IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(
                new UserStore<ApplicationUser>(
                    context.Get<ApplicationDbContext>()));

            manager.UserValidator = new UserValidator<ApplicationUser>(
                manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            return manager;
        }
    }


    public class ApplicationRoleManager
        : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(
            RoleStore<IdentityRole> store)
            : base(store)
        {
        }


        public static ApplicationRoleManager Create(
            IdentityFactoryOptions<ApplicationRoleManager> options,
            IOwinContext context)
        {
            return new ApplicationRoleManager(
                new RoleStore<IdentityRole>(
                    context.Get<ApplicationDbContext>()));
        }
    }
}
