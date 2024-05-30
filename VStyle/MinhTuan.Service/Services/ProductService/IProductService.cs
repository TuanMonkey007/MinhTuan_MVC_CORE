using Microsoft.AspNetCore.Http;
using MinhTuan.Domain.Core.DTO;
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

namespace MinhTuan.Service.Services.ProductService
{
    public interface IProductService : IService<Product>
    {
        ResponseWithDataDto<PagedList<ProductDTO>> GetDataByPage(ProductSearchDTO searchDTO);
        bool UpdateProductCategory(Guid id, List<Guid> listCategory);
        bool UpdateProductImage(Guid id, List<string> listImageFileName);
        bool UpdateProductImageThumbnail(Guid id, string thumbnailFileName);
        ProductDTO GetByCode(string code);
        bool CheckExistCode(string code,Guid id);
        bool CheckExistName(string name, Guid id);
    }
}
