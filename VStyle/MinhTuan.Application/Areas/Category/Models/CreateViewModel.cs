using System.ComponentModel.DataAnnotations;

namespace MinhTuan.Application.Areas.Category.Models
{
    public class CreateViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mã danh mục")]
        public string  Code { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        public string  Name  { get; set; }
        public string  Description { get; set; }
    }
}
