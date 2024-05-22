

using System.ComponentModel.DataAnnotations;

namespace MinhTuan.API.ViewModels.CategoryViewModel
{
    public class CreateCategoryViewModel
    {
        [Required]
        public string  Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
