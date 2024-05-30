using AutoMapper;
using Microsoft.AspNetCore.Http;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.DTOs.ProductDTO;
using MinhTuan.Domain.DTOs.ProductDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Domain.Repository.CategoryRepositoy;
using MinhTuan.Domain.Repository.ProductRepository;
using MinhTuan.Service.Core;
using MinhTuan.Service.SearchDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.ProductService
{
    internal class ProductService : Service<Product>, IProductService
    {

        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository,
            IProductCategoryRepository productCategoryRepository,
            IProductImageRepository productImageRepository
            ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _productImageRepository = productImageRepository;
        }

        public bool CheckExistCode(string code, Guid id)
        {
            if (id == Guid.Empty)
            {
                return _productRepository.FindBy(x => x.Code == code && x.IsDelete != true).Any();
            }
            return _productRepository.FindBy(x => x.Code == code && x.IsDelete != true && x.Id != id).Any();
        }

        public bool CheckExistName(string name, Guid id)
        {
            if (id == Guid.Empty)
            {
                return _productRepository.FindBy(x => x.Name == name && x.IsDelete != true).Any();
            }
            return _productRepository.FindBy(x => x.Name == name && x.IsDelete != true && x.Id != id).Any();
        }

        public ProductDTO GetByCode(string code)
        {
           var result =  _productRepository.FindBy(x => x.Code == code &&x.IsDelete !=true).FirstOrDefault();
            return _mapper.Map<ProductDTO>(result);
        }

        public ResponseWithDataDto<PagedList<ProductDTO>> GetDataByPage(ProductSearchDTO searchDTO)
        {
            try
            {
                var query = from entityTbl in _productRepository.GetQueryable()
                            where entityTbl.IsDelete != true //Xét xóa mềm
                            select new ProductDTO
                            {
                                Id = entityTbl.Id,
                                Name = entityTbl.Name ?? string.Empty,
                                Code = entityTbl.Code ?? string.Empty,
                                Description = entityTbl.Description ?? string.Empty,
                                Price = entityTbl.Price,
                                StockQuantity = entityTbl.StockQuantity
                                
                            };


                if (searchDTO != null)
                {
                    if (!string.IsNullOrEmpty(searchDTO.Name_Filter))
                    {
                        var idSearch = searchDTO.Name_Filter.ToString().RemoveAccentsUnicode();
                        var isNormal = searchDTO.Name_Filter.ToString().ToLower() != idSearch.ToLower();
                        var list = _productRepository.GetQueryable().Select(x => x.Name).ToList().Where(x => x.ToString().ToLower().RemoveAccentsUnicode().Contains(idSearch.ToLower()));
                        query = query.Where(x => list.Contains(x.Name));

                    }
                    if (!string.IsNullOrEmpty(searchDTO.Code_Filter))
                    {
                        var idSearch = searchDTO.Code_Filter.ToString();
                        var isNormal = searchDTO.Code_Filter.ToString().ToLower() != idSearch.ToLower();
                        var list = _productRepository.GetQueryable().Select(x => x.Code).ToList().Where(x => x.ToString().ToLower().Contains(idSearch.ToLower()));
                        query = query.Where(x => list.Contains(x.Code));
                    }
                    if (searchDTO.Price_Filter != null)
                    {
                        query = query.Where(x => x.Price == searchDTO.Price_Filter);
                    }
                }
                var result = PagedList<ProductDTO>.Create(query, searchDTO);
                return new ResponseWithDataDto<PagedList<ProductDTO>>()
                {
                    Data = result,

                    Message = "Lấy thành công"
                };

            }
            catch (Exception ex)
            {
                return new ResponseWithDataDto<PagedList<ProductDTO>>()
                {
                    Data = null,

                    Message = ex.Message

                };
            }
        }

        public  bool UpdateProductCategory(Guid id, List<Guid> listCategory)
        {
            try
            {
                var product =  _productRepository.FindBy(p => p.Id.Equals(id) && p.IsDelete != true).FirstOrDefault();
                if (product == null)
                {
                    return false;
                }
                var existingProductCategories = _productCategoryRepository.FindBy(pc => pc.ProductId.Equals(id)).ToList();
                if (existingProductCategories.Any())
                {
                    _productCategoryRepository.DeleteRange(existingProductCategories);
                }
               
                var newProductCategories = listCategory.Select(categoryId => new Product_Category
                {
                    ProductId = id,
                    CategoryId = categoryId
                }).ToList();
                if (newProductCategories.Any())
                {
                    _productCategoryRepository.AddRange(newProductCategories);
                }
                _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
            




        }

        public bool UpdateProductImage(Guid id, List<string> listImageFileName)
        {
            try
            {
                // Tìm xem có sản phẩm cần sửa không
                var product = _productRepository.FindBy(p => p.Id.Equals(id) && p.IsDelete != true).FirstOrDefault();
                if (product == null)
                {
                    return false;
                }
                //Tìm tất cả ảnh cũ và xóa nó đi
                var existingProductImages = _productImageRepository.FindBy(pc => pc.ProductId.Equals(id) &&pc.IsThumbnail==false ).ToList();
                if (existingProductImages.Any())
                {
                    _productImageRepository.DeleteRange(existingProductImages);
                }
                if(listImageFileName.Count > 0)
                {
                    foreach(var item in listImageFileName)
                    {
                        var newImageProduct = new Product_Image() { ProductId = id, Path = item, IsThumbnail = false };
                        _productImageRepository.Add(newImageProduct);//Thêm ảnh mới 
                    }
                }
              
                _unitOfWork.SaveChangesAsync();

                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateProductImageThumbnail(Guid id, string thumbnailFileName)
        {
           // if (thumbnailFileName == null) return false;
            try
            {
                // Tìm ảnh thumnail hiện tại
                var product = _productRepository.FindBy(p => p.Id.Equals(id) && p.IsDelete != true).FirstOrDefault();
                if (product == null)
                {
                    return false;
                }
                //Xóa nó đi
                var existingProductImages = _productImageRepository.FindBy(pc => pc.ProductId.Equals(id) && pc.IsThumbnail==true).ToList();
                if (existingProductImages.Any())
                {
                    _productImageRepository.DeleteRange(existingProductImages);
                }
                if(!string.IsNullOrEmpty(thumbnailFileName))
                {
                    var newProductThumbnail = new Product_Image() { IsThumbnail = true, Path = thumbnailFileName, ProductId = id };
                    _productImageRepository.Add(newProductThumbnail);
                }
               
               
                _unitOfWork.SaveChangesAsync();
                
                //Thêm ảnh mới làm thumbnail
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
