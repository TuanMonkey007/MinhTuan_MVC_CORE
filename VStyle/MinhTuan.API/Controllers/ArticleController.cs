using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MinhTuan.API.Helper;
using MinhTuan.API.ViewModels.ArticleViewModel;
using MinhTuan.API.ViewModels.CategoryViewModel;
using MinhTuan.API.ViewModels.ProductViewModel;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.ArticleDTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.DTOs.ProductDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Service.SearchDTO;
using MinhTuan.Service.Services.ArticleService;
using MinhTuan.Service.Services.ProductService;
using Newtonsoft.Json;
using System.IO;
using System;
using Microsoft.AspNetCore.Authorization;

namespace MinhTuan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : Controller
    { 
        private readonly IMapper _mapper;
        private readonly IArticleService _articleService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ArticleController(
            IMapper mapper,
            IArticleService articleService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _mapper = mapper;
            _articleService = articleService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("get-data-by-page")]
        public async Task<IActionResult> GetDataByPage([FromBody] ArticleSearchDTO searchDTO)
        {
            try
            {
                var sessionKey = $"searchArticle_{typeof(ArticleDTO).Name}_{JsonConvert.SerializeObject(searchDTO)}";
                var jsonData = _httpContextAccessor.HttpContext.Session.GetString(sessionKey);

                if (!string.IsNullOrEmpty(jsonData)) // Kiểm tra jsonData có null hoặc rỗng không
                {
                    try
                    {
                        var usersFromSession = JsonConvert.DeserializeObject<PagedList<ArticleDTO>>(jsonData);

                        // Kiểm tra tính hợp lệ của usersFromSession
                        if (usersFromSession != null && usersFromSession.Items != null && usersFromSession.Items.Any())
                        {
                            var resultSession = new ResponseWithDataDto<PagedList<ArticleDTO>>()
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

                var resultGetted1 = _articleService.GetDataByPage(searchDTO);
                var resultGetted = resultGetted1.Result;
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
        public async Task<IActionResult> Create([FromForm] CreateArticleViewModel model)
        {
            var serverResponse = new ResponseWithMessageDto() { Message = "Tạo bài viết mới thành công" };

            try
            {
                var isExistSlug = _articleService.isExistSlug(model.Slug, Guid.Empty);
                if (isExistSlug)
                {
                    serverResponse.Message = "Bài viết đã bị trùng slug";
                    return Ok(serverResponse);
                }
                var modelDTO = _mapper.Map<ArticleDTO>(model);
                //Lưu ảnh trước khi tạo
                string thumbnailFileName = string.Empty;
                if (model.ThumbnailFile != null)
                {
                    thumbnailFileName = new UploadHandler().UploadArticleThumbnail(model.ThumbnailFile);//validate file ảnh
                    if (thumbnailFileName.Contains("Kích") || thumbnailFileName.Contains("Định"))
                    {
                        serverResponse.Message = thumbnailFileName;
                        return Ok(serverResponse);
                    }

                    modelDTO.Thumbnail = thumbnailFileName;
                }


                await _articleService.CreateAsync(_mapper.Map<Article>(modelDTO));
                var articleCreated = _articleService.FindByAsync(x => x.Slug == model.Slug).Result.FirstOrDefault();
                //Cập nhật danh mục bài viết
                await _articleService.UpdateArticleCategory(articleCreated.Id, model.ListCategory);

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
        [Authorize]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var serverResponse = new ResponseWithMessageDto() { Message = "Xóa thành công" };

            try
            {
                var data = await _articleService.GetByIdAsync(id);
                if (data == null)
                {
                    serverResponse.Message = "Không tìm thấy dữ liệu";
                    serverResponse.Status = "Fail";
                    return Ok(serverResponse);
                }
                await _articleService.SoftDeleteAsync(data);
                return Ok(serverResponse);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                            new ResponseWithMessageDto { Status = "Error", Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById(Guid id)
        {
            try
            {
                var result = await _articleService.GetArticleById(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }


        }
    
    [HttpGet("get-category-by-article-id/{id}")]
    public async Task<IActionResult> GetCategoryByArticleId(Guid id)
    {
        try
        {
            var response = await _articleService.GetCategoryByArticleId(id);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }


        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromForm] CreateArticleViewModel model)
        {
            var serverResponse = new ResponseWithMessageDto() { Message = "Cập nhật thành công" };
            try
            {
                var isExistSlug = _articleService.isExistSlug(model.Slug, id);
                if (isExistSlug)
                {
                    serverResponse.Message = "Bài viết đã bị trùng slug";
                    return Ok(serverResponse);
                }
                var modelDTO = _mapper.Map<ArticleDTO>(model);
                //Lưu ảnh trước khi tạo
                string thumbnailFileName = string.Empty;
                if (model.ThumbnailFile != null && model.OldThumbnail != null)
                {
                    string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "ArticleThumbnail");

                    thumbnailFileName = new UploadHandler().UploadArticleThumbnail(model.ThumbnailFile);//validate file ảnh
                    if (thumbnailFileName.Contains("Kích") || thumbnailFileName.Contains("Định"))
                    {
                        serverResponse.Message = thumbnailFileName;
                        return Ok(serverResponse);
                    }
                    var imagePath = Path.Combine(webRootPath, model.OldThumbnail);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    modelDTO.Thumbnail = thumbnailFileName;
                }

                var articleNeedUpdate = _mapper.Map<Article>(modelDTO);
                articleNeedUpdate.Id = id;
                await _articleService.UpdateAsync(articleNeedUpdate);
                //Cập nhật danh mục bài viết
                await _articleService.UpdateArticleCategory(articleNeedUpdate.Id, model.ListCategory);



            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok(serverResponse);
        }


        [HttpGet("get-relative-article-by-id/{id}")]
        public async Task<IActionResult> GetRelativeProductById(Guid id)
        {

            var response = await _articleService.GetRelativeArticleById(id);

            return Ok(response);
        }

    }
}
