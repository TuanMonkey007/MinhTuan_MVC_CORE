using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.ProductDTO
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        public int? StockQuantity { get; set; }
        public string? Thumbnail { get; set; }
        public string? ThumbnailBase64 { get; set; } // Chuỗi base64 của ảnh

        public string? ThumbnailContentType { get; set; } // Loại nội dung của ảnh (ví dụ: image/jpeg)
    }
}
