using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.DTOs.PaymentInfoDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Domain.Repository.PaymentInfoRepository;
using MinhTuan.Service.Core;
using MinhTuan.Service.SearchDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using MinhTuan.Domain.Repository.OrderRepository;

namespace MinhTuan.Service.Services.PaymentInfoService
{
    public class PaymentInfoService : Service<PaymentInfo>, IPaymentInfoService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentInfoRepository _paymentInfoRepository;
        public PaymentInfoService(IUnitOfWork unitOfWork, IPaymentInfoRepository paymentInfoRepository,
            IOrderRepository orderRepository
            ) : base(unitOfWork)
        {
            _paymentInfoRepository = paymentInfoRepository;
            _orderRepository = orderRepository;
        }

        public async Task<ResponseWithDataDto<PagedList<PaymentInfoDTO>>> GetDataByPage(PaymentInfoSearchDTO searchDTO)
        {
            try
            {
                var query = from entityTbl in _paymentInfoRepository.GetQueryable()
                            join order in _orderRepository.GetQueryable() on entityTbl.OrderId equals order.Id
                            where entityTbl.IsDelete != true //Xét xóa mềm
                            select new PaymentInfoDTO
                            {
                                Id = entityTbl.Id,
                                OrderId = entityTbl.OrderId,
                                TotalAmount = entityTbl.TotalAmount,
                                PaymentMethodId = entityTbl.PaymentMethodId,
                                PaymentStatusId = entityTbl.PaymentStatusId,
                                PaymentDate = entityTbl.UpdatedDate != null ? entityTbl.UpdatedDate : entityTbl.CreatedDate,
                                TransactionId = entityTbl.TransactionId,
                                AdditionalData = entityTbl.AdditionalData,
                                VnpBankCode = entityTbl.VnpBankCode,
                                VnpBankTranNo =  entityTbl.VnpBankTranNo,
                                VnpCardType = entityTbl.VnpCardType,
                                VnpPayDate = entityTbl.VnpPayDate,
                                VnpResponseCode = entityTbl.VnpResponseCode,
                                OrderCode = order.Code,
                                CustomerName = order.CustomerName


                            };


                if (searchDTO != null)
                {
                    if (searchDTO.StartDate_Filter.HasValue)
                    {
                        query = query.Where(x => x.PaymentDate.Value.Date >= searchDTO.StartDate_Filter.Value.Date);
                    }
                    if (searchDTO.EndDate_Filter.HasValue)
                    {
                        query = query.Where(x => x.PaymentDate.Value.Date <= searchDTO.EndDate_Filter.Value.Date);
                    }

                    if (!string.IsNullOrEmpty(searchDTO.sortQuery))
                    {
                        query = query.OrderBy(searchDTO.sortQuery);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.PaymentDate);
                    }
                }
                var result = PagedList<PaymentInfoDTO>.Create(query, searchDTO);
                return new ResponseWithDataDto<PagedList<PaymentInfoDTO>>()
                {
                    Data = result,

                    Message = "Lấy thành công"
                };

            }
            catch (Exception ex)
            {
                return new ResponseWithDataDto<PagedList<PaymentInfoDTO>>()
                {
                    Data = null,

                    Message = ex.Message

                };
            }
        }
    }
}
