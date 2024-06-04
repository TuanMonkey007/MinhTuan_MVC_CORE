using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.CartDTO
{
    public class CartItemDTO
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
