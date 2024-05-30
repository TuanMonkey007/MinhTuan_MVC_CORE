using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.AccountDTO;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Service.SearchDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.AccountService;

public interface IAccountService
{
    public ResponseWithDataDto<PagedList<UserDTO>> GetDataByPage(AccountSearchDTO search);
    public Task<ResponseWithDataDto<List<string>>> GetRoleByIdAsync(Guid id);
    public Task<string> LogInAsync(LogInDTO model);
    public Task<IdentityResult> RegisterAsync(RegisterDTO model);
    public Task LogOutAsync();
    public Task<string> HandleGoogleLoginResponse();
    public Task<string> HandleFacebookLoginResponse();
    public Task<string> ForgotPassword(ForgotPasswordDTO model);
    public Task<bool> ResetPassword(ChangePasswordDTO model);

}
