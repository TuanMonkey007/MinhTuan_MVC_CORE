using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MinhTuan.Domain.Helper;
using MinhTuan.Domain.DTOs.AccountDTO;
using MinhTuan.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MinhTuan.Service.Services.AccountService;


public class AccountService : IAccountService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public AccountService(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, IConfiguration configuration,
        RoleManager<IdentityRole> roleManager,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _mapper = mapper;
		_roleManager = roleManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> LogInAsync(LogInDTO model)
    {
        // Kiểm tra xem model.PhoneOrEmail là số điện thoại hay email
        var isPhoneNumber = model.PhoneOrEmail.Contains("@") ? false : true;

        // Tìm người dùng theo số điện thoại hoặc email
        var user = isPhoneNumber ? await _userManager.FindByNameAsync(model.PhoneOrEmail) : await _userManager.FindByEmailAsync(model.PhoneOrEmail);

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe,false);
        if (!result.Succeeded)
        {
            return string.Empty;
        }
        
            var authClaims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, user.FullName), // Thêm tên người dùng vào claim
            new Claim(ClaimTypes.Email, string.IsNullOrEmpty(user.Email) ? "Unknown": user.Email),
             new Claim(ClaimTypes.MobilePhone, string.IsNullOrEmpty(user.PhoneNumber) ? "Unknown": user.PhoneNumber),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            };
      
      
		var userRoles = await _userManager.GetRolesAsync(user);
		foreach (var role in userRoles)
		{
            authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
		}
		var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(5), //Hết hạn xác thực jwt trong 5 phút
            claims: authClaims,
            signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)

            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<IdentityResult> RegisterAsync(RegisterDTO model)
    {
        var user = _mapper.Map<AppUser>(model);
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            if(!await _roleManager.RoleExistsAsync(AppRole.CUSTOMER))
            {
                await _roleManager.CreateAsync(new IdentityRole(AppRole.CUSTOMER));

            }
           await _userManager.AddToRoleAsync(user, AppRole.CUSTOMER); //Role default when register = Customer
        }
        return result;
    }
    public async Task LogOutAsync()
    {
        if (!_signInManager.SignOutAsync().IsFaulted)
        {
            await _signInManager.SignOutAsync();
        }
       
    }

}
