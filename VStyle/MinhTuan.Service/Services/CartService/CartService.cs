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
    public class CartService : Service<Cart>, ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
