using MinhTuan.Domain.Helper.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.SearchDTO
{
    public class AccountSearchDTO:SearchBase
    {
        public string? FullName_Filter { get; set; }
        public string? PhoneNumber_Filter { get; set; }
        public string? Email_Filter { get; set; }
        public string?  Status_Filter { get; set; }
    }
}
