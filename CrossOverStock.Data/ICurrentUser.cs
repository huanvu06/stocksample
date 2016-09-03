using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossOverStock.Data
{
    public interface ICurrentUser
    {
        List<int> RoleIds { get; set; }
        int UserId { get; set; }
        string UserName { get; set; }
        string FullName { get; set; }
        string Email { get; set; }
        bool IsDeleted { get; set; }
    }
}