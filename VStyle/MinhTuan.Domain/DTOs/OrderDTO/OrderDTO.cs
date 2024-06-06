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
        public Guid? Status { get; set; }
        public double TotalAmount { get; set; }

        public Guid? VoucherId { get; set; }
        public double ShippingCost { get; set; }

    }
}
