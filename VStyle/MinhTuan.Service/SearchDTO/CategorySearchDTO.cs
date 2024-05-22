using MinhTuan.Domain.Helper.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.SearchDTO
{
    public class CategorySearchDTO:SearchBase
    {
        public string? Code_Filter { get; set; }
        public string? Name_Filter { get; set; }
        public string? Description_Filter { get; set; }
    }
}
