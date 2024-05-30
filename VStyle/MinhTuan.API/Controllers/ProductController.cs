using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MinhTuan.API.Helper;
using MinhTuan.API.ViewModels.CategoryViewModel;
using MinhTuan.API.ViewModels.ProductViewModel;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.DTOs.ProductDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Service.SearchDTO;
using MinhTuan.Service.Services.ProductService;
using Newtonsoft.Json;

namespace MinhTuan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;


        public ProductController(IMapper mapper, IProductService productService,IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _productService = productService;
        }
        [HttpPost("get-data-by-page")]
        public async Task<IActionResult> GetDataByPage([FromBody] ProductSearchDTO searchDTO)
        {
            try
            {
                var sessionKey = $"searchProduct_{typeof(ProductDTO).Name}_{JsonConvert.SerializeObject(searchDTO)}";
                var jsonData = _httpContextAccessor.HttpContext.Session.GetString(sessionKey);

                if (!string.IsNullOrEmpty(jsonData)) // Kiểm tra jsonData có null hoặc rỗng không
                {
                    try
                    {
                        var usersFromSession = JsonConvert.DeserializeObject<PagedList<ProductDTO>>(jsonData);

                        // Kiểm tra tính hợp lệ của usersFromSession
                        if (usersFromSession != null && usersFromSession.Items != null && usersFromSession.Items.Any())
                        {
                            var resultSession = new ResponseWithDataDto<PagedList<ProductDTO>>()
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

                var resultGetted = _productService.GetDataByPage(searchDTO);

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
        public async Task<IActionResult> Create([FromForm] CreateProductViewModel model)
        {
            var serverResponse = new ResponseWithMessageDto() { Message = "Tạo mới thành công" };

            try
            {
                var modelDTO = _mapper.Map<ProductDTO>(model);

                var isExistCode = _productService.CheckExistCode(modelDTO.Code, Guid.Empty);
                if (isExistCode)
                {
                    serverResponse.Message = "Trùng mã sản phẩm";
                    return Ok(serverResponse);
                }

                var isExistName = _productService.CheckExistName(modelDTO.Name, Guid.Empty);
                if (isExistName)
                {
                    serverResponse.Message = "Trùng tên sản phẩm";
                    return Ok(serverResponse);
                }
                List<string> listImageFileName = new List<string>(); // Lấy ra danh sách tên ảnh
                if (model.ListImageFile != null)
                {

                    foreach (var item in model.ListImageFile)
                    {
                        var itemFileName = new UploadHandler().UploadProductImage(item);//validate file ảnh
                        if (itemFileName.Contains("Kích") || itemFileName.Contains("Định"))
                        {
                            serverResponse.Message = itemFileName; //Trả về nếu ảnh lỗi
                            return Ok(serverResponse);
                        }
                        listImageFileName.Add(itemFileName);
                    }
                }
                string thumbnailFileName = string.Empty;
                if (model.ThumbnailFile != null)
                {
                    thumbnailFileName = new UploadHandler().UploadProductImage(model.ThumbnailFile);//validate file ảnh
                    if (thumbnailFileName.Contains("Kích") || thumbnailFileName.Contains("Định"))
                    {
                        serverResponse.Message = thumbnailFileName;
                        return Ok(serverResponse);
                    }
                }

                    await _productService.CreateAsync(_mapper.Map<Product>(modelDTO));
                    var productCreated = _productService.GetByCode(model.Code);
                
                    _productService.UpdateProductImageThumbnail(productCreated.Id, thumbnailFileName);//Cập nhật lại ảnh đại diện
                
              
                    _productService.UpdateProductImage(productCreated.Id, listImageFileName);//Cập nhật tất cả ảnh sản phẩm
              
                   
                    _productService.UpdateProductCategory(productCreated.Id, model.ListCategory);//Cập nhật lại danh mục của sản phẩm
                

            }
            catch (Exception ex)
            {
                serverResponse.Message = "Có lỗi xảy ra khi tạo sản phẩm";
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
                var data = await _productService.GetByIdAsync(id);
                if (data == null)
                {
                    serverResponse.Message = "Không tìm thấy dữ liệu";
                    serverResponse.Status = "Fail";
                    return Ok(serverResponse);
                }
                await _productService.SoftDeleteAsync(data);
                return Ok(serverResponse);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                            new ResponseWithMessageDto { Status = "Error", Message = ex.Message });
            }
        }
    }
}
