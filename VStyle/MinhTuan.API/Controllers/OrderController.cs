using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MinhTuan.API.ViewModels.CategoryViewModel;
using MinhTuan.API.ViewModels.OrderViewModel;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.DTOs.OrderDTO;
using MinhTuan.Domain.DTOs.ProductDTO;
using MinhTuan.Domain.DTOs.StatisticDTO;
using MinhTuan.Domain.DTOs.VNPayDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Domain.Repository.ProductRepository;
using MinhTuan.Service.SearchDTO;
using MinhTuan.Service.Services.CartService;
using MinhTuan.Service.Services.CategoryService;
using MinhTuan.Service.Services.DataCategoryService;
using MinhTuan.Service.Services.OrderItemService;
using MinhTuan.Service.Services.OrderService;
using MinhTuan.Service.Services.PaymentInfoService;
using MinhTuan.Service.Services.ProductService;
using MinhTuan.Service.Services.VNPAY;
using MinhTuan.Service.Services.VoucherService;
using Newtonsoft.Json;
using System.Net.WebSockets;

namespace MinhTuan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICartService _cartService;
        private readonly IVoucherService _voucherService;
        private readonly IOrderService _orderService;
        private readonly ICartItemService _cartItemService;
        private readonly IOrderItemService _orderItemService;
        private readonly IDataCategoryService _dataCategoryService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IVnPayService _vnpayService;
        private readonly IConfiguration _config;
        private readonly IPaymentInfoService _paymentInfoService;
        private readonly IProductService _productService;
      //  private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductVariantService _productVariantService;

        public OrderController(
             IMapper mapper,
               IHttpContextAccessor httpContextAccessor,
               IOrderService orderService,
               ICartService cartService,
               IVoucherService voucherService,
               ICartItemService cartItemService,
               IOrderItemService orderItemService,
               IDataCategoryService dataCategoryService,
                UserManager<AppUser> userManager,
                IVnPayService vnPayService,
                IConfiguration config,
                IPaymentInfoService paymentInfoService,
                IProductService productService,
           //     IProductVariantRepository productVariantRepository,
                IProductVariantService productVariantService
               )
        {
            _mapper = mapper;
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
            _cartService = cartService;
            _voucherService = voucherService;
            _cartItemService = cartItemService;
            _orderItemService = orderItemService;
            _dataCategoryService = dataCategoryService;
            _userManager = userManager;
            _vnpayService = vnPayService;
            _config = config;
            _paymentInfoService = paymentInfoService;
            _productService = productService;
         //   _productVariantRepository = productVariantRepository;
            _productVariantService = productVariantService;
            
        }

        [HttpPost("get-data-by-page")]
        public async Task<IActionResult> GetDataByPage([FromBody] OrderSearchDTO searchDTO)
        {
            try
            {
                var sessionKey = $"searchOrder_{typeof(OrderDTO).Name}_{JsonConvert.SerializeObject(searchDTO)}";
                var jsonData = _httpContextAccessor.HttpContext.Session.GetString(sessionKey);

                if (!string.IsNullOrEmpty(jsonData)) // Kiểm tra jsonData có null hoặc rỗng không
                {
                    try
                    {
                        var usersFromSession = JsonConvert.DeserializeObject<PagedList<OrderDTO>>(jsonData);

                        // Kiểm tra tính hợp lệ của usersFromSession
                        if (usersFromSession != null && usersFromSession.Items != null && usersFromSession.Items.Any())
                        {
                            var resultSession = new ResponseWithDataDto<PagedList<OrderDTO>>()
                            {
                                Data = usersFromSession,
                            };
                            return Ok(resultSession);
                        }
                        else
                        {

                            _httpContextAccessor.HttpContext.Session.Remove(sessionKey); // Xóa session không hợp lệ
                        }
                    }
                    catch (JsonException ex)
                    {

                        _httpContextAccessor.HttpContext.Session.Remove(sessionKey);
                    }
                }

                var resultGetted = _orderService.GetDataByPage(searchDTO);
                foreach (var item in resultGetted.Data.Items)
                {

                    item.PaymentMethodName = _dataCategoryService.GetNameById((Guid)item.PaymentMethod);
                    item.StatusName = _dataCategoryService.GetNameById((Guid)item.Status);
                    var paymentInfo = await _paymentInfoService.FindByAsync(x => x.OrderId.Equals(item.Id) && x.IsDelete != true);
                    var paymentStatusId = paymentInfo.FirstOrDefault().PaymentStatusId;
                    item.PaymentStatusName = _dataCategoryService.GetNameById((Guid)paymentStatusId);

                }
                // Kiểm tra resultGetted.Data trước khi serialize
                if (resultGetted != null && resultGetted.Data != null && resultGetted.Data.Items != null)
                {
                    jsonData = JsonConvert.SerializeObject(resultGetted.Data);
                    HttpContext.Session.SetString(sessionKey, jsonData);
                }

                return Ok(resultGetted); // Trả về resultGetted dù có hay không có dữ liệu
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseWithMessageDto { Status = "Lỗi", Message = "Đã có lỗi xảy ra." });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateOrderViewModel model)
        {
            var serverResponse = new ResponseWithMessageDto() { Message = "Tạo đơn hàng mới thành công" };

            try
            {
                var modelDTO = _mapper.Map<OrderDTO>(model);
                var newCode = await _orderService.AutoGenOrderCode();
                modelDTO.Code = newCode;
                var entity = _mapper.Map<Order>(modelDTO);

                //Tạo được rồi thì đổi trạng thái giỏ hàng
                //dựa trên cartid
                var updateCart = await _cartService.GetByIdAsync(model.CartId);
                var namePaymentMethod = _dataCategoryService.GetCodeById(model.PaymentMethod);
                entity.Status = namePaymentMethod == "TIEN_MAT" ? _dataCategoryService.GetIdByCode("CHO_XAC_NHAN") : _dataCategoryService.GetIdByCode("CHO_THANH_TOAN");

                await _orderService.CreateAsync(entity);
                updateCart.IsOrder = true;
                await _cartService.UpdateAsync(updateCart);
                if (model.VoucherId != null)
                {
                    var updateVoucher = await _voucherService.GetByIdAsync(model.VoucherId);
                    updateVoucher.Quantity -= 1;
                    await _voucherService.UpdateAsync(updateVoucher);
                }
                //Chuyển cart item sang order item
                var listCartItem = await _cartItemService.GetAllCartItemByCartId(model.CartId);
                foreach (var item in listCartItem.Data)
                {
                    var newOrderItem = new OrderItem()
                    {
                        OrderId = entity.Id,
                        ProductVariantId = item.ProductVariantId,
                        Quantity = item.Quantity,
                        Price = (double)item.ProductPrice
                    };
                    await _orderItemService.CreateAsync(newOrderItem);
                    //Giảm số lượng sản phẩm khi đã đặt hóa đơn hàng
                    var productVariant = await _productVariantService.GetByIdAsync(item.ProductVariantId);
                    productVariant.StockQuantity -= item.Quantity;
                    await _productVariantService.UpdateAsync(productVariant);
                }
                var newPaymentInfo = new PaymentInfo()
                {
                    OrderId = entity.Id,
                    PaymentMethodId = model.PaymentMethod,
                    TotalAmount = modelDTO.TotalAmount,
                    PaymentStatusId = _dataCategoryService.GetIdByCodeandParentCode("CHO_THANH_TOAN", "PAYMENT_STATUS").Result

                };
                if (namePaymentMethod != "TIEN_MAT")//Nếu thanh toán bằng VNPay
                {
                    var vnpayModel = new VnPaymentRequestDTO
                    {
                        Amount = modelDTO.TotalAmount,
                        CreatedDate = DateTime.Now,
                        Description = $"{modelDTO.CustomerName} {modelDTO.CustomerPhoneNumber}",
                        FullName = modelDTO.CustomerName,
                        OrderCode = modelDTO.Code
                    };
                  
                    await _paymentInfoService.CreateAsync(newPaymentInfo);
                    serverResponse.Message = _vnpayService.CreatePaymentUrl(HttpContext, vnpayModel);
                }
                else //Nếu thanh toàn bằng tiền mặt
                {
                     await _paymentInfoService.CreateAsync(newPaymentInfo);
                }
                serverResponse.Status = newCode; //Trả về mã đơn hàng
            }
            catch (Exception ex)
            {
                serverResponse.Message = "Có lỗi xảy ra khi tạo đơn hàng";
                // Có thể tùy chỉnh phản hồi lỗi cụ thể hơn dựa trên loại exception (ex)
                return StatusCode(500, serverResponse); // Trả về mã lỗi 500 (Internal Server Error)
            }

            return Ok(serverResponse);
        }

        [HttpGet("get-link-pay-vnpay/{id}")]
        public async Task<IActionResult> GetLinkPayVNPay(Guid id)
        {
            var serverResponse = new ResponseWithMessageDto();
            var orderInfo = await _orderService.GetByIdAsync(id);
            var vnpayModel = new VnPaymentRequestDTO
            {
                Amount = orderInfo.TotalAmount,
                CreatedDate = DateTime.Now,
                Description = $"{orderInfo.CustomerName} {orderInfo.CustomerPhoneNumber}",
                FullName = orderInfo.CustomerName,
                OrderCode = orderInfo.Code
            };
            serverResponse.Message = _vnpayService.CreatePaymentUrl(HttpContext, vnpayModel);
            return Ok(serverResponse);
        }

        [HttpPost("buy-now")]
        public async Task<IActionResult> BuyNow([FromBody] BuyNowViewModel model)
        {
            var serverResponse = new ResponseWithMessageDto() { Message = "Tạo đơn hàng mới thành công" };

            try
            {
                ////Tạo giỏ hàng chứa 1 sản phẩm
                //var newCart = new Cart()
                //{
                //    IsOrder = true
                //};
                //if (model.UserId.HasValue)
                //{
                //    newCart.UserId = model.UserId.Value;
                //}
                //await _cartService.CreateAsync(newCart);
                ////Thêm mới cartItem
                //var newCartItem = new CartItem() { CartId = newCart.Id, Quantity = model.Quantity, ProductVariantId = model.ProductVariantId };
                //await _cartItemService.CreateAsync(newCartItem);

                var modelDTO = _mapper.Map<OrderDTO>(model);
                var newCode = await _orderService.AutoGenOrderCode();
                modelDTO.Code = newCode;
                var entity = _mapper.Map<Order>(modelDTO);
               // entity.CartId = newCart.Id;
                //Tạo được rồi thì đổi trạng thái giỏ hàng
                //dựa trên cartid
           
                var namePaymentMethod = _dataCategoryService.GetCodeById(model.PaymentMethod);
                entity.Status = namePaymentMethod == "TIEN_MAT" ? _dataCategoryService.GetIdByCode("CHO_XAC_NHAN") : _dataCategoryService.GetIdByCode("CHO_THANH_TOAN");

                await _orderService.CreateAsync(entity);
               
                if (model.VoucherId != null)
                {
                    var updateVoucher = await _voucherService.GetByIdAsync(model.VoucherId);
                    updateVoucher.Quantity -= 1;
                    await _voucherService.UpdateAsync(updateVoucher);
                }
                
                    var newOrderItem = new OrderItem()
                    {
                        OrderId = entity.Id,
                        ProductVariantId = model.ProductVariantId,
                        Quantity = model.Quantity,
                        Price = (double)model.Price
                    };
                    await _orderItemService.CreateAsync(newOrderItem);
                //Giảm số lượng sản phẩm khi đã đặt hóa đơn hàng
                var productVariant = await _productVariantService.GetByIdAsync(model.ProductVariantId);
                productVariant.StockQuantity -= model.Quantity;
                await _productVariantService.UpdateAsync(productVariant);
                var newPaymentInfo = new PaymentInfo()
                {
                    OrderId = entity.Id,
                    PaymentMethodId = model.PaymentMethod,
                    TotalAmount = modelDTO.TotalAmount,
                    PaymentStatusId = _dataCategoryService.GetIdByCodeandParentCode("CHO_THANH_TOAN", "PAYMENT_STATUS").Result

                };
                if (namePaymentMethod != "TIEN_MAT")//Nếu thanh toán bằng VNPay
                {
                    var vnpayModel = new VnPaymentRequestDTO
                    {
                        Amount = modelDTO.TotalAmount,
                        CreatedDate = DateTime.Now,
                        Description = $"{modelDTO.CustomerName} {modelDTO.CustomerPhoneNumber}",
                        FullName = modelDTO.CustomerName,
                        OrderCode = modelDTO.Code
                    };

                    await _paymentInfoService.CreateAsync(newPaymentInfo);
                    serverResponse.Message = _vnpayService.CreatePaymentUrl(HttpContext, vnpayModel);
                }
                else
                {
                    await _paymentInfoService.CreateAsync(newPaymentInfo);
                }
                serverResponse.Status = newCode; //Trả về mã đơn hàng
            }
            catch (Exception ex)
            {
                serverResponse.Message = "Có lỗi xảy ra khi tạo đơn hàng";
                // Có thể tùy chỉnh phản hồi lỗi cụ thể hơn dựa trên loại exception (ex)
                return StatusCode(500, serverResponse); // Trả về mã lỗi 500 (Internal Server Error)
            }

            return Ok(serverResponse);
        }


        [HttpDelete("soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var serverResponse = new ResponseWithMessageDto() { Message = "Xóa thành công" };

            try
            {
                var data = await _orderService.GetByIdAsync(id);
                if (data == null)
                {
                    serverResponse.Message = "Không tìm thấy dữ liệu";
                    serverResponse.Status = "Fail";
                    return Ok(serverResponse);
                }
                await _orderService.SoftDeleteAsync(data);
                return Ok(serverResponse);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                            new ResponseWithMessageDto { Status = "Error", Message = ex.Message });
            }
        }

        [HttpGet("get-order-info-by-id/{id}")]
        public async Task<IActionResult> GetOrderInfoById(Guid id) {
            var response = new ResponseWithDataDto<OrderDTO>();
            var baseInfo = await _orderService.GetByIdAsync(id);
            var baseInfoDTO = _mapper.Map<OrderDTO>(baseInfo);
            if(baseInfo.UserId != null)//Nếu là người dùng đã đăng nhập mua thì sẽ xuất thông tin
            {
                var userInfo = await _userManager.FindByIdAsync(baseInfo.UserId.ToString());
                baseInfoDTO.UserName = userInfo.FullName;
                baseInfoDTO.UserPhoneNumber = userInfo.PhoneNumber;
                baseInfoDTO.UserEmail = userInfo.Email;
                baseInfoDTO.UserAddress = userInfo.Address;
            }
            if(baseInfo.VoucherId != null)
            {
                var voucher = await _voucherService.GetByIdAsync(baseInfo.VoucherId);
                baseInfoDTO.VoucherCode = voucher.Code;
            }
            baseInfoDTO.PaymentMethodName = _dataCategoryService.GetNameById((Guid)baseInfoDTO.PaymentMethod);
            baseInfoDTO.StatusName = _dataCategoryService.GetNameById((Guid)baseInfoDTO.Status);
            var paymentInfo = await _paymentInfoService.FindByAsync(x => x.OrderId.Equals(id) && x.IsDelete != true);
            var paymentStatusId = paymentInfo.FirstOrDefault().PaymentStatusId;
            baseInfoDTO.PaymentStatusName = _dataCategoryService.GetNameById((Guid)paymentStatusId);    
            response.Data = baseInfoDTO;
            return Ok(response);
        }


        [HttpGet("get-order-info-by-user-email/{email}")]
        public async Task<IActionResult> GetOrderInfoByUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var id = Guid.Parse(user.Id); //user.Id
            var response = new ResponseWithDataDto<List<OrderDTO>>();
            var baseInfo = await _orderService.FindByAsync(x => x.UserId.Equals(id) && x.IsDelete != true);
            baseInfo = baseInfo.OrderByDescending(x => x.CreatedDate).ToList();
            
            response.Data = _mapper.Map<List<OrderDTO>>(baseInfo);
            foreach(var item in response.Data)
            {
                item.PaymentMethodName = _dataCategoryService.GetNameById((Guid)item.PaymentMethod);
                item.StatusName = _dataCategoryService.GetNameById((Guid)item.Status);
                var paymentInfo = await _paymentInfoService.FindByAsync(x => x.OrderId.Equals(item.Id) && x.IsDelete != true);
                var paymentStatusId = paymentInfo.FirstOrDefault().PaymentStatusId;
                item.PaymentStatusName = _dataCategoryService.GetNameById((Guid)paymentStatusId);
                if (item.VoucherId != null)
                {
                    var voucher = await _voucherService.GetByIdAsync(item.VoucherId);
                    item.VoucherCode = voucher.Code;
                }
            }
            return Ok(response);
        }

        [HttpPost("get-order-info-by-list-order-code")]
        public async Task<IActionResult> GetOrderInfoByListOrderCode([FromBody] List<string> orderCodes)
        {
           
            var response = new ResponseWithDataDto<List<OrderDTO>>();
            response.Data = new List<OrderDTO>();
            foreach(var item in orderCodes)
            {
                var baseInfoItem = await _orderService.FindByAsync(x => x.Code.Equals(item) && x.IsDelete != true);
                if(baseInfoItem.FirstOrDefault() != null)
                {
                    var baseInfoDTO = _mapper.Map<OrderDTO>(baseInfoItem.FirstOrDefault());
                    response.Data.Add(baseInfoDTO);
                }

            }

            foreach (var item in response.Data)
            {
                item.PaymentMethodName = _dataCategoryService.GetNameById((Guid)item.PaymentMethod);
                item.StatusName = _dataCategoryService.GetNameById((Guid)item.Status);
                var paymentInfo = await _paymentInfoService.FindByAsync(x => x.OrderId.Equals(item.Id) && x.IsDelete != true);
                var paymentStatusId = paymentInfo.FirstOrDefault().PaymentStatusId;
                item.PaymentStatusName = _dataCategoryService.GetNameById((Guid)paymentStatusId);
                if (item.VoucherId != null)
                {
                    var voucher = await _voucherService.GetByIdAsync(item.VoucherId);
                    item.VoucherCode = voucher.Code;
                }
            }
            return Ok(response);
        }


        [HttpGet("get-order-info-by-code/{orderCode}")]
        public async Task<IActionResult> GetOrderInfoByCode(string orderCode)
        {
            var response = new ResponseWithDataDto<OrderDTO>();
            var baseInfo1 = await _orderService.FindByAsync(x=>x.Code.Equals(orderCode) && x.IsDelete != true);
            var baseInfo = baseInfo1.FirstOrDefault();
            var baseInfoDTO = _mapper.Map<OrderDTO>(baseInfo);
            if (baseInfo.UserId != null)//Nếu là người dùng đã đăng nhập mua thì sẽ xuất thông tin
            {
                var userInfo = await _userManager.FindByIdAsync(baseInfo.UserId.ToString());
                baseInfoDTO.UserName = userInfo.FullName;
                baseInfoDTO.UserPhoneNumber = userInfo.PhoneNumber;
                baseInfoDTO.UserEmail = userInfo.Email;
                baseInfoDTO.UserAddress = userInfo.Address;
            }
            baseInfoDTO.PaymentMethodName = _dataCategoryService.GetNameById((Guid)baseInfoDTO.PaymentMethod);
            baseInfoDTO.StatusName = _dataCategoryService.GetNameById((Guid)baseInfoDTO.Status);
            var paymentInfo = await _paymentInfoService.FindByAsync(x => x.OrderId.Equals(baseInfo.Id) && x.IsDelete != true);
            var paymentStatusId = paymentInfo.FirstOrDefault().PaymentStatusId;
            baseInfoDTO.PaymentStatusName = _dataCategoryService.GetNameById((Guid)paymentStatusId);
            response.Data = baseInfoDTO;
            return Ok(response);
        }

        [HttpGet("get-order-item-by-id/{id}")]
        public async Task<IActionResult> GetOrderItemsById(Guid id)
        {
            var response = await _orderService.GetOrderItemsById(id);
            return Ok(response);
        }
        [HttpGet("update-order-status/{orderId}/{statusId}")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, Guid statusId)
        {
            var response = new ResponseWithMessageDto() { Message = "Cập nhật thành công" };
            try
            {
                var orderUpdate = await _orderService.GetByIdAsync(orderId);
                orderUpdate.Status = statusId;
            
               //Nếu trạng thái thành công thì cập nhật thông tin thanh toán
               var statusSuccessIdCOD = await _dataCategoryService.GetIdByCodeandParentCode("GIAO_HANG_THANHCONG", "STATUS_COD");
                if (orderUpdate.Status == statusSuccessIdCOD)
                {
                    var paymentInfo = await _paymentInfoService.FindByAsync(x => x.OrderId.Equals(orderId) && x.IsDelete != true);
                    var paymentInfoUpdate = paymentInfo.FirstOrDefault();
                    var paymentStatusId = await _dataCategoryService.GetIdByCodeandParentCode("DA_THANH_TOAN", "PAYMENT_STATUS");
                    paymentInfoUpdate.PaymentStatusId = paymentStatusId;
                    await _paymentInfoService.UpdateAsync(paymentInfoUpdate);
                }
                await _orderService.UpdateAsync(orderUpdate);
                return Ok(response);
            }
            catch(Exception e)
            {
                response.Message = e.Message;
                return Ok(response);
            }
       
            
        }


        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelOrder(Guid id, [FromBody] CancelOrderReason model)
        {
            var response = new ResponseWithMessageDto() { Message = "Hủy thành công" };
            try
            {
                var order = await _orderService.GetByIdAsync(id);
                order.IsCancelled = true;
                order.ReasonCancelled = model.ReasonCancel;
                await _orderService.UpdateAsync(order);
                //Cập nhật trạng thái thanh toán
                var cancelStatusId = await _dataCategoryService.GetIdByCodeandParentCode("DA_HUY", "PAYMENT_STATUS");
                var paymentInfo = await _paymentInfoService.FindByAsync(x => x.OrderId.Equals(id) && x.IsDelete != true);
                var paymentInfoUpdate = paymentInfo.FirstOrDefault();
                paymentInfoUpdate.PaymentStatusId = cancelStatusId;

                //Cập nhật mã giảm giá nếu hủy đơn
                if(order.VoucherId != null)
                {
                    var voucher = await _voucherService.GetByIdAsync(order.VoucherId);
                    voucher.Quantity += 1;
                    await _voucherService.UpdateAsync(voucher);
                }
                //Tăng số lượng sản phẩm lên
                var orderItems = await _orderItemService.FindByAsync(x => x.OrderId.Equals(id) && x.IsDelete != true);
                foreach (var item in orderItems)
                {
                   await _orderService.UpdateCancelProductVariant(item.ProductVariantId, item.Quantity);
                }

                await _paymentInfoService.UpdateAsync(paymentInfoUpdate);
                return Ok(response);
            }
            catch(Exception e)
            {
                response.Message = e.Message;
                return Ok(response);
            }
        }
        [HttpGet("payment-callback")]
        public async Task<IActionResult> PaymentCallBack()
        {
            string returnUrl = _config["VnPay:FrontendReturnURL"];
            var response = _vnpayService.PaymentExecute(Request.Query);
            var orderCode = response.OrderDescription; //Mã đơn hàng để ở description
            var order = await _orderService.FindByAsync(x => x.Code.Equals(orderCode) && x.IsDelete != true);
            var orderId = order.FirstOrDefault().Id;
            if(response == null || response.VnPayResponseCode != "00")
            {
                returnUrl += $"?success=false&ordercode={orderCode}";
            }
            else
            {
                returnUrl += $"?success=true&ordercode={orderCode}";
                //Cập nhật trạng thái thanh toán của VNPAY
               var payinfo =  await _paymentInfoService.FindByAsync(x => x.OrderId.Equals(orderId) && x.IsDelete !=true);
               var updateInfo = payinfo.FirstOrDefault();
                updateInfo.VnpResponseCode = response.VnPayResponseCode;
                updateInfo.TransactionId = response.TransactionId;
                var paymentStatusId = await _dataCategoryService.GetIdByCodeandParentCode("DA_THANH_TOAN", "PAYMENT_STATUS");
                updateInfo.PaymentStatusId = paymentStatusId;
                updateInfo.VnpPayDate = DateTime.Now;
                await _paymentInfoService.UpdateAsync(updateInfo);
                //Cập nhật trạng thái đơn hàng

                var newStatus = await _dataCategoryService.GetIdByCodeandParentCode("CHUAN_BI_HANG", "STATUS_VNPAY");
                var orderUpdate = await _orderService.GetByIdAsync(orderId);
                orderUpdate.Status = newStatus;
                await _orderService.UpdateAsync(orderUpdate);


            }
            return Redirect(returnUrl);
        }


        [HttpGet("get-all-order-today")]
        public async Task<IActionResult> GetAllOrderToday()
        {
            
            var searchDTO = new OrderSearchDTO()
            {
                CreatedTime_Filter = DateTime.Today
            };
           var response = _orderService.GetDataByPage(searchDTO);
            return Ok(response);
        }
        [HttpGet("count-order-today")]
        public async Task<IActionResult> CountOrderToday()
        {

            var response = new ResponseWithDataDto<int>();
            response.Data = _orderService.CountNumberOfOrderToday();
           
            return Ok(response);
        }
        [HttpGet("count-order-yesterday")]
        public async Task<IActionResult> CountOrderYesterday()
        {

            var response = new ResponseWithDataDto<int>();
            response.Data = _orderService.CountNumberOfOrderYesterday();

            return Ok(response);
        }
        [HttpGet("count-order-waiting-confirm")]
        public async Task<IActionResult> CountOrderWaitingConfirm()
        {

            var response = new ResponseWithDataDto<int>();
            var statusId = await _dataCategoryService.GetIdByCodeandParentCode("CHO_XAC_NHAN", "STATUS_COD");
            response.Data = _orderService.CountNumberOfOrderWaitingConfirm(statusId);

            return Ok(response);
        }

        [HttpGet("get-revenue-today")]
        public async Task<IActionResult> GetRevenueToday()
        {
           
            var response = new ResponseWithDataDto<double>();
            response.Data = _orderService.GetRevenueToday();
            return Ok(response);
        }

        [HttpGet("get-revenue-of-month")]
        public async Task<IActionResult> GetRevenueOfMonth()
        {
            var response = new ResponseWithDataDto<List<RevenueDTO>>();
            response.Data = _orderService.GetRevenueOfMonth();
            return Ok(response);
        }
        [HttpGet("get-revenue-day-to-day/{startDay}/{endDay}")]
        public async Task<IActionResult> GetRevenueDayToDay(DateTime startDay, DateTime endDay)
        {
            var response = new ResponseWithDataDto<List<RevenueDTO>>();
            response.Data = _orderService.GetRevenueDayToDay(startDay,endDay);
            return Ok(response);
        }

        [HttpGet("get-revenue-of-category")]
        public async Task<IActionResult> GetRevenueOfCategory()
        {
            var response = new ResponseWithDataDto<List<RevenueCategoryDTO>>();
            var listCategoryId = new List<Guid>();
           var itemNam =await _dataCategoryService.GetIdByCodeandParentCode("DO_NAM", "SAN_PHAM");
            var itemNu = await _dataCategoryService.GetIdByCodeandParentCode("DO_NU", "SAN_PHAM");
           
            listCategoryId.Add(itemNam);
            listCategoryId.Add(itemNu);

            response.Data = _orderService.GetRevenueCategories(listCategoryId);
            return Ok(response);
        }
        [HttpGet("get-revenue-of-category-day-to-day/{startDay}/{endDay}")]
        public async Task<IActionResult> GetRevenueOfCategoryDayToDay(DateTime startDay, DateTime endDay)
        {
            var response = new ResponseWithDataDto<List<RevenueCategoryDTO>>();
            var listCategoryId = new List<Guid>();
            var itemNam = await _dataCategoryService.GetIdByCodeandParentCode("DO_NAM", "SAN_PHAM");
            var itemNu = await _dataCategoryService.GetIdByCodeandParentCode("DO_NU", "SAN_PHAM");

            listCategoryId.Add(itemNam);
            listCategoryId.Add(itemNu);

            response.Data = _orderService.GetRevenueCategoriesDayToDay(listCategoryId, startDay,endDay);
            return Ok(response);
        }

        [HttpGet("get-top-product-selling")] //Lấy sản phẩm bán chạy trong tuần
        public async Task<IActionResult> GetTopProductSelling()
        {
            var response = new ResponseWithDataDto<List<ProductTopSellingDTO>>();
            response.Data =  _orderService.GetProductTopSellingOfWeek();
            //Lấy ảnh từ productId
            foreach(var item in response.Data)
            {
                var product = await _productService.GetProductById(item.ProductId);
              var productItem =  product.Data.Items.FirstOrDefault();
                item.ThumbnailPath = productItem.Thumbnail;
                item.ThumbnailBase64 = productItem.ThumbnailBase64;
                item.ThumbnailContentType = productItem.ThumbnailContentType;
            }

            return Ok(response);
        }

        [HttpGet("get-top-product-selling-day-to-day/{startDay}/{endDay}")] //Lấy sản phẩm bán chạy trong tuần
        public async Task<IActionResult> GetTopProductSellingDayToDay(DateTime startDay, DateTime endDay)
        {
            var response = new ResponseWithDataDto<List<ProductTopSellingDTO>>();
            response.Data = _orderService.GetProductTopSellingDayToDay(startDay,endDay);
            //Lấy ảnh từ productId
            foreach (var item in response.Data)
            {
                var product = await _productService.GetProductById(item.ProductId);
                var productItem = product.Data.Items.FirstOrDefault();
                item.ThumbnailPath = productItem.Thumbnail;
                item.ThumbnailBase64 = productItem.ThumbnailBase64;
                item.ThumbnailContentType = productItem.ThumbnailContentType;
            }

            return Ok(response);
        }





    }
}
