using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.CartDTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.DTOs.OrderDTO;
using MinhTuan.Domain.DTOs.ProductDTO;
using MinhTuan.Domain.DTOs.StatisticDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Service.Core.Services;
using MinhTuan.Service.SearchDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.OrderService
{
    public interface IOrderService :IService<Order>
    {
        ResponseWithDataDto<PagedList<OrderDTO>> GetDataByPage(OrderSearchDTO searchDTO);
        Task<ResponseWithDataDto<List<OrderItemDTO>>> GetOrderItemsById(Guid id);

        Task<string> AutoGenOrderCode();
        int CountNumberOfOrderToday();
        int CountNumberOfOrderYesterday();
        int CountNumberOfOrderWaitingConfirm(Guid statusId);
        double GetRevenueToday();
        List<RevenueDTO> GetRevenueOfMonth();
        List<RevenueDTO> GetRevenueDayToDay(DateTime startDay, DateTime endDay);
        List<RevenueCategoryDTO> GetRevenueCategories(List<Guid> categoryIdList);
        List<RevenueCategoryDTO> GetRevenueCategoriesDayToDay(List<Guid> categoryIdList,DateTime startDay, DateTime endDay);
        List<ProductTopSellingDTO> GetProductTopSellingOfWeek();
        List<ProductTopSellingDTO> GetProductTopSellingDayToDay(DateTime startDay, DateTime endDay);
    }
}
