using MinhTuan.Domain.Helper.EmailSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Core.Services
{
    public interface IEmailService
    {
        bool SendEmail(Message message);
        bool SendForgotPasswordEmail(Message message);
    }
}
