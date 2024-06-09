using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.Entities;
using MinhTuan.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.PaymentInfoService
{
    public class PaymentInfoService : Service<PaymentInfo>, IPaymentInfoService
    {
        public PaymentInfoService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
