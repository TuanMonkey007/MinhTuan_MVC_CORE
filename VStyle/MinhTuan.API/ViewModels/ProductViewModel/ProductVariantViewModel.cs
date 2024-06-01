namespace MinhTuan.API.ViewModels.ProductViewModel
{
    public class ProductVariantViewModel
    {
        public Guid ProductId { get; set; }
        public Guid SizeId { get; set; }
        public Guid ColorId { get; set; }
        public float Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
