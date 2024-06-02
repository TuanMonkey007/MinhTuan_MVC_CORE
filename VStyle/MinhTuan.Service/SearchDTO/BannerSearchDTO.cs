﻿using MinhTuan.Domain.Helper.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.SearchDTO
{
    public class BannerSearchDTO :SearchBase
    {
        public string?  IsDisplay_Filter { get; set; }
        public Guid[]?  CategoryId_Filter { get; set; }
    }
}
