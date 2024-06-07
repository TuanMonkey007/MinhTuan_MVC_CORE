using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        public CartController(IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ICartService cartService , ICartItemService cartItemService, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _cartService = cartService;
            _cartItemService = cartItemService;
            _userManager = userManager;
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddProductToCart([FromBody] AddToCartViewModel model)
        {
            var response = new ResponseWithMessageDto() { Message = "Thêm thất bại" };
            //Khách lần đầu mua hàng thì tạo mới một giỏ hàng
            if(model.CartId == null)
            {
                var newCart = new Cart() {UserId = Guid.Empty, IsOrder = false};
               await _cartService.CreateAsync(newCart);
                //Thêm mới
                var newCartItem = new CartItem() { CartId = newCart.Id, Quantity = model.Quantity, ProductVariantId = model.ProductVariantId };
                await _cartItemService.CreateAsync(newCartItem);
                response.Message = newCartItem.CartId.ToString();
            }
            else
            {
                var check = _cartItemService.FindByAsync(x => x.ProductVariantId.Equals(model.ProductVariantId) && x.IsDelete !=true);
                await check;
                if (check.Result.Any())
                {
                    var updateCartItem = check.Result.FirstOrDefault();
                    updateCartItem.Quantity += model.Quantity;
                    await _cartItemService.UpdateAsync(updateCartItem);
                    response.Message = updateCartItem.CartId.ToString();
                    return Ok(response);
                }
                //Thêm mới
                var newCartItem = new CartItem() { CartId = (Guid)model.CartId, Quantity = model.Quantity, ProductVariantId = model.ProductVariantId };
                await _cartItemService.CreateAsync(newCartItem);
                response.Message = newCartItem.CartId.ToString();
            }
            
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllCartItemInCart(Guid id)
        {
            var response = await _cartItemService.GetAllCartItemByCartId(id);
            return Ok(response);
        }
        [HttpDelete("soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var response = new ResponseWithMessageDto() { Message = "Xóa thành công" };
            try
            {
                var item = await _cartItemService.GetByIdAsync(id);

                await _cartItemService.SoftDeleteAsync(item);
                return Ok(response);
            }
            catch(Exception e)
            {
                response.Message = e.Message;
                return Ok(e);
            }

        }
        [HttpPut("update-quantity/{id}")]
        public async Task<IActionResult> UpdateQuantity(Guid id, [FromQuery] int quantity )
        {
            var response = new ResponseWithMessageDto() { Message = "Cập nhật thành công" };
            try
            {
                var item = await _cartItemService.GetByIdAsync(id);
                item.Quantity = quantity;
                await _cartItemService.UpdateAsync(item);
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                return Ok(e);
            }
        }
        [HttpGet("get-user-cart-id/{email}")]
        public async Task<IActionResult> GetUserCartId(string email)
        {
            var response = new ResponseWithMessageDto() { Message = "Lấy thất bại" };
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                var userId = user.Id;
               var cart = await _cartService.FindByAsync(x=> x.UserId.ToString() == userId.ToString() && x.IsDelete != true && x.IsOrder !=true);
               
                if(cart.Count() > 0) //nếu chưa có thì tạo giỏ hàng
                {
                    response.Message = cart.FirstOrDefault().Id.ToString();
                    return Ok(response);
                   
                }
                else
                {
                    var newCart = new Cart() { UserId = Guid.Parse(userId) };
                    await _cartService.CreateAsync(newCart);
                    response.Message = newCart.Id.ToString();
                    return Ok(response);
                }
                
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                return BadRequest(e);
            }
        }
    }
}
