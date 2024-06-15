using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.StatisticDTO
{
    public class RevenueCategoryDTO
    {
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public double Revenue { get; set; }
    }
}
