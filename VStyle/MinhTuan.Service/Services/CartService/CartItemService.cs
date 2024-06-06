using AutoMapper;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.DTOs.CartDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Repository.CartItemRepository;
using MinhTuan.Domain.Repository.DataCategoryRepository;
using MinhTuan.Domain.Repository.ProductRepository;
using MinhTuan.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.CartService
{
    public class CartItemService : Service<CartItem>, ICartItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IDataCategoryRepository _dataCategoryRepository;

        public CartItemService(IUnitOfWork unitOfWork, IMapper mapper, ICartItemRepository cartItemRepository,
            IProductVariantRepository productVariantRepository,
            IProductRepository productRepository,
            IProductImageRepository productImageRepository,
            IDataCategoryRepository dataCategoryRepository
            ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cartItemRepository = cartItemRepository;
            _productVariantRepository = productVariantRepository;
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _dataCategoryRepository = dataCategoryRepository;
        }

        public async Task<ResponseWithDataDto<List<CartItemDTO>>> GetAllCartItemByCartId(Guid id)
        {
            var response = new ResponseWithDataDto<List<CartItemDTO>>() { Message = "Lấy danh sách thành công"};
           var result = await _cartItemRepository.FindByAsync(x => x.CartId.Equals(id) && x.IsDelete != true);

            //Lấy ra ảnh thumbnail
            response.Data = _mapper.Map<List<CartItemDTO>>(result);
            foreach(var item in response.Data)
            {
                var productVariant = _productVariantRepository.GetById(item.ProductVariantId);
                item.ProductPrice = productVariant.Price;
                item.ColorName = _dataCategoryRepository.GetById(productVariant.ColorId).Name;
                item.SizeName = _dataCategoryRepository.GetById(productVariant.SizeId).Name;
                item.ProductName = _productRepository.GetById(productVariant.ProductId).Name;
                var productImage = _productImageRepository.FindBy(x => x.ProductId.Equals(productVariant.ProductId) && x.IsDelete != true && x.IsThumbnail ==true).FirstOrDefault();
                //chuyển lấy ảnh base64
                item.ThumbnailPath = productImage.Path;
                if(item.ThumbnailPath != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Product", item.ThumbnailPath);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(path);
                    item.ThumbnailBase64 = Convert.ToBase64String(imageBytes);
                    var fileExtension = Path.GetExtension(Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Product", item.ThumbnailPath)).ToLowerInvariant();
                    item.ThumbnailContentType = fileExtension switch
                    {
                        ".jpg" or ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        _ => "application/octet-stream",
                    };
                }
               




            }
            return response;
        }
    }
}
