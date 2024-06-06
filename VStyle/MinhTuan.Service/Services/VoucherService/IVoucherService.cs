using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.DTOs.VoucherDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Service.Core.Services;
using MinhTuan.Service.SearchDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.VoucherService
{
    public interface IVoucherService :IService<Voucher>
    {
        ResponseWithDataDto<PagedList<VoucherDTO>> GetDataByPage(VoucherSearchDTO searchDTO);
        bool CheckExitCode(string code, Guid id);
    }
}
