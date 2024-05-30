using MinhTuan.Domain.Helper.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.SearchDTO
{
    public class ProductSearchDTO :SearchBase
    {
        public string? Code_Filter { get; set; }
        public string? Name_Filter { get; set; }
        public float? Price_Filter { get; set; }
        public int? StockQuantity_Filter { get; set; }
    }
}
