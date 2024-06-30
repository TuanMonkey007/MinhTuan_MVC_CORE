using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MinhTuan.API.ViewModels.CategoryViewModel;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.AccountDTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Service.Core.Services;

using MinhTuan.Service.SearchDTO;
using MinhTuan.Service.Services.AccountService;
using MinhTuan.Service.Services.CategoryService;
using MinhTuan.Service.Services.DataCategoryService;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MinhTuan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICategoryService _categoryService;
        private readonly IDataCategoryService _dataCategoryService ;
        public CategoryController(
             IMapper mapper,
               IHttpContextAccessor httpContextAccessor,
               ICategoryService categoryService,
               IDataCategoryService dataCategoryService 
             )
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _categoryService = categoryService;
            _dataCategoryService = dataCategoryService;

        }
        [HttpPost("get-data-by-page")]
        public async Task<IActionResult> GetDataByPage([FromBody] CategorySearchDTO searchDTO)
        {
            try
            {
                var sessionKey = $"searchCategory_{typeof(CategoryDTO).Name}_{JsonConvert.SerializeObject(searchDTO)}";
                var jsonData = _httpContextAccessor.HttpContext.Session.GetString(sessionKey);

                if (!string.IsNullOrEmpty(jsonData)) // Kiểm tra jsonData có null hoặc rỗng không
                {
                    try
                    {
                        var usersFromSession = JsonConvert.DeserializeObject<PagedList<CategoryDTO>>(jsonData);

                        // Kiểm tra tính hợp lệ của usersFromSession
                        if (usersFromSession != null && usersFromSession.Items != null && usersFromSession.Items.Any())
                        {
                            var resultSession = new ResponseWithDataDto<PagedList<CategoryDTO>>()
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

                var resultGetted = _categoryService.GetDataByPage(searchDTO);

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
        public async Task<IActionResult> Create([FromBody] CreateCategoryViewModel model)
        {
            var serverResponse = new ResponseWithMessageDto() {Message = "Tạo danh mục mới thành công" };

            try
            {
                var modelDTO = _mapper.Map<CategoryDTO>(model);

                var isExistCode = _categoryService.CheckExitCode(modelDTO.Code,Guid.Empty);
                if (isExistCode)
                {
                    serverResponse.Message = "Trùng mã danh mục";
                    return Ok(serverResponse);
                }

                var isExistName = _categoryService.CheckExitName(modelDTO.Name,Guid.Empty);
                if (isExistName)
                {
                    serverResponse.Message = "Trùng tên danh mục";
                    return Ok(serverResponse);
                }

                await _categoryService.CreateAsync(_mapper.Map<Category>(modelDTO));
            }
            catch (Exception ex)
            {
                serverResponse.Message = "Có lỗi xảy ra khi tạo danh mục";
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
                var data = await _categoryService.GetByIdAsync(id);
                if (data == null)
                {
                    serverResponse.Message = "Không tìm thấy dữ liệu";
                    serverResponse.Status = "Fail";
                    return Ok(serverResponse);
                }
                if(_dataCategoryService.FindByAsync(x => x.ParentId.ToString() == id.ToString() &&x.IsDelete != true ).Result.Count() > 0)
                {
                    serverResponse.Message = "Không thể xóa dữ liệu này";
                    serverResponse.Status = "Fail";
                    return Ok(serverResponse);
                }
                await _categoryService.SoftDeleteAsync(data);
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
            var serverResponse = new ResponseWithDataDto<CategoryDTO>() { Message = "Lấy thông tin thành công"};
            try
            {
                var result = await _categoryService.GetByIdAsync(id);
                if (result != null)
                {
                    serverResponse.Data = _mapper.Map<CategoryDTO>(result);
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
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateCategoryViewModel model)
        {

            var serverResponse = new ResponseWithMessageDto() { Message = "Cập nhật thành công" };

            try
            {
                var modelDTO = _mapper.Map<CategoryDTO>(model);

                var isExistCode = _categoryService.CheckExitCode(modelDTO.Code,id);
                if (isExistCode)
                {
                    serverResponse.Message = "Trùng mã danh mục";
                    return Ok(serverResponse);
                }

                var isExistName = _categoryService.CheckExitName(modelDTO.Name,id);
                if (isExistName)
                {
                    serverResponse.Message = "Trùng tên danh mục";
                    return Ok(serverResponse);
                }
                var EntityNeedUpdate = await _categoryService.GetByIdAsync(id);
                EntityNeedUpdate.Code = modelDTO.Code;
                EntityNeedUpdate.Name = modelDTO.Name;
                EntityNeedUpdate.Description = modelDTO.Description;
                await _categoryService.UpdateAsync(EntityNeedUpdate);
            }
            catch (Exception ex)
            {
                serverResponse.Message = "Có lỗi xảy ra khi cập nhật";
                // Có thể tùy chỉnh phản hồi lỗi cụ thể hơn dựa trên loại exception (ex)
                return StatusCode(500, serverResponse); // Trả về mã lỗi 500 (Internal Server Error)
            }

            return Ok(serverResponse);
        }
    }
}