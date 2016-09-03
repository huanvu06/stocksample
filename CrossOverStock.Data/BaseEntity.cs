using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossOverStock.Data
{
    public abstract class BaseEntity
    {        
        public virtual int Id { get; set; }        
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime CreatedOnDate { get; set; }
        public virtual int CreatedByUserId { get; set; }
        public virtual DateTime LastModifiedOnDate { get; set; }
        public virtual int LastModifiedByUserId { get; set; }        
    }
}
