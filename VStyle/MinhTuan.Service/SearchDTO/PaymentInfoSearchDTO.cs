﻿using MinhTuan.Domain.Helper.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.SearchDTO
{
    public class PaymentInfoSearchDTO: SearchBase
    {
        public DateTime? StartDate_Filter { get; set; }
        public DateTime? EndDate_Filter { get; set; }
    }
}
