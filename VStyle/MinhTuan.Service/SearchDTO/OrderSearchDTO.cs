using MinhTuan.Domain.Helper.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.SearchDTO
{
    public class OrderSearchDTO :SearchBase
    {
        public DateTime?  CreatedTime_Filter { get; set; }
        public DateTime? StartTime_Filter { get; set; }
        public DateTime? EndTime_Filter { get; set; }
        public string? NameCustomer_Filter { get; set; }
        public string? Status_Filter { get; set; }
        public string? OrderCode_Filter { get; set; }
    }
}
