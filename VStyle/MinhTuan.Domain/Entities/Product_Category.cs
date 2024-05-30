using MinhTuan.Domain.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Entities
{
    [Table("ProductCategories")]
    public class Product_Category:AuditableEntity
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        // Navigation properties
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("CategoryId")]
        public virtual DataCategory Category { get; set; }

    }
}
