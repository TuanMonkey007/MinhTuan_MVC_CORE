using MinhTuan.Domain.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Entities
{
    [Table("Carts")]
    public class Cart : AuditableEntity
    {
        public Guid UserId { get; set; }
        public bool IsOrder { get; set; }
        
    }
}
