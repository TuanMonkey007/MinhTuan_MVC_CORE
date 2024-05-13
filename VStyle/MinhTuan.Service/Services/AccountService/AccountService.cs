﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MinhTuan.Domain.DTOs.AccountDTO;
using MinhTuan.Domain.Entities;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using MinhTuan.Domain.Helper.Constants;

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

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
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
            expires: DateTime.Now.AddHours(4), //Hết hạn xác thực jwt trong 4 giờ
            claims: authClaims,
            signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)

            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<IdentityResult> RegisterAsync(RegisterDTO model)
    {
        var user = _mapper.Map<AppUser>(model);
        user.AccountType = AccountType.NORMAL;
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            if (!await _roleManager.RoleExistsAsync(AppRole.CUSTOMER))
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

    public async Task<string> HandleGoogleLoginResponse() // Hàm này sẽ trả về một token xác thực
    {
        var result = await _httpContextAccessor.HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (result?.Principal != null && result.Principal.Identities.FirstOrDefault() != null)
        {
            var emailClaim = result.Principal.FindFirst(ClaimTypes.Email);
            if (emailClaim != null)
            {
                var email = emailClaim.Value;

                // Kiểm tra xem email đã tồn tại trong cơ sở dữ liệu hay không
                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser != null)
                {
                    // Nếu tài khoản đã tồn tại, đăng nhập người dùng bằng tài khoản Identity
                    var signInTask = _signInManager.SignInAsync(existingUser, isPersistent: true);
                    await signInTask;
                    var roleUser = _userManager.GetRolesAsync(existingUser);
                    if (signInTask.IsCompletedSuccessfully)
                    {
                        var authClaims = new List<Claim>
                         {
                            new Claim(ClaimTypes.Name, string.IsNullOrEmpty(result.Principal.FindFirst(ClaimTypes.Name).Value)?"Unknown":result.Principal.FindFirst(ClaimTypes.Name).Value ), // Thêm tên người dùng vào claim
                            new Claim(ClaimTypes.Email, email ),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.Role, roleUser.Result.First())
                         };

                        var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                        var token = new JwtSecurityToken(
                            issuer: _configuration["JWT:ValidIssuer"],
                            audience: _configuration["JWT:ValidAudience"],
                            expires: DateTime.Now.AddHours(4), //Hết hạn xác thực jwt trong 4 giờ
                            claims: authClaims,
                            signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)

                            );
                        return new JwtSecurityTokenHandler().WriteToken(token);
                    }
                    else
                    {
                        // Đăng nhập không thành công, xử lý lỗi nếu cần
                        return string.Empty;
                    }
                }
                else
                {
                    RegisterDTO model = new RegisterDTO
                    {
                        FullName = string.IsNullOrEmpty(result?.Principal.FindFirst(ClaimTypes.Name).Value) ? "Unknown" : result?.Principal.FindFirst(ClaimTypes.Name).Value,
                        Email = email,
                    };
                    var newUser = _mapper.Map<AppUser>(model);
                    newUser.AccountType = AccountType.GOOGLE;
                    newUser.UserName = email;
                    var resultCreate = await _userManager.CreateAsync(newUser);
                    if (resultCreate.Succeeded)
                    {
                        if (!await _roleManager.RoleExistsAsync(AppRole.CUSTOMER))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(AppRole.CUSTOMER));
                        }
                        await _userManager.AddToRoleAsync(newUser, AppRole.CUSTOMER); //Role default when register = Customer
                    }
                    var newUserCreated = await _userManager.FindByEmailAsync(email);
                    var signInTask = _signInManager.SignInAsync(newUserCreated, isPersistent: false);
                    await signInTask;
                    var roleUser = _userManager.GetRolesAsync(newUserCreated);
                    if (signInTask.IsCompletedSuccessfully)
                    {
                        var authClaims = new List<Claim>
                         {
                            new Claim(ClaimTypes.Name, string.IsNullOrEmpty(result.Principal.FindFirst(ClaimTypes.Name).Value)?"Unknown":result.Principal.FindFirst(ClaimTypes.Name).Value ), // Thêm tên người dùng vào claim
                            new Claim(ClaimTypes.Email, email ),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.Role, roleUser.Result.First())
                         };

                        var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                        var token = new JwtSecurityToken(
                            issuer: _configuration["JWT:ValidIssuer"],
                            audience: _configuration["JWT:ValidAudience"],
                            expires: DateTime.Now.AddHours(4), //Hết hạn xác thực jwt trong 4 giờ
                            claims: authClaims,
                            signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)

                            );
                        return new JwtSecurityTokenHandler().WriteToken(token);
                    }
                    else
                    {
                        // Đăng nhập không thành công, xử lý lỗi nếu cần
                        return string.Empty;
                    }
                    // Đăng ký thành công thì chuyển sang tạo token đăng nhập
                }
            }
            else
            {
                // Không có claim về email
                return string.Empty;
            }
        }
        else
        {
            // Xử lý trường hợp không có claims hoặc Principal
            return string.Empty;
        }
    }

    public async Task<string> HandleFacebookLoginResponse()
    {
        var result = await _httpContextAccessor.HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
     
        if (result?.Principal != null && result.Principal.Identities.FirstOrDefault() != null)//Đã xác thực
        {
            //Kiểm tra xem có user type Facebook nào có mã như kia chưa nameidentifier chưa
            //Có rồi thì đăng nhập bằng tài khoản đó
            //Chưa có thì tạo rồi đăng nhập
            var nameIdentifier = result.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            var existingUser = await _userManager.FindByNameAsync(nameIdentifier);//Tìm kiếm bằng username
            if(existingUser != null)//Nếu đã tồn tại thì đăng nhập bằng tài khoản đó
            {
                // Nếu tài khoản đã tồn tại, đăng nhập người dùng bằng tài khoản Identity
                var signInTask = _signInManager.SignInAsync(existingUser, isPersistent: true);
                await signInTask;

                if (signInTask.IsCompletedSuccessfully)
                {
                    var authClaims = new List<Claim>
                         {
                            new Claim(ClaimTypes.Name, string.IsNullOrEmpty(result.Principal.FindFirst(ClaimTypes.Name).Value)?"Unknown":result.Principal.FindFirst(ClaimTypes.Name).Value ), // Thêm tên người dùng vào claim
                         //new Claim(ClaimTypes.Email, string.IsNullOrEmpty(result.Principal.FindFirst(ClaimTypes.Email).Value)?"Unknown":result.Principal.FindFirst(ClaimTypes.Email).Value ), 
                         
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.Role, AppRole.CUSTOMER)
                         };

                    var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(4), //Hết hạn xác thực jwt trong 4 giờ
                        claims: authClaims,
                        signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)

                        );
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
                else
                {
                    // Đăng nhập không thành công, xử lý lỗi nếu cần
                    return string.Empty;
                }
            }
            else //chưa thì đăng ký thì đăng ký rồi đăng nhập => trả về token
            {
                RegisterDTO model = new RegisterDTO
                {
                    FullName = string.IsNullOrEmpty(result?.Principal.FindFirst(ClaimTypes.Name).Value) ? "Unknown" : result?.Principal.FindFirst(ClaimTypes.Name).Value,
                   
                };
                var newUser = _mapper.Map<AppUser>(model);
                newUser.AccountType = AccountType.FACEBOOK;
                newUser.UserName = nameIdentifier;
                var resultCreate = await _userManager.CreateAsync(newUser);
                if (resultCreate.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(AppRole.CUSTOMER))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(AppRole.CUSTOMER));
                    }
                    await _userManager.AddToRoleAsync(newUser, AppRole.CUSTOMER); //Role default when register = Customer
                }
                var newUserCreated = await _userManager.FindByNameAsync(nameIdentifier);
                var signInTask = _signInManager.SignInAsync(newUserCreated, isPersistent: false);
                await signInTask;

                if (signInTask.IsCompletedSuccessfully)
                {
                    var authClaims = new List<Claim>
                         {
                            new Claim(ClaimTypes.Name, string.IsNullOrEmpty(result.Principal.FindFirst(ClaimTypes.Name).Value)?"Unknown":result.Principal.FindFirst(ClaimTypes.Name).Value ), // Thêm tên người dùng vào claim
                         // new Claim(ClaimTypes.Email, string.IsNullOrEmpty(result.Principal.FindFirst(ClaimTypes.Email).Value)?"Unknown":result.Principal.FindFirst(ClaimTypes.Email).Value ), // Thêm tên người dùng vào claim
                          
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.Role, AppRole.CUSTOMER)
                         };

                    var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(4), //Hết hạn xác thực jwt trong 4 giờ
                        claims: authClaims,
                        signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)

                        );
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
                else
                {
                    // Đăng nhập không thành công, xử lý lỗi nếu cần
                    return string.Empty;
                }
            }


        }
        else
        {
            return string.Empty;
        }

           

    }
}