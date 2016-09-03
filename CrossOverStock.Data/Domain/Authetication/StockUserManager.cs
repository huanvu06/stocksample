using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CrossOverStock.Data.Domain.Authetication
{
    public class StockUserManager : UserManager<StockUser, int>
    {
        public StockUserManager(IUserStore<StockUser, int> store)
            : base(store)
        {
        }

        public static StockUserManager Create(IdentityFactoryOptions<StockUserManager> options, IOwinContext context)
        {
            var manager = new StockUserManager(new UserStore<StockUser, StockRole, int, StockUserLogin, StockUserRole, StockUserClaim>(context.Get<IdentityContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<StockUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 7,
                //RequireNonLetterOrDigit = true,
                //RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<StockUser, int>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<StockUser, int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<StockUser, int>(dataProtectionProvider.Create("OMS Identity Authentication"));
            }

            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class StockSignInManager : SignInManager<StockUser, int>
    {
        public StockSignInManager(StockUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(StockUser user)
        {
            return user.GenerateUserIdentityAsync((StockUserManager)UserManager);
        }

        public static StockSignInManager Create(IdentityFactoryOptions<StockSignInManager> options, IOwinContext context)
        {
            return new StockSignInManager(context.GetUserManager<StockUserManager>(), context.Authentication);
        }
    }
}
