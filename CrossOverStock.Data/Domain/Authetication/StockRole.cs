using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossOverStock.Data.Domain.Authetication
{
    public class StockRole : IdentityRole<int, StockUserRole>
    {
        public StockRole() : base()
        {
        }
        
    }

    public class StockUserRole : IdentityUserRole<int>
    {
        public StockUserRole() : base() { }
    }
}
