using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.DTOs.PaymentInfoDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Service.Core.Services;
using MinhTuan.Service.SearchDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.PaymentInfoService
{
    public interface IPaymentInfoService :IService<PaymentInfo>
    {
        Task<ResponseWithDataDto<PagedList<PaymentInfoDTO>>> GetDataByPage(PaymentInfoSearchDTO searchDTO);
    }
}
