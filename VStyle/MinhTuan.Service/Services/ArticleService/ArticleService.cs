using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.DTOs.ArticleDTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Domain.Repository.ArticleRepository;
using MinhTuan.Domain.Repository.DataCategoryRepository;
using MinhTuan.Service.Core;
using MinhTuan.Service.SearchDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MinhTuan.Service.Services.ArticleService
{
    public class ArticleService : Service<Article>, IArticleService
    {
        private readonly IDataCategoryRepository _dataCategoryRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ArticleService(IUnitOfWork unitOfWork,
            IDataCategoryRepository dataCategoryRepository,
            IArticleRepository articleRepository,
            IArticleCategoryRepository articleCategoryRepository
            ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dataCategoryRepository = dataCategoryRepository;
            _articleRepository = articleRepository;
            _articleCategoryRepository = articleCategoryRepository;
        }
        private static string ImageToBase64(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return null;

            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "ArticleThumbnail", imagePath);
                byte[] imageBytes = System.IO.File.ReadAllBytes(path);
                return Convert.ToBase64String(imageBytes);
            }
            catch
            {
                return null; // Hoặc trả về một giá trị mặc định khác
            }
        }

        private static string GetContentTypeFromFileName(string fileName)
        {
            var fileExtension = Path.GetExtension(Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "ArticleThumbnail", fileName)).ToLowerInvariant();
            return fileExtension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".bmp" => "image/bmp", // Thêm định dạng BMP
                ".webp" => "image/webp", // Thêm định dạng WebP
                _ => "application/octet-stream",
            };
        }

        public async Task<ResponseWithDataDto<PagedList<ArticleDTO>>> GetArticleById(Guid id)
        {
            var search = new ArticleSearchDTO() { Id_Filter = id, PageIndex = 1, PageSize = 1 };
            return await GetDataByPage(search);
        }

        public async Task<ResponseWithDataDto<PagedList<ArticleDTO>>> GetDataByPage(ArticleSearchDTO searchDTO)
        {

            try
            {
                var query = from entityTbl in _articleRepository.GetQueryable()
                            where entityTbl.IsDelete != true //Xét xóa mềm
                            select new ArticleDTO
                            {
                                Id = entityTbl.Id,
                               Title = entityTbl.Title,
                               Summary = entityTbl.Summary,
                               Content =entityTbl.Content,
                               Thumbnail = entityTbl.Thumbnail,
                                Tag = entityTbl.Tag,
                                Slug = entityTbl.Slug,
                                CountView = entityTbl.CountView,
                                Status = entityTbl.Status,
                                PublishDate = entityTbl.PublishDate,
                                Author = entityTbl.CreatedBy !=null ? entityTbl.CreatedBy : entityTbl.UpdatedBy,
                                AuthorId = entityTbl.CreatedID !=null ? entityTbl.CreatedID : entityTbl.UpdatedID

                            };


                if (searchDTO != null)
                {
                    if (searchDTO.Id_Filter.HasValue)
                    {
                        query = query.Where(x => x.Id.Equals(searchDTO.Id_Filter));
                    }
                    if (!string.IsNullOrEmpty(searchDTO.Title_Filter))
                    {
                        var idSearch = searchDTO.Title_Filter.ToString().RemoveAccentsUnicode();
                        var isNormal = searchDTO.Title_Filter.ToString().ToLower() != idSearch.ToLower();
                        var list = _articleRepository.GetQueryable().Select(x => x.Title).ToList().Where(x => x.ToString().ToLower().RemoveAccentsUnicode().Contains(idSearch.ToLower()));
                        query = query.Where(x => list.Contains(x.Title));
                    }
                    if (!string.IsNullOrEmpty(searchDTO.Author_Filter))
                    {

                        var idSearch = searchDTO.Author_Filter.RemoveAccentsUnicode();
   
                        var listCreated = _articleRepository.GetQueryable().Select(x => x.CreatedBy).ToList().Where(x =>x != null && x.ToString().ToLower().RemoveAccentsUnicode().Contains(idSearch.ToLower()));
                        var listUpdated = _articleRepository.GetQueryable().Select(x => x.UpdatedBy).ToList().Where(x =>x != null && x.ToString().ToLower().RemoveAccentsUnicode().Contains(idSearch.ToLower()));
                       
 
                        query = query.Where(x => listCreated.Contains(x.Author) || listUpdated.Contains(x.Author));
                    }
                  
                    if (searchDTO.StartDate_Filter.HasValue)
                    {
                        query = query.Where(x => x.PublishDate.Date >= searchDTO.StartDate_Filter.Value.Date);
                    }
                    if (searchDTO.EndDate_Filter.HasValue)
                    {
                        query = query.Where(x => x.PublishDate.Date <= searchDTO.EndDate_Filter.Value.Date);
                    }
                    if (!string.IsNullOrEmpty(searchDTO.Slug_Filter))
                    {
                        query = query.Where(x => x.Slug.Contains(searchDTO.Slug_Filter));
                    }

                    if (searchDTO.Status_Filter.HasValue)
                    {
                        query = query.Where(x => x.Status == searchDTO.Status_Filter);
                    }
                    if (searchDTO.Category_Filter != null && searchDTO.Category_Filter.Length > 0)
                    {
                        query = query.Where(p => _articleCategoryRepository.GetQueryable().Any(pc => pc.ArticleId == p.Id && searchDTO.Category_Filter.Contains(pc.CategoryId)));
                    }
                    if (!string.IsNullOrEmpty(searchDTO.sortQuery))
                    {
                        query = query.OrderBy(searchDTO.sortQuery);
                    }
                    else
                    {
                        query = query.OrderBy(x => x.PublishDate);
                    }
                }
                var result = PagedList<ArticleDTO>.Create(query, searchDTO);
                foreach (var articleDTO in result.Items)
                {

                    articleDTO.ThumbnailBase64 = ImageToBase64(articleDTO.Thumbnail);
                    articleDTO.ThumbnailContentType = GetContentTypeFromFileName(articleDTO.Thumbnail);

                    //lấy ra tên danh mục
                    var listCategory =  _articleCategoryRepository.FindByAsync(x=>x.ArticleId.Equals(articleDTO.Id)).Result;

                    List<Guid> listCategoryId = new List<Guid>();
                    List<string> listCategoryName = new List<string>();
                    foreach (var item in listCategory)
                    {
                        listCategoryId.Add(item.CategoryId);
                        var category = await _dataCategoryRepository.FindByAsync(x => x.Id.Equals(item.CategoryId));
                        var xxx = category.FirstOrDefault();
                        listCategoryName.Add(xxx.Name);
                    }
                    articleDTO.ListCategory = listCategoryId;
                    articleDTO.ListCategoryName = listCategoryName;
                    
               
                }
                return new ResponseWithDataDto<PagedList<ArticleDTO>>()
                {
                    Data = result,

                    Message = "Lấy thành công"
                };

            }
            catch (Exception ex)
            {
                return new ResponseWithDataDto<PagedList<ArticleDTO>>()
                {
                    Data = null,

                    Message = ex.Message

                };
            }
        }

        public async Task<bool> UpdateArticleCategory(Guid id, List<Guid> listCategory)
        {
            try
            {
                var product = _articleRepository.FindBy(p => p.Id.Equals(id) && p.IsDelete != true).FirstOrDefault();
                if (product == null)
                {
                    return false;
                }
                var existingProductCategories = _articleCategoryRepository.FindBy(pc => pc.ArticleId.Equals(id)).ToList();
                if (existingProductCategories.Any())
                {
                    _articleCategoryRepository.DeleteRange(existingProductCategories);
                }

                var newProductCategories = listCategory.Select(categoryId => new Article_Category
                {
                    ArticleId = id,
                    CategoryId = categoryId
                }).ToList();
                if (newProductCategories.Any())
                {
                    _articleCategoryRepository.AddRange(newProductCategories);
                }
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<ResponseWithDataDto<List<Guid>>> GetCategoryByArticleId(Guid id)
        {
            var result = await _articleCategoryRepository.FindByAsync(x => x.ArticleId.Equals(id) && x.IsDelete != true);
            var newList = new List<Guid>();
            foreach (var item in result)
            {
                newList.Add(item.CategoryId);
            }
            var response = new ResponseWithDataDto<List<Guid>>() { Data = newList };
            return response;
        }

        public bool isExistSlug(string slug,Guid id)
        {

            if (id == Guid.Empty)
            {
                return _articleRepository.FindBy(x => x.Slug == slug && x.IsDelete != true).Any();
            }
            return _articleRepository.FindBy(x => x.Slug == slug && x.IsDelete != true && x.Id != id).Any();
        }

        public async Task<ResponseWithDataDto<PagedList<ArticleDTO>>> GetRelativeArticleById(Guid id)
        {
            var search = new ArticleSearchDTO() { PageIndex = 1, PageSize = 20 };
            var product_Categories = await _articleCategoryRepository.FindByAsync(x => x.ArticleId == id);
            var categoryIds = product_Categories.Select(x => x.CategoryId).ToArray();
            search.Category_Filter = categoryIds;
            var result = await GetDataByPage(search);
            var productToRemove = result.Data.Items.FirstOrDefault(p => p.Id == id);
            if (productToRemove != null)
            {
                result.Data.Items.Remove(productToRemove);
            }
            return result;
        }
    }
}
