using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.PaymentInfoDTO
{
    public class PaymentInfoDTO
    {
        public  Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public double TotalAmount { get; set; }
        public Guid PaymentMethodId { get; set; }
        public Guid PaymentStatusId { get; set; }
        public string? TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? AdditionalData { get; set; }
        public string? VnpBankCode { get; set; }
        public string? VnpBankTranNo { get; set; }
        public string? VnpCardType { get; set; }
        public DateTime? VnpPayDate { get; set; }
        public string? VnpResponseCode { get; set; }

        ///////
        public  string?  OrderCode { get; set; }
        public string? PaymentMethodName { get; set; }
        public string? PaymentStatusName { get; set; }
        public  string? CustomerName { get; set; }
    }
}
