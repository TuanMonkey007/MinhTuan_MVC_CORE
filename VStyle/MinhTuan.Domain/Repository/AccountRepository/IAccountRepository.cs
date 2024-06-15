using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.DTOs.AccountDTO;
using MinhTuan.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Repository.AccountRepository;

public interface IAccountRepository: IRepository<AppUser>
{
    public Task<string> LogInAsync(LogInDTO model);
    public Task<IdentityResult> RegisterAsync(RegisterDTO model);
}
