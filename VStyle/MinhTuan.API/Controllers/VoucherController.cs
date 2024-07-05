using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhTuan.API.ViewModels.CategoryViewModel;
using MinhTuan.API.ViewModels.VoucherViewModel;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.DTOs.VoucherDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Service.SearchDTO;
using MinhTuan.Service.Services.CategoryService;
using MinhTuan.Service.Services.OrderService;
using MinhTuan.Service.Services.VoucherService;
using Newtonsoft.Json;

namespace MinhTuan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoucherController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IVoucherService _voucherService;
        private readonly IOrderService _orderService;

        public VoucherController(
              IMapper mapper,
               IHttpContextAccessor httpContextAccessor,
               IVoucherService voucherService,
               IOrderService orderService
            )
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _voucherService = voucherService;
            _orderService = orderService;
        }

        [HttpPost("get-data-by-page")]
        public async Task<IActionResult> GetDataByPage([FromBody] VoucherSearchDTO searchDTO)
        {
            try
            {
                var sessionKey = $"searchVoucher_{typeof(VoucherDTO).Name}_{JsonConvert.SerializeObject(searchDTO)}";
                var jsonData = _httpContextAccessor.HttpContext.Session.GetString(sessionKey);

                if (!string.IsNullOrEmpty(jsonData)) // Kiểm tra jsonData có null hoặc rỗng không
                {
                    try
                    {
                        var usersFromSession = JsonConvert.DeserializeObject<PagedList<VoucherDTO>>(jsonData);

                        // Kiểm tra tính hợp lệ của usersFromSession
                        if (usersFromSession != null && usersFromSession.Items != null && usersFromSession.Items.Any())
                        {
                            var resultSession = new ResponseWithDataDto<PagedList<VoucherDTO>>()
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

                var resultGetted = _voucherService.GetDataByPage(searchDTO);

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
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateVoucherViewModel model)
        {
            var serverResponse = new ResponseWithMessageDto() { Message = "Tạo voucher mới thành công" };

            try
            {
                var modelDTO = _mapper.Map<VoucherDTO>(model);

                var isExistCode = _voucherService.CheckExitCode(modelDTO.Code, Guid.Empty);
                if (isExistCode)
                {
                    serverResponse.Message = "Trùng mã Voucher";
                    return Ok(serverResponse);
                }
                if (model.TimeEnd == null) modelDTO.TimeEnd = DateTime.MaxValue;
                if (model.TimeStart == null) modelDTO.TimeStart = DateTime.Now;


                await _voucherService.CreateAsync(_mapper.Map<Voucher>(modelDTO));
            }
            catch (Exception ex)
            {
                serverResponse.Message = "Có lỗi xảy ra khi tạo Voucher";
                // Có thể tùy chỉnh phản hồi lỗi cụ thể hơn dựa trên loại exception (ex)
                return StatusCode(500, serverResponse); // Trả về mã lỗi 500 (Internal Server Error)
            }

            return Ok(serverResponse);
        }
        [HttpDelete("soft-delete/{id}")]
        [Authorize]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var serverResponse = new ResponseWithMessageDto() { Message = "Xóa thành công" };

            try
            {
                var data = await _voucherService.GetByIdAsync(id);
                if (data == null)
                {
                    serverResponse.Message = "Không tìm thấy dữ liệu";
                    serverResponse.Status = "Fail";
                    return Ok(serverResponse);
                }
                if(_orderService.FindByAsync(x=>x.VoucherId.Equals(id)).Result.Count() > 0)
                {
                    serverResponse.Message = "Không thể xóa dữ liệu này";
                    serverResponse.Status = "Fail";
                    return Ok(serverResponse);
                }
                await _voucherService.SoftDeleteAsync(data);
                return Ok(serverResponse);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                            new ResponseWithMessageDto { Status = "Error", Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var serverResponse = new ResponseWithDataDto<VoucherDTO>() { Message = "Lấy thông tin thành công" };
            try
            {
                var result = await _voucherService.GetByIdAsync(id);
                if (result != null)
                {
                    serverResponse.Data = _mapper.Map<VoucherDTO>(result);
                }
                return Ok(serverResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseWithMessageDto
                {
                    Status = "Fail",
                    Message = ex.Message
                }); ; ;
            }
        }
        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateVoucherViewModel model)
        {

            var serverResponse = new ResponseWithMessageDto() { Message = "Cập nhật thành công" };

            try
            {
                var modelDTO = _mapper.Map<VoucherDTO>(model);

                var isExistCode = _voucherService.CheckExitCode(modelDTO.Code, id);
                if (isExistCode)
                {
                    serverResponse.Message = "Trùng mã voucher";
                    return Ok(serverResponse);
                }

               
                var EntityNeedUpdate = await _voucherService.GetByIdAsync(id);
                EntityNeedUpdate.Code = modelDTO.Code;
                EntityNeedUpdate.MinimumPurchaseAmount = modelDTO.MinimumPurchaseAmount;
                EntityNeedUpdate.DiscountPercent = modelDTO.DiscountPercent;
                EntityNeedUpdate.Quantity = modelDTO.Quantity;
                EntityNeedUpdate.TimeStart = modelDTO.TimeStart;
                EntityNeedUpdate.TimeEnd = modelDTO.TimeEnd;
                EntityNeedUpdate.Type = modelDTO.Type;
                EntityNeedUpdate.MaxValue = modelDTO.MaxValue;
                EntityNeedUpdate.DiscountAmount = modelDTO.DiscountAmount;
                await _voucherService.UpdateAsync(EntityNeedUpdate);
            }
            catch (Exception ex)
            {
                serverResponse.Message = "Có lỗi xảy ra khi cập nhật";
                // Có thể tùy chỉnh phản hồi lỗi cụ thể hơn dựa trên loại exception (ex)
                return StatusCode(500, serverResponse); // Trả về mã lỗi 500 (Internal Server Error)
            }

            return Ok(serverResponse);
        }


        [HttpGet("get-voucher-by-code/{code}")]
        public async Task<IActionResult> GetByIdAsync(string code)
        {
            var serverResponse = new ResponseWithDataDto<VoucherDTO>() { Message = "Lấy thông tin thành công" };
            try
            {
                var result = await _voucherService.FindByAsync(x=>x.Code.ToLower().Equals(code.ToLower()) && x.IsDelete != true && x.Quantity >0 && x.TimeEnd > DateTime.Now);
                if (result.Count() > 0)
                {
                    serverResponse.Data = _mapper.Map<VoucherDTO>(result.FirstOrDefault());
                }
                else
                {
                    serverResponse.Message = "Mã không hợp lệ";
                }
                
                return Ok(serverResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseWithMessageDto
                {
                    Status = "Fail",
                    Message = ex.Message
                }); ; ;
            }
        }
    }
}
