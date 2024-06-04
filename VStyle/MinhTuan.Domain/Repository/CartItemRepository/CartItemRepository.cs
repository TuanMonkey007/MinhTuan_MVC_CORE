using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Repository.CartItemRepository
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(VStyleContext context) : base(context)
        {
        }
    }
}
