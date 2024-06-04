using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MinhTuan.API.ViewModels.CartViewModel;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Service.Services.CartService;
using MinhTuan.Service.Services.ProductService;

namespace MinhTuan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly ICartItemService _cartItemService;

        public CartController(IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ICartService cartService , ICartItemService cartItemService)
        {
            _httpContextAccessor = httpContextAccessor;
            _cartService = cartService;
            _cartItemService = cartItemService;
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddProductToCart([FromBody] AddToCartViewModel model)
        {
            var response = new ResponseWithMessageDto() { Message = "Thêm thất bại" };
            //Khách lần đầu mua hàng thì tạo mới một giỏ hàng
            if(model.CartId == null)
            {
                var newCart = new Cart() {UserId = Guid.Empty};
               await _cartService.CreateAsync(newCart);
                //Thêm mới
                var newCartItem = new CartItem() { CartId = newCart.Id, Quantity = model.Quantity, ProductVariantId = model.ProductVariantId };
                await _cartItemService.CreateAsync(newCartItem);
                response.Message = newCartItem.CartId.ToString();
            }
            else
            {
                //Thêm mới
                var newCartItem = new CartItem() { CartId = (Guid)model.CartId, Quantity = model.Quantity, ProductVariantId = model.ProductVariantId };
                await _cartItemService.CreateAsync(newCartItem);
                response.Message = newCartItem.CartId.ToString();
            }
            
            return Ok(response);
        }
    }
}
