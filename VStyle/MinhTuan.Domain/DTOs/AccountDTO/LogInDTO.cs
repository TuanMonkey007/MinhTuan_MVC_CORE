using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.AccountDTO;

public class LogInDTO
{
    public string PhoneOrEmail { get; set; }
    public string  Password { get; set; }
    public bool RememberMe { get; set; }
}
