using MinhTuan.Domain.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Entities
{
    [Table("Products")]
    public class Product :AuditableEntity
    {
        public string Code { get; set; }
        public string? Name { get; set; }
        public string? Description  { get; set; }
        public float Price { get; set; }
        public bool? IsDisplay { get; set; }
        public int? StockQuantity { get; set; }
        //  public Guid CategoryId { get; set; } // Thêm một bảng chứa danh mục - sản phẩm


    }
}
