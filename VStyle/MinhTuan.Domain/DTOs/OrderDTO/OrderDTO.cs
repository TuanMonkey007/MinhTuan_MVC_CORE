using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.OrderDTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string ShippingAddress { get; set; }
        public string? CustomerNote { get; set; }
        public string? ShopNote { get; set; }
        public Guid PaymentMethod { get; set; }
        public string? PaymentMethodName { get; set; }
        public Guid? Status { get; set; }
        public string? StatusName { get; set; }
        public double TotalAmount { get; set; }

        public Guid? VoucherId { get; set; }
        public double ShippingCost { get; set; }
        public Guid CartId { get; set; }
        public DateTime? CreatedDate { get; set; }
        //Thông tin người dùng (nếu có) 
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string?  UserPhoneNumber { get; set; }
        public string?  UserAddress { get; set; }
        public bool? IsCancelled { get; set; }
        public string? ReasonCancelled { get; set; }
    }
}
