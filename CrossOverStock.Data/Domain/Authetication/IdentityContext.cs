using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossOverStock.Data.Domain.Authetication
{
    public class IdentityContext : IdentityDbContext<StockUser, StockRole, int, StockUserLogin, StockUserRole, StockUserClaim>
    {
        public IdentityContext() : base("StockContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<StockRole>().Property(x => x.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            
            base.OnModelCreating(modelBuilder);
        }

        //public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        //public virtual DbSet<Client> Clients { get; set; }
        //public virtual DbSet<Endpoint> Endpoints { get; set; }
        //public virtual DbSet<Permission> Permissions { get; set; }
        //public virtual DbSet<PermissionRole> PermissionRoles { get; set; }

        public static IdentityContext Create()
        {
            return new IdentityContext();
        }
    }
}
