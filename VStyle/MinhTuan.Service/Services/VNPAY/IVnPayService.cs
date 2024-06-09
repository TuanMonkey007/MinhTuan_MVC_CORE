using Microsoft.AspNetCore.Http;
using MinhTuan.Domain.DTOs.VNPayDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.VNPAY
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestDTO model);
        VnPaymentResponseDTO PaymentExecute(IQueryCollection collections);
    }
}
