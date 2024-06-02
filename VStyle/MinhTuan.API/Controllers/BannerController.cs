using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MinhTuan.API.Helper;
using MinhTuan.API.ViewModels.BannerViewModel;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.BannerDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Service.SearchDTO;
using MinhTuan.Service.Services.BannerService;
using Newtonsoft.Json;

namespace MinhTuan.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BannerController : Controller
{
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBannerService _bannerService;

    public BannerController(IMapper mapper, IHttpContextAccessor httpContextAccessor, IBannerService bannerService)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _bannerService = bannerService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromForm] BannerViewModel model)
    {
        var serverResponse = new ResponseWithMessageDto() { Message = "Tạo banner mới thành công" };

        try
        {
            var modelDTO = _mapper.Map<BannerDTO>(model);
            var bannerFileName = new UploadHandler().UploadBannerImage(model.BannerImage);//validate file ảnh
            if (bannerFileName.Contains("Kích") || bannerFileName.Contains("Định"))
            {
                serverResponse.Message = bannerFileName;//Trả về tb lỗi ảnh
                return Ok(serverResponse);
            }
            modelDTO.Path = bannerFileName;
            await _bannerService.CreateAsync(_mapper.Map<Banner>(modelDTO));

        }
        catch (Exception ex)
        {
            serverResponse.Message = "Có lỗi xảy ra khi tạo banner";
            // Có thể tùy chỉnh phản hồi lỗi cụ thể hơn dựa trên loại exception (ex)
            return StatusCode(500, serverResponse); // Trả về mã lỗi 500 (Internal Server Error)
        }

        return Ok(serverResponse);
    }

    [HttpPost("get-data-by-page")]
    public async Task<IActionResult> GetDataByPage([FromBody] BannerSearchDTO searchDTO)
    {
        try
        {
            var sessionKey = $"searchBanner_{typeof(BannerDTO).Name}_{JsonConvert.SerializeObject(searchDTO)}";
            var jsonData = _httpContextAccessor.HttpContext.Session.GetString(sessionKey);

            if (!string.IsNullOrEmpty(jsonData)) // Kiểm tra jsonData có null hoặc rỗng không
            {
                try
                {
                    var usersFromSession = JsonConvert.DeserializeObject<PagedList<BannerDTO>>(jsonData);

                    // Kiểm tra tính hợp lệ của usersFromSession
                    if (usersFromSession != null && usersFromSession.Items != null && usersFromSession.Items.Any())
                    {
                        var resultSession = new ResponseWithDataDto<PagedList<BannerDTO>>()
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

            var resultGetted = _bannerService.GetDataByPage(searchDTO);

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

    [HttpDelete("soft-delete/{id}")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var serverResponse = new ResponseWithMessageDto() { Message = "Xóa thành công" };

        try
        {
            var data = await _bannerService.GetByIdAsync(id);
            if (data == null)
            {
                serverResponse.Message = "Không tìm thấy dữ liệu";
                serverResponse.Status = "Fail";
                return Ok(serverResponse);
            }
            await _bannerService.SoftDeleteAsync(data);
            return Ok(serverResponse);

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                        new ResponseWithMessageDto { Status = "Error", Message = ex.Message });
        }
    }
}


