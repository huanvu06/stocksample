using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossOverStock.Data.Impl
{
    public class CurrentUser:ICurrentUser
    {
        public List<int> RoleIds { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
    }
}
