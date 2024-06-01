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
        Task<bool> UpdateProductCategory(Guid id, List<Guid> listCategory);
        Task<bool> UpdateProductImage(Guid id, List<string> listImageFileName);
        Task<bool> UpdateProductImageThumbnail(Guid id, string thumbnailFileName);
        Task<bool> UpdateProductVariant(Guid id,List<ProductVariantDTO> listModel);
        List<ProductVariantDTO> GetAllProductVariantByProductId(Guid id);
        ProductDTO GetByCode(string code);
        bool CheckExistCode(string code,Guid id);
        bool CheckExistName(string name, Guid id);
        Task<ResponseWithDataDto<List<ImageResponseDTO>>> GetAllImageOfProduct(Guid id);
        Task<ResponseWithDataDto<PagedList<ProductDTO>>> GetProductById(Guid id);
        Task<ResponseWithDataDto<List<Guid>>> GetCategoryByProductId(Guid id);
    }
}
