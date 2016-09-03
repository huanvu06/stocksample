using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CrossOverStock.Data.Domain.Authetication
{
    public class StockUser : IdentityUser<int, StockUserLogin, StockUserRole, StockUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(StockUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string OTPPassword { get; set; }
        public DateTime? OTPGeneratedOn { get; set; }
        public int LockCount { get; set; }
        public bool IsDeleted { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class StockUserLogin : IdentityUserLogin<int>
    {
    }
    public class StockUserClaim : IdentityUserClaim<int> { }
}
