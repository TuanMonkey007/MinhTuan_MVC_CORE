using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.BannerDTO
{
    public class BannerDTO
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public Guid CategoryId { get; set; }
        public string?  CategoryName { get; set; }
        public int OrderDisplay { get; set; }
        public bool IsDisplay { get; set; }
        public string? BannerBase64 { get; set; }
        public string?  BannerContentType { get; set; }
    }
}
