using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.VoucherDTO
{
    public class VoucherDTO
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MinimumPurchaseAmount { get; set; } // Số tiền tối thiểu để áp dụng

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount { get; set; } // Số tiền được giảm

        [Range(0, 100)]
        public int DiscountPercent { get; set; } // Phần trăm giảm giá (nếu có)

        [Range(0, double.MaxValue)]
        public decimal? MaxValue { get; set; } // Giá trị giảm tối đa (nếu có)

        public int Type { get; set; } // Enum cho loại voucher : shippefee / orderfee

        public DateTime TimeStart { get; set; } // Thời gian bắt đầu

        public DateTime TimeEnd { get; set; } // Thời gian kết thúc

        public int Quantity { get; set; } //Số lượng mã giảm giá
    }
}
