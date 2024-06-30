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
using System.Linq.Dynamic.Core;
using MinhTuan.Domain.DTOs.StatisticDTO;
using MinhTuan.Domain.DTOs.ProductDTO;
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
        private readonly IProductCategoryRepository _productCategoryRepository;

        public OrderService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IProductVariantRepository productVariantRepository,
            IProductRepository productRepository,
            IProductImageRepository productImageRepository,
            IDataCategoryRepository dataCategoryRepository,
            IProductCategoryRepository productCategoryRepository
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
            _productCategoryRepository  = productCategoryRepository;
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
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
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

        public int CountNumberOfOrderToday()
        {
            var today = DateTime.Now.Date;
            var result = _orderRepository.FindBy(x => x.CreatedDate.Value.Date.Equals(today) && x.IsDelete != true).Count();
            return result;
        }
        public int CountNumberOfOrderYesterday()
        {
            var today = DateTime.Now.Date - TimeSpan.FromDays(1);
            var result = _orderRepository.FindBy(x => x.CreatedDate.Value.Date.Equals(today) && x.IsDelete != true).Count();
            return result;
        }
        public  int CountNumberOfOrderWaitingConfirm(Guid statusId)
        {
    
            
            var result = _orderRepository.FindBy(x => x.Status.Equals(statusId) && x.IsDelete != true && x.IsCancelled != true).Count();
            return result;
        }
        public double GetRevenueToday()
        {
            //Doanh thu tính theo đơn chưa bị hủy
            var today = DateTime.Now.Date;
            var result = _orderRepository.FindBy(x => x.CreatedDate.Value.Date.Equals(today) && x.IsCancelled != true && x.IsDelete != true).Sum(x => x.TotalAmount);
            return result;
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
                                CartId = entityTbl.CartId != null ? entityTbl.CartId : Guid.Empty,
                                CreatedDate = entityTbl.CreatedDate,
                                IsCancelled = entityTbl.IsCancelled,
                                ReasonCancelled = entityTbl.ReasonCancelled,
                            };
                if (searchDTO != null)
                {


                    if (searchDTO.CreatedTime_Filter.HasValue)
                    {
                        query = query.Where(x => x.CreatedDate.Value.Date == searchDTO.CreatedTime_Filter.Value.Date);

                    }
                    if(searchDTO.NameCustomer_Filter != null)
                    {
                        var idSearch = searchDTO.NameCustomer_Filter.ToString().RemoveAccentsUnicode();
                        var list = _orderRepository.GetQueryable().Select(x => x.CustomerName).ToList().Where(x => x.ToString().ToLower().RemoveAccentsUnicode().Contains(idSearch.ToLower()));
                        query = query.Where(x => list.Contains(x.CustomerName));
                       // query = query.Where(x => x.CustomerName.Contains(searchDTO.NameCustomer_Filter));
                    }
                    if (searchDTO.OrderCode_Filter != null)
                    {
                        var idSearch = searchDTO.OrderCode_Filter.ToString().RemoveAccentsUnicode();
                        var list = _orderRepository.GetQueryable().Select(x => x.Code).ToList().Where(x => x.ToString().ToLower().RemoveAccentsUnicode().Contains(idSearch.ToLower()));
                        query = query.Where(x => list.Contains(x.Code));
                        // query = query.Where(x => x.CustomerName.Contains(searchDTO.NameCustomer_Filter));
                    }
                    if (searchDTO.StartTime_Filter.HasValue)
                    {
                        query = query.Where(x => x.CreatedDate.Value.Date >= searchDTO.StartTime_Filter.Value.Date);
                    }
                    if (searchDTO.EndTime_Filter.HasValue)
                    {
                        query = query.Where(x => x.CreatedDate.Value.Date <= searchDTO.EndTime_Filter.Value.Date);
                    }
                }
                if (!string.IsNullOrEmpty(searchDTO.sortQuery))
                {
                    query = query.OrderBy(searchDTO.sortQuery);
                }
                else
                {
                    query = query.OrderByDescending(x => x.CreatedDate);
                }
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

        public List<RevenueDTO> GetRevenueOfMonth()
        {
            try
            {
                var query = from entityTbl in _orderRepository.GetQueryable()
                            where entityTbl.IsDelete != true && entityTbl.IsCancelled != true
                            group entityTbl by entityTbl.CreatedDate.Value.Date into g // Nhóm theo ngày
                            select new RevenueDTO
                            {
                                Label_Day = g.Key, // Ngày đã được nhóm
                                Revenue = g.Sum(x => x.TotalAmount), // Tổng doanh thu của ngày đó
                                CountOrder = g.Count() // Số lượng đơn hàng trong ngày đó
                            };

                return query.ToList();
            }
            catch
            {
                throw;
            }
        }
        public  List<RevenueCategoryDTO> GetRevenueCategories(List<Guid> categoryIdList)
        {
            try
            {
                var query = from order in _orderRepository.GetQueryable()
                            join orderItem in _orderItemRepository.GetQueryable() on order.Id equals orderItem.OrderId
                            join productVariant in _productVariantRepository.GetQueryable() on orderItem.ProductVariantId equals productVariant.Id
                            join product in _productRepository.GetQueryable() on productVariant.ProductId equals product.Id
                            join productCategory in _productCategoryRepository.GetQueryable() on product.Id equals productCategory.ProductId
                            where order.IsDelete != true && order.IsCancelled != true
                                  && (productCategory.CategoryId.Equals(categoryIdList[0]) || productCategory.CategoryId.Equals(categoryIdList[1])) // Khoảng tìm kiếm theo giờ tính
                            group new { order, productCategory } by productCategory.CategoryId into g
                            select new RevenueCategoryDTO
                            {
                                CategoryId = g.Key, // Giới tính (NAM hoặc NU)
                                Revenue = g.Sum(x => x.order.TotalAmount) // Tổng doanh thu của giới tính đó
                            };

                // Chuyển đổi query thành List
                var revenueList = query.ToList();

                foreach (var item in revenueList)
                {
                    if (item.CategoryId.Equals(categoryIdList[0]))
                    {
                        item.CategoryName = "Đồ nam";
                    }
                    else
                    {
                        item.CategoryName = "Đồ nữ";
                    }
                }

                return revenueList;
            }
            catch
            {
                throw;
            }
        }

        public List<ProductTopSellingDTO> GetProductTopSellingOfWeek()
        {
            try
            {
                var endDate = DateTime.Now; // Kết thúc tuần hiện tại
                var startDate = endDate.AddDays(-365); // Bắt đầu tuần hiện tại (7 ngày trước)
                var query = (from order in _orderRepository.GetQueryable()
                             join orderItem in _orderItemRepository.GetQueryable() on order.Id equals orderItem.OrderId
                             join productVariant in _productVariantRepository.GetQueryable() on orderItem.ProductVariantId equals productVariant.Id
                             join product in _productRepository.GetQueryable() on productVariant.ProductId equals product.Id
                             where order.CreatedDate.Value.Date >= startDate.Date && order.CreatedDate.Value.Date <= endDate.Date && order.IsDelete != true && order.IsCancelled != true
                             group orderItem by product into productGroup
                             orderby productGroup.Sum(oi => oi.Quantity) descending
                             select new ProductTopSellingDTO
                             {
                                 ProductId = productGroup.Key.Id,
                                 ProductName = productGroup.Key.Name,
                                 ProductPrice = productGroup.Select(oi => oi.Price).FirstOrDefault(), // Lấy giá của biến thể đầu tiên
                                 TotalQuantitySold = productGroup.Sum(oi => oi.Quantity)
                             }).Take(10);



                return query.ToList();

            }
            catch
            {
                throw;
            }
        }

        public List<RevenueDTO> GetRevenueDayToDay(DateTime startDay, DateTime endDay)
        {
            try
            {
                var query = from entityTbl in _orderRepository.GetQueryable()
                            where entityTbl.IsDelete != true && entityTbl.IsCancelled != true
                                   && entityTbl.CreatedDate.Value.Date >= startDay.Date
                  && entityTbl.CreatedDate.Value.Date <= endDay.Date
                            group entityTbl by entityTbl.CreatedDate.Value.Date into g // Nhóm theo ngày
                            select new RevenueDTO
                            {
                                Label_Day = g.Key, // Ngày đã được nhóm
                                Revenue = g.Sum(x => x.TotalAmount), // Tổng doanh thu của ngày đó
                                CountOrder = g.Count() // Số lượng đơn hàng trong ngày đó
                            };

                return query.ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<RevenueCategoryDTO> GetRevenueCategoriesDayToDay(List<Guid> categoryIdList, DateTime startDay, DateTime endDay)
        {
            try
            {
                var query = from order in _orderRepository.GetQueryable()
                            join orderItem in _orderItemRepository.GetQueryable() on order.Id equals orderItem.OrderId
                            join productVariant in _productVariantRepository.GetQueryable() on orderItem.ProductVariantId equals productVariant.Id
                            join product in _productRepository.GetQueryable() on productVariant.ProductId equals product.Id
                            join productCategory in _productCategoryRepository.GetQueryable() on product.Id equals productCategory.ProductId
                            where order.IsDelete != true 
                                 && order.IsCancelled != true
                                 && order.CreatedDate.Value.Date >= startDay.Date
                                 && order.CreatedDate.Value.Date <= endDay.Date
                                  && (productCategory.CategoryId.Equals(categoryIdList[0]) || productCategory.CategoryId.Equals(categoryIdList[1])) // Khoảng tìm kiếm theo giờ tính
                            group new { order, productCategory } by productCategory.CategoryId into g
                            select new RevenueCategoryDTO
                            {
                                CategoryId = g.Key, // Giới tính (NAM hoặc NU)
                                Revenue = g.Sum(x => x.order.TotalAmount) // Tổng doanh thu của giới tính đó
                            };

                // Chuyển đổi query thành List
                var revenueList = query.ToList();

                foreach (var item in revenueList)
                {
                    if (item.CategoryId.Equals(categoryIdList[0]))
                    {
                        item.CategoryName = "Đồ nam";
                    }
                    else
                    {
                        item.CategoryName = "Đồ nữ";
                    }
                }

                return revenueList;
            }
            catch
            {
                throw;
            }
        }

        public List<ProductTopSellingDTO> GetProductTopSellingDayToDay(DateTime startDay, DateTime endDay)
        {

            try
            {
         
                var query = (from order in _orderRepository.GetQueryable()
                             join orderItem in _orderItemRepository.GetQueryable() on order.Id equals orderItem.OrderId
                             join productVariant in _productVariantRepository.GetQueryable() on orderItem.ProductVariantId equals productVariant.Id
                             join product in _productRepository.GetQueryable() on productVariant.ProductId equals product.Id
                             where order.CreatedDate.Value.Date >= startDay && order.CreatedDate.Value.Date <= endDay && order.IsDelete != true && order.IsCancelled != true
                             group orderItem by product into productGroup
                             orderby productGroup.Sum(oi => oi.Quantity) descending
                             select new ProductTopSellingDTO
                             {
                                 ProductId = productGroup.Key.Id,
                                 ProductName = productGroup.Key.Name,
                                 ProductPrice = productGroup.Select(oi => oi.Price).FirstOrDefault(), // Lấy giá của biến thể đầu tiên
                                 TotalQuantitySold = productGroup.Sum(oi => oi.Quantity)
                             }).Take(10);



                return query.ToList();

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateCancelProductVariant(Guid id, int quantity)
        {
            var productVariant = await _productVariantRepository.GetByIdAsync(id);
            productVariant.StockQuantity += quantity;
            _productVariantRepository.Update(productVariant);
          //  await _unitOfWork.SaveChangesAsync();
            return true;

        }
    }
}
