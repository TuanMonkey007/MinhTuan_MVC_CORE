using Microsoft.AspNetCore.Identity;
using MinhTuan.Domain.DTOs.AccountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.AccountService;

public interface IAccountService
{
    public Task<string> LogInAsync(LogInDTO model);
    public Task<IdentityResult> RegisterAsync(RegisterDTO model);
    public Task LogOutAsync();
}
