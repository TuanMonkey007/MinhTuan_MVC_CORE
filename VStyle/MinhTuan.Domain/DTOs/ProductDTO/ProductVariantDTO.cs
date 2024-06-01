using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.ProductDTO
{
    public class ProductVariantDTO
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid SizeId { get; set; }
        public Guid ColorId { get; set; }
        public float Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
