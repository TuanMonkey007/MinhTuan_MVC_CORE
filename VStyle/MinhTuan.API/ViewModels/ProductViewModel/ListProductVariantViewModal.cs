using Newtonsoft.Json;

namespace MinhTuan.API.ViewModels.ProductViewModel
{
    public class ListProductVariantViewModal
    {
        [JsonProperty("listModel")]
        public List<ProductVariantViewModel>? listModel { get; set; }
    }
}
