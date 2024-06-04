using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.Entities;
using MinhTuan.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.CartService
{
    public class CartItemService : Service<CartItem>, ICartItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartItemService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
