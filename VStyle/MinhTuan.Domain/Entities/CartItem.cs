using MinhTuan.Domain.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Entities
{
    [Table("CartItems")]
    public class CartItem :AuditableEntity
    {
        public Guid CartId { get; set; }
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }
        [ForeignKey("ProductVariantId")]
        public virtual Product_Variant ProductVariant { get; set; }

    }
}
