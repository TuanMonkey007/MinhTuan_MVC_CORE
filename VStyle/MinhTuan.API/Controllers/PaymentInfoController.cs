using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.DTOs.PaymentInfoDTO;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Service.SearchDTO;
using MinhTuan.Service.Services.PaymentInfoService;
using Newtonsoft.Json;

namespace MinhTuan.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentInfoController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPaymentInfoService _paymentInfoService;

        public PaymentInfoController(
                   IMapper mapper,
               IHttpContextAccessor httpContextAccessor,
               IPaymentInfoService paymentInfoService
               )
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _paymentInfoService = paymentInfoService;
        }

        [HttpPost("get-data-by-page")]
        public async Task<IActionResult> GetDataByPage([FromBody] PaymentInfoSearchDTO searchDTO)
        {
            try
            {
                var sessionKey = $"searchPaymentInfo_{typeof(PaymentInfoDTO).Name}_{JsonConvert.SerializeObject(searchDTO)}";
                var jsonData = _httpContextAccessor.HttpContext.Session.GetString(sessionKey);

                if (!string.IsNullOrEmpty(jsonData)) // Kiểm tra jsonData có null hoặc rỗng không
                {
                    try
                    {
                        var usersFromSession = JsonConvert.DeserializeObject<PagedList<PaymentInfoDTO>>(jsonData);

                        // Kiểm tra tính hợp lệ của usersFromSession
                        if (usersFromSession != null && usersFromSession.Items != null && usersFromSession.Items.Any())
                        {
                            var resultSession = new ResponseWithDataDto<PagedList<PaymentInfoDTO>>()
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

                var resultGetted = await _paymentInfoService.GetDataByPage(searchDTO);
             
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

    }
}
