using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MinhTuan.Domain.DTOs.AccountDTO;
using MinhTuan.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Repository.AccountRepository;

public class AccountRepository : IAccountRepository
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AccountRepository(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, IConfiguration configuration,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<string> LogInAsync(LogInDTO model)
    {
        var user = _mapper.Map<AppUser>(model);

        var result = await _signInManager.PasswordSignInAsync(user,model.Password,false,false);
        if (!result.Succeeded)
        {
            return string.Empty;
        }
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, model.PhoneOrEmail),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            
        };
        var authenKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        var token = new JwtSecurityToken(
            issuer: _configuration["WT:ValidIsssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(5), //Hết hạn xác thực jwt trong 5 phút
            claims: authClaims,
            signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authenKey,SecurityAlgorithms.HmacSha512Signature)

            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<IdentityResult> RegisterAsync(RegisterDTO model)
    {
        var user = _mapper.Map<AppUser>(model);
        return await _userManager.CreateAsync(user, model.Password);
    }
}