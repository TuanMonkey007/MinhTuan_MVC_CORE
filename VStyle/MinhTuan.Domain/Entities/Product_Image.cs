using MinhTuan.Domain.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Entities
{
    [Table("ProductImages")]
    public class Product_Image:AuditableEntity
    {
        public Guid ProductId { get; set; }
        public string Path { get; set; }
        public bool IsThumbnail { get; set; }
        public byte[]? ImageFeature { get; set; }
        // Navigation property
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
