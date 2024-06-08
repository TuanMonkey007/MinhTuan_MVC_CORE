using AutoMapper;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.DTOs.CartDTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.DTOs.OrderDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Domain.Repository.CategoryRepositoy;
using MinhTuan.Domain.Repository.DataCategoryRepository;
using MinhTuan.Domain.Repository.OrderItemRepository;
using MinhTuan.Domain.Repository.OrderRepository;
using MinhTuan.Domain.Repository.ProductRepository;
using MinhTuan.Service.Core;
using MinhTuan.Service.SearchDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.OrderService
{
    public class OrderService : Service<Order>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IDataCategoryRepository _dataCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;

        public OrderService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IProductVariantRepository productVariantRepository,
            IProductRepository productRepository,
            IProductImageRepository productImageRepository,
            IDataCategoryRepository dataCategoryRepository
            ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _productVariantRepository = productVariantRepository;
            _dataCategoryRepository = dataCategoryRepository;
            _productImageRepository = productImageRepository;
        }

        public async Task<string> AutoGenOrderCode()
        {
            // Phần cố định: "VS"
            string prefix = "VS";

            // 9 ký tự số: lấy thời gian hiện tại (tính bằng mili giây từ 1/1/1970)
            string timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString().Substring(3, 9); // Loại bỏ 3 chữ số đầu để đảm bảo đủ 9 ký tự

            // Dấu gạch ngang: "-"
            string separator = "-";

            // 6 ký tự bất kỳ: kết hợp chữ hoa, chữ thường và số
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            while (true)
            {
                string randomString = new string(Enumerable.Repeat(chars, 6)
                                                    .Select(s => s[random.Next(s.Length)])
                                                    .ToArray());

                // Kết hợp lại thành mã đơn hàng hoàn chỉnh
                string newOrderCode = prefix + timestamp + separator + randomString;
                var isExist = await _orderRepository.FindByAsync(x => x.Code.Equals(newOrderCode) && x.IsDelete != true);
                if (isExist.Count() == 0)
                {
                    return newOrderCode;
                }
            }
        }


        public ResponseWithDataDto<PagedList<OrderDTO>> GetDataByPage(OrderSearchDTO searchDTO)
        {
            try
            {
                var query = from entityTbl in _orderRepository.GetQueryable()
                            where entityTbl.IsDelete != true //Xét xóa mềm
                            select new OrderDTO
                            {
                                Id = entityTbl.Id,
                                UserId = entityTbl.UserId,
                                Code = entityTbl.Code ?? string.Empty,
                                CustomerName = entityTbl.CustomerName ?? string.Empty,
                                CustomerPhoneNumber = entityTbl.CustomerPhoneNumber ?? string.Empty,
                                ShippingAddress = entityTbl.ShippingAddress ?? string.Empty,
                                CustomerNote = entityTbl.CustomerNote ?? string.Empty,
                                ShopNote = entityTbl.ShopNote ?? string.Empty,
                                PaymentMethod = entityTbl.PaymentMethod != null ? entityTbl.PaymentMethod: Guid.Empty,
                                Status = entityTbl.Status != null ? entityTbl.Status:Guid.Empty,
                                TotalAmount = entityTbl.TotalAmount,
                                ShippingCost = entityTbl.ShippingCost,
                                VoucherId = entityTbl.VoucherId,
                                CartId = entityTbl.CartId,
                                CreatedDate = entityTbl.CreatedDate,
                                IsCancelled = entityTbl.IsCancelled,
                                ReasonCancelled = entityTbl.ReasonCancelled
                            };

                var result = PagedList<OrderDTO>.Create(query, searchDTO);
                return new ResponseWithDataDto<PagedList<OrderDTO>>()
                {
                    Data = result,

                    Message = "Lấy thành công"
                };

            }
            catch (Exception ex)
            {
                return new ResponseWithDataDto<PagedList<OrderDTO>>()
                {
                    Data = null,

                    Message = ex.Message

                };
            }
        }

        public async Task<ResponseWithDataDto<List<OrderItemDTO>>> GetOrderItemsById(Guid id)
        {
            var response = new ResponseWithDataDto<List<OrderItemDTO>>() { Message = "Lấy danh sách thành công" };
            var result = await _orderItemRepository.FindByAsync(x => x.OrderId.Equals(id) && x.IsDelete != true);

            //Lấy ra ảnh thumbnail
            response.Data = _mapper.Map<List<OrderItemDTO>>(result);
            foreach (var item in response.Data)
            {
                var productVariant = _productVariantRepository.GetById(item.ProductVariantId);
                item.ColorName = _dataCategoryRepository.GetById(productVariant.ColorId).Name;
                item.SizeName = _dataCategoryRepository.GetById(productVariant.SizeId).Name;
                item.ProductName = _productRepository.GetById(productVariant.ProductId).Name;
                item.ProductCode = _productRepository.GetById(productVariant.ProductId).Code;
                var productImage = _productImageRepository.FindBy(x => x.ProductId.Equals(productVariant.ProductId) && x.IsDelete != true && x.IsThumbnail == true).FirstOrDefault();
                //chuyển lấy ảnh base64
                item.ThumbnailPath = productImage.Path;
                if (item.ThumbnailPath != null)
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
