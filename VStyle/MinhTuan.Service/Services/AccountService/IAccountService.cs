using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using MinhTuan.Domain.DTOs.AccountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.AccountService;

public interface IAccountService
{
    public Task<List<UserDTO>> GetAllUserAsync();
    public Task<string> LogInAsync(LogInDTO model);
    public Task<IdentityResult> RegisterAsync(RegisterDTO model);
    public Task LogOutAsync();
    public Task<string> HandleGoogleLoginResponse();
    public Task<string> HandleFacebookLoginResponse();
    public Task<string> ForgotPassword(ForgotPasswordDTO model);
    public Task<bool> ResetPassword(ChangePasswordDTO model);

}
