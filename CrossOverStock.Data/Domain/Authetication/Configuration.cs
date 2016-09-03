using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossOverStock.Data.Domain.Authetication
{
    internal sealed class Configuration : DbMigrationsConfiguration<IdentityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}
