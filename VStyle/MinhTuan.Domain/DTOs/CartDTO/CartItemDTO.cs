using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.CartDTO
{
    public class CartItemDTO
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductVariantId { get; set; }
        
        public int Quantity { get; set; }

        public string? ThumbnailPath { get; set; }
        public string? ThumbnailBase64 { get; set; }
        public string?  ThumbnailContentType { get; set; }
        public string? ProductName { get; set; }
        public float? ProductPrice { get; set; }
        public string? SizeName { get; set; }
        public string?  ColorName { get; set; }
    }
}
