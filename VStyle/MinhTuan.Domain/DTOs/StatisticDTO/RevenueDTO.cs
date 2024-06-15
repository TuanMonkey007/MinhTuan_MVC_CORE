using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.StatisticDTO
{
    public class RevenueDTO
    {
        public DateTime Label_Day { get; set; }
        public double  Revenue { get; set; }
        public int CountOrder { get; set; }
    }
}
