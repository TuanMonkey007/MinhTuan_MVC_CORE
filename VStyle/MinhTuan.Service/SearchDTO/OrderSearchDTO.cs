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
    }
}
