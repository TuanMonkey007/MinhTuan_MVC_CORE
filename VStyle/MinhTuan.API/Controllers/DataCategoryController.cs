﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhTuan.API.ViewModels.CategoryViewModel;
using MinhTuan.API.ViewModels.DataCategoryViewModel;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.DTOs.DataCategoryDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Service.SearchDTO;
using MinhTuan.Service.Services.DataCategoryService;
using Newtonsoft.Json;

namespace MinhTuan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataCategoryController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataCategoryService _dataCategoryService;

        public DataCategoryController(IMapper mapper, IHttpContextAccessor httpContextAccessor,IDataCategoryService dataCategoryService)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _dataCategoryService = dataCategoryService;

        }
        [HttpPost("get-data-by-page")]
        public async Task<IActionResult> GetDataByPage([FromBody] DataCategorySearchDTO searchDTO)
        {
            try
            {
                var sessionKey = $"searchCategory_{typeof(DataCategoryDTO).Name}_{JsonConvert.SerializeObject(searchDTO)}";
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

                var resultGetted = _dataCategoryService.GetDataByPage(searchDTO);

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
        public async Task<IActionResult> Create([FromBody] CreateDataCategoryViewModel model)
        {
            var serverResponse = new ResponseWithMessageDto() { Message = "Tạo mới thành công" };

            try
            {
                var modelDTO = _mapper.Map<DataCategoryDTO>(model);

                var isExistCode = _dataCategoryService.CheckExitCode(modelDTO.Code,modelDTO.ParentId, Guid.Empty);
                if (isExistCode)
                {
                    serverResponse.Message = "Trùng mã dữ liệu danh mục";
                    return Ok(serverResponse);
                }

                var isExistName = _dataCategoryService.CheckExitName(modelDTO.Name,modelDTO.ParentId, Guid.Empty);
                if (isExistName)
                {
                    serverResponse.Message = "Trùng tên dữ liệu danh mục";
                    return Ok(serverResponse);
                }

                await _dataCategoryService.CreateAsync(_mapper.Map<DataCategory>(modelDTO));
            }
            catch (Exception ex)
            {
                serverResponse.Message = "Có lỗi xảy ra";
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
                var data = await _dataCategoryService.GetByIdAsync(id);
                if (data == null)
                {
                    serverResponse.Message = "Không tìm thấy dữ liệu";
                    serverResponse.Status = "Fail";
                    return Ok(serverResponse);
                }
                await _dataCategoryService.SoftDeleteAsync(data);
                return Ok(serverResponse);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                            new ResponseWithMessageDto { Status = "Error", Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var serverResponse = new ResponseWithDataDto<DataCategoryDTO>() { Message = "Lấy thông tin thành công" };
            try
            {
                var result = await _dataCategoryService.GetByIdAsync(id);
                if (result != null)
                {
                    serverResponse.Data = _mapper.Map<DataCategoryDTO>(result);
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
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateDataCategoryViewModel model)
        {

            var serverResponse = new ResponseWithMessageDto() { Message = "Cập nhật thành công" };

            try
            {
                var modelDTO = _mapper.Map<DataCategoryDTO>(model);

                var isExistCode = _dataCategoryService.CheckExitCode(modelDTO.Code,model.ParentId, id);
                if (isExistCode)
                {
                    serverResponse.Message = "Trùng mã danh mục";
                    return Ok(serverResponse);
                }

                var isExistName = _dataCategoryService.CheckExitName(modelDTO.Name,modelDTO.ParentId, id);
                if (isExistName)
                {
                    serverResponse.Message = "Trùng tên danh mục";
                    return Ok(serverResponse);
                }
                var EntityNeedUpdate = await _dataCategoryService.GetByIdAsync(id);
                EntityNeedUpdate.Code = modelDTO.Code;
                EntityNeedUpdate.Name = modelDTO.Name;
                EntityNeedUpdate.Description = modelDTO.Description;
                await _dataCategoryService.UpdateAsync(EntityNeedUpdate);
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