using AutoMapper;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.DTOs.OrderDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Domain.Repository.CategoryRepositoy;
using MinhTuan.Domain.Repository.OrderRepository;
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
        public OrderService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IOrderRepository orderRepository) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderRepository = orderRepository;
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
                                CustomerName = entityTbl.CustomerName,
                                CustomerPhoneNumber = entityTbl.CustomerPhoneNumber,
                                ShippingAddress = entityTbl.ShippingAddress,
                                CustomerNote = entityTbl.CustomerNote,
                                ShopNote = entityTbl.ShopNote,
                                PaymentMethod = entityTbl.PaymentMethod,
                                Status = entityTbl.Status,
                                TotalAmount = entityTbl.TotalAmount,
                                ShippingCost = entityTbl.ShippingCost,
                                VoucherId = entityTbl.VoucherId

                                
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
    }
}
