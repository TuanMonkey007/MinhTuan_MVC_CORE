using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.CartDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Service.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.CartService
{
    public interface ICartItemService : IService<CartItem> 
    {
        Task<ResponseWithDataDto<List<CartItemDTO>>> GetAllCartItemByCartId(Guid id);
    }
}
