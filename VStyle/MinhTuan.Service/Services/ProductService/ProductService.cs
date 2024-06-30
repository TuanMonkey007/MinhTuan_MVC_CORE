using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.DTOs.DataCategoryDTO;
using MinhTuan.Domain.DTOs.ProductDTO;

using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Domain.Repository.CategoryRepositoy;
using MinhTuan.Domain.Repository.DataCategoryRepository;
using MinhTuan.Domain.Repository.ProductRepository;
using MinhTuan.Service.Core;
using MinhTuan.Service.SearchDTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System.Linq.Dynamic.Core;
using MinhTuan.Domain.DTOs.ArticleDTO;
using MinhTuan.Domain.Repository.OrderItemRepository;

namespace MinhTuan.Service.Services.ProductService
{
    internal class ProductService : Service<Product>, IProductService
    {

        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IDataCategoryRepository _dataCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository,
            IProductCategoryRepository productCategoryRepository,
            IProductVariantRepository productVariantRepository,
            IProductImageRepository productImageRepository,
            IDataCategoryRepository dataCategoryRepository,
            ICategoryRepository categoryRepository,
            IOrderItemRepository orderItemRepository
            ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _productImageRepository = productImageRepository;
            _productVariantRepository = productVariantRepository;
            _dataCategoryRepository = dataCategoryRepository;
            _categoryRepository = categoryRepository;
            _orderItemRepository = orderItemRepository;
        }
        private static string ImageToBase64(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return null;

            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Product", imagePath);
                byte[] imageBytes = System.IO.File.ReadAllBytes(path);
                return Convert.ToBase64String(imageBytes);
            }
            catch
            {
                return null; // Hoặc trả về một giá trị mặc định khác
            }
        }
        private static string GetContentTypeFromExtension(string fileExtension)
        {
            return fileExtension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".bmp" => "image/bmp", // Thêm định dạng BMP
                ".webp" => "image/webp", // Thêm định dạng WebP
                _ => "application/octet-stream",
            };
        }
        private static string GetContentTypeFromFileName(string fileName)
        {
            var fileExtension = Path.GetExtension(Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Product", fileName)).ToLowerInvariant();
            return fileExtension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".bmp" => "image/bmp", // Thêm định dạng BMP
                ".webp" => "image/webp", // Thêm định dạng WebP
                _ => "application/octet-stream",
            };
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

        //public  List<ProductVariantDTO> GetAllProductVariantByProductId(Guid id)
        //{
        //    var result =   _productVariantRepository.FindBy(x => x.ProductId.Equals(id)).ToList();

        //    var resultList =  _mapper.Map<List<ProductVariantDTO>>(result);
        //    return resultList;
        //}
        public List<ProductVariantDTO> GetAllProductVariantByProductId(Guid id)
        {
            // Lấy tất cả các ProductVariant với ProductId tương ứng
            var result = _productVariantRepository.FindBy(x => x.ProductId.Equals(id)).ToList();

            // Lấy danh sách các ProductVariantId từ kết quả
            var productVariantIds = result.Select(x => x.Id).ToList();
           
            // Ánh xạ danh sách ProductVariant sang ProductVariantDTO
            var resultList = _mapper.Map<List<ProductVariantDTO>>(result);
            foreach (var item in resultList)
            {
                var isExist= _orderItemRepository.FindBy(x => x.ProductVariantId.Equals(item.Id)).Any();
                item.Changeable = !isExist;
            }
            return resultList;
        }

        //xử lý bất đồng bộ
        public async  Task<ResponseWithDataDto<PagedList<ProductDTO>>> GetDataByPage(ProductSearchDTO searchDTO)
        {
            try
            {
             var query = from product in _productRepository.GetQueryable()
                           
                            where product.IsDelete != true 
                            select new ProductDTO
                            {
                                Id = product.Id,
                                Name = product.Name ?? string.Empty,
                                Code = product.Code ?? string.Empty,
                                Description = product.Description ?? string.Empty,
                                Price = product.Price,
                                StockQuantity = product.StockQuantity,
                                CreatedDate = product.CreatedDate,
                               
                               
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
                        query = query.Where(x => x.Price >= searchDTO.Price_Filter[0] && x.Price <= searchDTO.Price_Filter[1]);
                    }
                    if (searchDTO.Category_Filter != null && searchDTO.Category_Filter.Length > 0)
                    {
                        query = query.Where(p => _productCategoryRepository.GetQueryable().Any(pc => pc.ProductId == p.Id && searchDTO.Category_Filter.Contains(pc.CategoryId)));
                    }
                    if (searchDTO.Id_Filter.HasValue)
                    {
                        query = query.Where(x => x.Id.Equals(searchDTO.Id_Filter));
                    }
                    if (!searchDTO.CategoryCode_Filter.IsNullOrEmpty())
                    {
                        var parentCategory = _categoryRepository.FindByAsync(x => x.Code.Equals("SAN_PHAM"));
                      var parentId= parentCategory.Result.FirstOrDefault().Id;
                        var categoryId = _dataCategoryRepository.FindBy(x => x.Code.Equals(searchDTO.CategoryCode_Filter) && x.ParentId.Equals(parentId)  ).FirstOrDefault().Id;
                        query = query.Where(p => _productCategoryRepository.GetQueryable().Any(pc => pc.ProductId == p.Id && pc.CategoryId.Equals(categoryId)));
                    }
                    if (searchDTO.Size_Filter != null && searchDTO.Size_Filter.Length > 0)
                    {
                        query = query.Where(p => _productVariantRepository.GetQueryable().Any(pc => pc.ProductId == p.Id && searchDTO.Size_Filter.Contains(pc.SizeId)));
                    }
                    if (searchDTO.Color_Filter != null && searchDTO.Color_Filter.Length > 0)
                    {
                        query = query.Where(p => _productVariantRepository.GetQueryable().Any(pc => pc.ProductId == p.Id && searchDTO.Color_Filter.Contains(pc.ColorId)));
                    }


                    if (!string.IsNullOrEmpty(searchDTO.sortQuery))
                    {
                        query = query.OrderBy(searchDTO.sortQuery);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.CreatedDate);
                    }



                }
                var result = PagedList<ProductDTO>.Create(query, searchDTO);
                // Lấy danh sách ProductId
                var productIds = result.Items.Select(p => p.Id).ToList();
                // Truy vấn ảnh thumbnail (chỉ lấy những ảnh có IsThumbnail = true)
                // Truy vấn ảnh thumbnail (chỉ lấy ảnh đầu tiên có IsThumbnail = true cho mỗi sản phẩm)
                var thumbnailImages =  _productImageRepository.GetQueryable()
                    .Where(pi => productIds.Contains(pi.ProductId) && pi.IsThumbnail)
                    .GroupBy(pi => pi.ProductId)
                    .Select(g => g.FirstOrDefault()) // Lấy ảnh thumbnail đầu tiên trong mỗi nhóm
                    .ToList();
                // Gán ảnh thumbnail vào ProductDTO
                foreach (var productDTO in result.Items)
                {
                    var thumbnailImage = thumbnailImages.FirstOrDefault(pi => pi.ProductId == productDTO.Id);
                    if (thumbnailImage == null) continue;
                    productDTO.Thumbnail = thumbnailImage.Path;
                    productDTO.ThumbnailBase64 = ImageToBase64(thumbnailImage.Path);
                    productDTO.ThumbnailContentType = GetContentTypeFromExtension(Path.GetExtension(thumbnailImage.Path).ToLowerInvariant());
                    //Lấy ra tất cả danh mục của sản phẩm
                    var listCategory = _productCategoryRepository.FindByAsync(x => x.ProductId.Equals(productDTO.Id)).Result;

                    List<Guid> listCategoryId = new List<Guid>();
                    List<string> listCategoryName = new List<string>();
                    foreach (var item in listCategory)
                    {
                        listCategoryId.Add(item.CategoryId);
                        var category = await _dataCategoryRepository.FindByAsync(x => x.Id.Equals(item.CategoryId));
                        var xxx = category.FirstOrDefault();
                        listCategoryName.Add(xxx.Name);
                    }
                    productDTO.ListCategory = listCategoryId;
                    productDTO.ListCategoryName = listCategoryName;
                }
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

        public async  Task<bool> UpdateProductCategory(Guid id, List<Guid> listCategory)
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
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
            




        }

        public async Task<bool> UpdateProductImage(Guid id, List<string> listImageFileName)
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
                    string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Product");
                    foreach (var productImage in existingProductImages)
                    {
                        // Xóa ảnh cũ trên máy chủ
                        var imagePath = Path.Combine(webRootPath, productImage.Path);
                        if (File.Exists(imagePath))
                        {
                            File.Delete(imagePath);
                        }
                    }
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
              
                await _unitOfWork.SaveChangesAsync();

                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateProductImageThumbnail(Guid id, string thumbnailFileName)
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
                    string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Product");
                    foreach (var productImage in existingProductImages)
                    {
                        // Xóa ảnh cũ trên máy chủ
                        var imagePath = Path.Combine(webRootPath, productImage.Path);
                        if (File.Exists(imagePath))
                        {
                            File.Delete(imagePath);
                        }
                    }
                    _productImageRepository.DeleteRange(existingProductImages);
                }
                if(!string.IsNullOrEmpty(thumbnailFileName))
                {
                    var newProductThumbnail = new Product_Image() { IsThumbnail = true, Path = thumbnailFileName, ProductId = id };
                    _productImageRepository.Add(newProductThumbnail);
                }


                await _unitOfWork.SaveChangesAsync();
                
                //Thêm ảnh mới làm thumbnail
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task UpdateProductStockQuantity(Guid productId)
        {
            // 1. Tìm sản phẩm
            var product =  _productRepository.GetById(productId);
            if (product == null)
            {
                throw new Exception("Không tìm thấy sản phẩm.");
            }

            // 2. Tính tổng StockQuantity của các ProductVariant
            var totalStockQuantity = await _productVariantRepository.GetQueryable()
                .Where(pv => pv.ProductId == productId)
                .SumAsync(pv => pv.StockQuantity);

            // 3. Cập nhật StockQuantity của sản phẩm
            product.StockQuantity = totalStockQuantity;

            // 4. Lưu thay đổi
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateProductVariant(Guid id, List<ProductVariantDTO> listModel)
        {
            try
            {
                var product = _productRepository.FindBy(p => p.Id.Equals(id) && p.IsDelete != true).FirstOrDefault();
                if (product == null)
                {
                    return false;
                }
                foreach(var item in listModel)
                {
                    //Nếu đã tồn tại thì cập nhật số lượng và giá
                    if(item.Id != Guid.Empty)
                    {
                       var itemUpdate =  _mapper.Map<Product_Variant>(item);
                       _productVariantRepository.Update(itemUpdate);
                    }
                    else
                    {
                        var itemCreate = _mapper.Map<Product_Variant>(item);
                        _productVariantRepository.Add(itemCreate);
                    }
                }

                var existingProductVariants = _productVariantRepository.FindBy(pv => pv.ProductId.Equals(id))
                    .ToList();
                //Xóa các bản ghi không có trong listmodel
                if (existingProductVariants.Any())
                {
                    var existingVariantsCopy = new List<Product_Variant>(existingProductVariants);
                    foreach (var item in existingVariantsCopy)
                    {
                        var matchingNewVariant = listModel.FirstOrDefault(nv =>
                            nv.SizeId == item.SizeId && nv.ColorId == item.ColorId);
                        if (matchingNewVariant == null)
                        {
                            _productVariantRepository.Delete(item);
                        }
                    }
                }
                    //// Lặp qua từng biến thể hiện có (dùng bản sao)
                    //for (int i = 0; i < existingVariantsCopy.Count; i++)
                    //{
                    //    var existingVariant = existingVariantsCopy[i];
                    //    var matchingNewVariant = listModel.FirstOrDefault(nv =>
                    //        nv.SizeId == existingVariant.SizeId && nv.ColorId == existingVariant.ColorId);

                    //    if (matchingNewVariant == null)
                    //    {
                    //        // Nếu không tồn tại, thực hiện soft delete
                    //        existingVariant.IsDelete = true;
                    //    }
                    //    else
                    //    {
                    //        // Nếu tồn tại, cập nhật thông tin
                    //        existingVariant.Price = matchingNewVariant.Price;
                    //        existingVariant.StockQuantity = matchingNewVariant.StockQuantity;
                    //        // ... (cập nhật các thuộc tính khác nếu cần)

                    //        // Loại bỏ khỏi listModel để tối ưu việc thêm mới
                    //       listModel.Remove(matchingNewVariant);
                    //    }

                    //}
                    //await _unitOfWork.SaveChangesAsync();

                    //// Lặp qua từng biến thể mới (sử dụng vòng lặp for)
                    //for (int i = 0; i < listModel.Count; i++)
                    //{

                    //    var newVariant = listModel[i];

                    //    // Kiểm tra xem biến thể này có tồn tại trong danh sách hiện có không
                    //    var existingVariant = existingProductVariants.FirstOrDefault(pv =>
                    //        pv.ProductId == id && pv.SizeId == newVariant.SizeId && pv.ColorId == newVariant.ColorId);

                    //    if (existingVariant == null)
                    //    {
                    //        // Nếu không tồn tại, thêm biến thể mới vào repository
                    //        var productVariant = new Product_Variant()
                    //        {
                    //            ProductId = id,
                    //            ColorId = newVariant.ColorId,
                    //            SizeId = newVariant.SizeId,
                    //            Price = newVariant.Price,
                    //            StockQuantity = newVariant.StockQuantity
                    //        };
                    //        _productVariantRepository.Add(productVariant);
                    //    }
                    //    else
                    //    {
                    //        listModel.Remove(newVariant);
                    //        i--;
                    //    }
                    //}
                    await _unitOfWork.SaveChangesAsync();
                // Cập nhật StockQuantity của sản phẩm
                await UpdateProductStockQuantity(id);
                
                return true;
            }
            catch
            {
                return false; // Hoặc ném ra exception cụ thể
            }
        }

        public async Task<ResponseWithDataDto<List<ImageResponseDTO>>> GetAllImageOfProduct(Guid id)
        {//Trả về danh sách tất cả các ảnh chi tiết của sản phẩm
            
            var response = new ResponseWithDataDto<List<ImageResponseDTO>>() { Data = new List<ImageResponseDTO>()};
            var result = await _productImageRepository.FindByAsync(x => x.ProductId.Equals(id)&&x.IsThumbnail!=true && x.IsDelete != true);
            foreach(var item in result)
            {
                var imageResponse = new ImageResponseDTO()
                {
                    Path = item.Path,
                    Base64 = ImageToBase64(item.Path),
                    ContentType = GetContentTypeFromFileName(item.Path)
                };
                response.Data.Add(imageResponse);
            }
            return response;
        }

        public async Task<ResponseWithDataDto<PagedList<ProductDTO>>> GetProductById(Guid id)
        {
            var search = new ProductSearchDTO() { Id_Filter = id, PageIndex = 1, PageSize = 1 };
            var result = await  GetDataByPage(search);
            return result;
            
        }

        public async Task<ResponseWithDataDto<List<Guid>>> GetCategoryByProductId(Guid id)
        {
            var result =await _productCategoryRepository.FindByAsync(x => x.ProductId.Equals(id) && x.IsDelete != true);
            var newList = new List<Guid>();
            foreach(var item in result)
            {
                newList.Add(item.CategoryId);
            }
            var response = new ResponseWithDataDto<List<Guid>>() { Data = newList };
            return response;
        }

        public async Task<ResponseWithDataDto<List<DataCategoryDTO>>> GetAllSizeOfProductById(Guid id)
        {
            var result = await _productVariantRepository.FindByAsync(x => x.ProductId.Equals(id));
            var lisSizeId = new List<Guid>();
            foreach (var item in result)
            {
                lisSizeId.Add(item.SizeId);
            }
            var result2 = await _dataCategoryRepository.FindByAsync(x => lisSizeId.Contains(x.Id));
            var listSizeDTO = _mapper.Map<List<DataCategoryDTO>>(result2);
            var response = new ResponseWithDataDto<List<DataCategoryDTO>>() { Data = listSizeDTO };
            return response;
        }
        public async Task<ResponseWithDataDto<List<DataCategoryDTO>>> GetAllColorOfProductById(Guid id)
        {
            var result = await _productVariantRepository.FindByAsync(x => x.ProductId.Equals(id));
            var lisSizeId = new List<Guid>();
            foreach (var item in result)
            {
                lisSizeId.Add(item.ColorId);
            }
            var result2 = await _dataCategoryRepository.FindByAsync(x => lisSizeId.Contains(x.Id));
            var listSizeDTO = _mapper.Map<List<DataCategoryDTO>>(result2);
            var response = new ResponseWithDataDto<List<DataCategoryDTO>>() { Data = listSizeDTO };
            return response;
        }

        public ResponseWithDataDto<PagedList<ProductDTO>> GetDataByListImagePath(ImageSearchDTO searchDTO)
        {
            try
            {

                var imagePathList = searchDTO.ImagePaths; // Danh sách imagePath từ Weaviate response

                var query = (from product in _productRepository.GetQueryable()
                             join productImage in _productImageRepository.GetQueryable()
                             on product.Id equals productImage.ProductId
                             where product.IsDelete != true && imagePathList.Contains(productImage.Path)
                             select product)
                             .Distinct(); // Lấy các sản phẩm duy nhất

                // Chuyển đổi sang ProductDTO
                var productDTOQuery = query.Select(product => new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name ?? string.Empty,
                    Code = product.Code ?? string.Empty,
                    Description = product.Description ?? string.Empty,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    CreatedDate = product.CreatedDate,
                });
                //searchDTO.PageSize = 30;
                //searchDTO.PageIndex = 1;
                var result = PagedList<ProductDTO>.Create(productDTOQuery, searchDTO);
                // Lấy danh sách ProductId
                var productIds = result.Items.Select(p => p.Id).ToList();
                // Truy vấn ảnh thumbnail (chỉ lấy những ảnh có IsThumbnail = true)
                // Truy vấn ảnh thumbnail (chỉ lấy ảnh đầu tiên có IsThumbnail = true cho mỗi sản phẩm)
                var thumbnailImages = _productImageRepository.GetQueryable()
                    .Where(pi => productIds.Contains(pi.ProductId) && pi.IsThumbnail)
                    .GroupBy(pi => pi.ProductId)
                    .Select(g => g.FirstOrDefault()) // Lấy ảnh thumbnail đầu tiên trong mỗi nhóm
                    .ToList();
                // Gán ảnh thumbnail vào ProductDTO
                foreach (var productDTO in result.Items)
                {
                    var thumbnailImage = thumbnailImages.FirstOrDefault(pi => pi.ProductId == productDTO.Id);
                    if (thumbnailImage == null) continue;
                    productDTO.Thumbnail = thumbnailImage.Path;
                    productDTO.ThumbnailBase64 = ImageToBase64(thumbnailImage.Path);
                    productDTO.ThumbnailContentType = GetContentTypeFromExtension(Path.GetExtension(thumbnailImage.Path).ToLowerInvariant());
                }
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

        public async Task<ResponseWithDataDto<PagedList<ProductDTO>>> GetRelativeProductById(Guid id)
        {
            var search = new ProductSearchDTO() {PageIndex = 1, PageSize = 20 };
            var product_Categories = await _productCategoryRepository.FindByAsync(x => x.ProductId == id);
            var categoryIds =  product_Categories.Select(x => x.CategoryId).ToArray();
            search.Category_Filter = categoryIds;
            var result =await GetDataByPage(search);
            var productToRemove = result.Data.Items.FirstOrDefault(p => p.Id == id);
            if (productToRemove != null)
            {
                result.Data.Items.Remove(productToRemove);
            }
            return result;
        }
    }
}
