using System.ComponentModel.DataAnnotations.Schema;

namespace MinhTuan.API.ViewModels.BannerViewModel
{
    public class BannerViewModel
    {
        public Guid CategoryId { get; set; }
        public int OrderDisplay { get; set; }
        public bool IsDisplay { get; set; }
        public IFormFile BannerImage { get; set; }
    }
}
