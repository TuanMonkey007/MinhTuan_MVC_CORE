namespace MinhTuan.API.ViewModels.OrderViewModel
{
    public class BuyNowViewModel
    {
        public double  Price { get; set; }
        public int Quantity { get; set; }
        public Guid ProductVariantId { get; set; }
        public Guid? UserId { get; set; }
        //  public string Code { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string ShippingAddress { get; set; }
        public string? CustomerNote { get; set; }
        public string? ShopNote { get; set; }
        public Guid PaymentMethod { get; set; }
        public Guid? Status { get; set; }
        public double TotalAmount { get; set; }

        public Guid? VoucherId { get; set; }
        public double ShippingCost { get; set; }
    }
}
