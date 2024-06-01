using MinhTuan.Domain.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Entities
{
    [Table("ProductVariants")]
    public class Product_Variant:AuditableEntity
    {
        public Guid ProductId { get; set; }
        public Guid SizeId { get; set; }
        public Guid ColorId { get; set; }
        public float Price { get; set; }
        public int StockQuantity { get; set; }
        // Navigation property
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        // Navigation property
        [ForeignKey("SizeId")]
        public virtual DataCategory Size { get; set; }
        [ForeignKey("ColorId")]
        public virtual DataCategory Color { get; set; }

    }
}
