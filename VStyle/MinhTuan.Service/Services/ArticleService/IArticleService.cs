using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.ArticleDTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.DTOs.ProductDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Service.Core.Services;
using MinhTuan.Service.SearchDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.ArticleService
{
    public interface IArticleService:IService<Article>
    {
       Task<ResponseWithDataDto<PagedList<ArticleDTO>>> GetDataByPage(ArticleSearchDTO searchDTO);
        Task<bool> UpdateArticleCategory(Guid id, List<Guid> listCategory);
        Task<ResponseWithDataDto<PagedList<ArticleDTO>>> GetArticleById(Guid id);
        Task<ResponseWithDataDto<List<Guid>>> GetCategoryByArticleId(Guid id);
        bool isExistSlug(string slug,Guid id);
        Task<ResponseWithDataDto<PagedList<ArticleDTO>>> GetRelativeArticleById(Guid id);
    }
}
