using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.BannerDTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Service.Core.Services;
using MinhTuan.Service.SearchDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.BannerService
{
    public interface IBannerService :IService<Banner>
    {
        ResponseWithDataDto<PagedList<BannerDTO>> GetDataByPage(BannerSearchDTO searchDTO);

    }
}
