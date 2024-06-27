using MinhTuan.Domain.Helper.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.SearchDTO
{
    public class ArticleSearchDTO:SearchBase
    {
        public string? Tag { get; set; }
        public Guid? Id_Filter { get; set; }
        public string? Slug_Filter { get; set; }
        public string? Author_Filter { get; set; }
        public string? Title_Filter { get; set; }
        public bool? Status_Filter { get; set; }
      //  public DateTime[]? RangeTime_Filter { get; set; }
        public DateTime? StartDate_Filter { get; set; }
        public DateTime? EndDate_Filter { get; set; }
        public Guid[]? Category_Filter { get; set; }
    }
}
