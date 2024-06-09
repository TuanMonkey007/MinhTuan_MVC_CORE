using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Repository.PaymentInfoRepository
{
    public class PaymentInfoRepository : Repository<PaymentInfo>, IPaymentInfoRepository
    {
        public PaymentInfoRepository(VStyleContext context) : base(context)
        {
        }
    }
}
