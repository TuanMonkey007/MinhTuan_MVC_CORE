namespace MinhTuan.API.ViewModels.CartViewModel
{
    public class AddToCartViewModel
    {
        public Guid? CartId { get; set; }
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }   
    }
}
