using AutoMapper;
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
using MinhTuan.Domain.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using MinhTuan.Service.Core.Services;
using MinhTuan.Domain.Helper.EmailSender;
using Microsoft.AspNetCore.Routing;
using MinhTuan.Domain;
using System.Net;
using MinhTuan.Domain.Helper.Pagination;

using MinhTuan.Domain.Core.DTO;
using MinhTuan.Service.SearchDTO;

using System.Data.Entity;

namespace MinhTuan.Service.Services.AccountService;

public class AccountService : IAccountService
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEmailService _emailService;
    private readonly LinkGenerator _linkGenerator;

    public AccountService(
    
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, IConfiguration configuration,
        RoleManager<IdentityRole> roleManager,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        LinkGenerator linkGenerator
        )
    {
    
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _mapper = mapper;
        _roleManager = roleManager;
        _httpContextAccessor = httpContextAccessor;
        _emailService = emailService;
        _linkGenerator = linkGenerator;
    }
    public ResponseWithDataDto<PagedList<UserDTO>> GetDataByPage(AccountSearchDTO searchDTO)
    {
        try
        {
            var query = from entityTbl in _userManager.Users.AsQueryable()
                        select new UserDTO
                        {
                            Id = entityTbl.Id,
                            FullName = entityTbl.FullName,
                            Email = entityTbl.Email,
                            PhoneNumber = entityTbl.PhoneNumber,
                            LockoutEnd = entityTbl.LockoutEnd,
                           // PhoneNumberConfirmed = entityTbl.PhoneNumberConfirmed,
                            EmailConfirmed = entityTbl.EmailConfirmed,
                            Gender = entityTbl.Gender
                            
                        };


            if (searchDTO != null)
            {
                if (!string.IsNullOrEmpty(searchDTO.FullName_Filter))
                {
                    var idSearch = searchDTO.FullName_Filter.ToString();
                    var isNormal = searchDTO.FullName_Filter.ToString().ToLower() != idSearch.ToLower();
                    var list = _userManager.Users.AsQueryable().Select(x => x.FullName).ToList().Where(x => x.ToString().ToLower().Contains(idSearch.ToLower()));
                    query = query.Where(x => list.Contains(x.FullName));
                }
            }
            var result = PagedList<UserDTO>.Create(query, searchDTO);
            return new ResponseWithDataDto<PagedList<UserDTO>>()
            {
                Data = result,
            
                Message = "Lấy thành công"
            };

        }
        catch (Exception ex)
        {
            return new ResponseWithDataDto<PagedList<UserDTO>>()
            {
                Data = null,
             
                Message = ex.Message

            };
        }
    }

    public async Task<string> LogInAsync(LogInDTO model)
    {
        // Kiểm tra xem model.PhoneOrEmail là số điện thoại hay email
        var isPhoneNumber = model.PhoneOrEmail.Contains("@") ? false : true;

        // Tìm người dùng theo số điện thoại hoặc email
        var user = isPhoneNumber ? await _userManager.FindByNameAsync(model.PhoneOrEmail) : await _userManager.FindByEmailAsync(model.PhoneOrEmail);
        if(user == null) return "Sai thông tin đăng nhập"; // không thấy người dùng
        
       
        var isLockout = await _userManager.IsLockedOutAsync(user);

        if (isLockout)
        {
            DateTimeOffset? lockoutEndDate = await _userManager.GetLockoutEndDateAsync(user);

            // Kiểm tra xem lockoutEndDate có giá trị hay không (null khi không bị khóa)
            if (lockoutEndDate.HasValue)
            {
                // Tính thời gian còn lại đến khi hết khóa
                TimeSpan remainingLockout = lockoutEndDate.Value - DateTimeOffset.Now;

                // Kiểm tra xem thời gian còn lại có lớn hơn 10 phút hay không
                if (remainingLockout.TotalMinutes > 10)
                {
                    return "Tài khoản đã bị khóa bởi quản trị viên";
                }
                else
                {
                    return "Tài khoản đã bị tạm khóa do đăng nhập sai nhiều lần";
                }
            }
        }
        else
        {
            var isCorrectPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isCorrectPassword)
            {
                await _userManager.AccessFailedAsync(user);
                var FailureTime = await _userManager.GetAccessFailedCountAsync(user);
                return "Số lần đăng nhập sai: " + FailureTime + "/5";

            }
            
        }



        await _userManager.ResetAccessFailedCountAsync(user);
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
         
            //Tìm user đã tạo
            var createdUser = await _userManager.FindByEmailAsync(user.Email);
           
            await _userManager.AddToRoleAsync(createdUser, AppRole.CUSTOMER); //Role default when register = Customer
            //Gửi email chứa token xác thực tài khoản
            var verifyToken = await _userManager.GenerateEmailConfirmationTokenAsync(createdUser);

            var confirmationLink = $"http://localhost:8080/confirm-email?token={WebUtility.UrlEncode(verifyToken)}&email={WebUtility.UrlEncode(model.Email)}";

            var message = new Message(new string[] { createdUser.Email! }, "Xác thực tài khoản", confirmationLink);
             _emailService.SendEmail(message);

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

    public async Task<string> ForgotPassword(ForgotPasswordDTO model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            return string.Empty;
        }
        
        var verifyToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        // Tạo confirmation link
        var confirmationLink = $"http://localhost:8080/reset-password?token={WebUtility.UrlEncode(verifyToken)}&email={WebUtility.UrlEncode(model.Email)}";
     
        var message = new Message(new string[] { user.Email! }, "Lấy lại mật khẩu", confirmationLink);
        _emailService.SendForgotPasswordEmail(message);
        return verifyToken;
    }

    public async Task<bool> ResetPassword(ResetPasswordDTO model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if(!result.Succeeded)
        {
            return false;
        }
        return true;
    }

    public async Task<ResponseWithDataDto<List<string>>> GetRoleByIdAsync(Guid id)
    {
        var response = new ResponseWithDataDto<List<string>>() { Message = "Lấy danh sách quyền thành công" };

        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                response.Message = "Không tìm thấy người dùng";
                response.Status = "Fail";
                return response;
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles == null || !userRoles.Any())
            {
                response.Message = "Không tìm thấy quyền";
                response.Status = "Fail";
                return response;
            }
            response.Data = (List<string>?)userRoles;
            response.Status = "Success";

            return response;
        }
        catch (Exception ex)
        {
            response.Message = "Lỗi khi lấy danh sách quyền: " + ex.Message;
            response.Status = "Fail";
            return response;
        }
    }

   
}