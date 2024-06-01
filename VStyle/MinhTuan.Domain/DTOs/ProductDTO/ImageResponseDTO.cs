using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.ProductDTO
{
    public class ImageResponseDTO
    {
        public string? Path { get; set; }
        public string? Base64 { get; set; }
        public string? ContentType { get; set; }
    }
}
