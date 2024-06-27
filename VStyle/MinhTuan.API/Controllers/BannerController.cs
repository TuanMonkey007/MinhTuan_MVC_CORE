using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinhTuan.API.Helper;
using MinhTuan.API.ViewModels.BannerViewModel;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.AccountDTO;
using MinhTuan.Domain.DTOs.BannerDTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
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
            if(model.BannerImage != null)
            {
                var bannerFileName = new UploadHandler().UploadBannerImage(model.BannerImage);//validate file ảnh
                if (bannerFileName.Contains("Kích") || bannerFileName.Contains("Định"))
                {
                    serverResponse.Message = bannerFileName;//Trả về tb lỗi ảnh
                    return Ok(serverResponse);
                }
                modelDTO.Path = bannerFileName;
            }
            var modelMap = _mapper.Map<Banner>(modelDTO);
            await _bannerService.CreateAsync(modelMap);

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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var serverResponse = new ResponseWithDataDto<BannerDTO>() { Message = "Lấy thông tin thành công" };
        try
        {
            var result = await _bannerService.GetByIdAsync(id);
            if (result != null)
            {
                var modelMap = _mapper.Map<BannerDTO>(result);
                if (!result.Path.IsNullOrEmpty())
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Banner", result.Path);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(path);
                    modelMap.BannerBase64 =  Convert.ToBase64String(imageBytes);
                    var fileExtension = Path.GetExtension(Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Banner", result.Path)).ToLowerInvariant();
                    modelMap.BannerContentType = fileExtension switch
                    {
                        ".jpg" or ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        _ => "application/octet-stream",
                    };
                }
                serverResponse.Data = modelMap; ;
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
    public async Task<IActionResult> Update(Guid id , [FromForm] BannerViewModel model)
    {
        var serverResponse = new ResponseWithMessageDto() { Message = "Cập nhật thành công" };

        try
        {
            var modelDTO = _mapper.Map<BannerDTO>(model);
           
            var bannerNeedUpdate = await _bannerService.GetByIdAsync(id);
            if(bannerNeedUpdate != null)
            {
                bannerNeedUpdate.IsDisplay = modelDTO.IsDisplay;
                bannerNeedUpdate.OrderDisplay = modelDTO.OrderDisplay;
       
                bannerNeedUpdate.CategoryId = modelDTO.CategoryId;
           
            }
            string bannerFileName = string.Empty;
            if (model.BannerImage != null)
            {
                bannerFileName = new UploadHandler().UploadBannerImage(model.BannerImage);//validate file ảnh
                if (bannerFileName.Contains("Kích") || bannerFileName.Contains("Định"))
                {
                    serverResponse.Message = bannerFileName;
                    return Ok(serverResponse);
                }
                bannerNeedUpdate.Path = bannerFileName;
            }
           
            var  result =  _bannerService.UpdateAsync(bannerNeedUpdate);
            await result;
            if (result.IsCompleted)
            {
                return Ok(serverResponse);
            }
            serverResponse.Message = "Lỗi cập nhật";
            return Ok(serverResponse);
        }
        catch (Exception ex)
        {
            serverResponse.Message = "Có lỗi xảy ra khi cập nhật";
            // Có thể tùy chỉnh phản hồi lỗi cụ thể hơn dựa trên loại exception (ex)
            return StatusCode(500, serverResponse); // Trả về mã lỗi 500 (Internal Server Error)
        }

    }

}


