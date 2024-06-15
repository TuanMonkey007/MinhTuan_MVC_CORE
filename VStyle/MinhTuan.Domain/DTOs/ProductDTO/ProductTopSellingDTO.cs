using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.ProductDTO
{
    public class ProductTopSellingDTO
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int TotalQuantitySold { get; set; }
        public string? ThumbnailPath   { get; set; }
        public string? ThumbnailBase64 { get; set; }
        public string? ThumbnailContentType { get; set; }


    }
}
