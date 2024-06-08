using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.OrderDTO
{
    public class OrderItemDTO
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string?  ColorName { get; set; }
        public string?  SizeName { get; set; }
      
        public  string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public string? ThumbnailPath { get; set; }
        public string? ThumbnailBase64 { get; set; }
        public string? ThumbnailContentType { get; set; }
    }
}
