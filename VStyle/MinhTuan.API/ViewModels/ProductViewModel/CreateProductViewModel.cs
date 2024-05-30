using System.ComponentModel.DataAnnotations;

namespace MinhTuan.API.ViewModels.ProductViewModel
{
    public class CreateProductViewModel
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        public IFormFile? ThumbnailFile { get; set; }
        public List<IFormFile>? ListImageFile { get; set; }
        public List<Guid> ListCategory { get; set; }
        // public int StockQuantity { get; set; } số lượng tồn được cập nhật từ các bản nhỏ
    }
}
