using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Repository.CartRepository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(VStyleContext context) : base(context)
        {
        }
    }
}
