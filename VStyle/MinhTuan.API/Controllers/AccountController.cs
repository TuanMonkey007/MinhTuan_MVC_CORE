using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MinhTuan.API.ViewModels.AccountViewModel;
using MinhTuan.Application.Helper.Constants;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.AccountDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Constants;
using MinhTuan.Service.Services.AccountService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MailKit;
using MinhTuan.Domain.Helper.EmailSender;
using MinhTuan.Service.Core.Services;
using MinhTuan.Domain;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Service.SearchDTO;
using Newtonsoft.Json;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.API.ViewModels.CategoryViewModel;

namespace MinhTuan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(
          
            IAccountService accountService,
            IMapper mapper,
              UserManager<AppUser> userManager,
             SignInManager<AppUser> signInManager,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService

            )
        {
          
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _accountService = accountService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        

        [HttpPost]
        [Route("get-all-user")]
        public async Task<IActionResult> GetDataByPage([FromBody] AccountSearchDTO searchDTO)
        {
            try
            {
                var sessionKey = $"searchAccount_{typeof(UserDTO).Name}_{JsonConvert.SerializeObject(searchDTO)}";
                var jsonData = _httpContextAccessor.HttpContext.Session.GetString(sessionKey);

                if (!string.IsNullOrEmpty(jsonData)) // Kiểm tra jsonData có null hoặc rỗng không
                {
                    try
                    {
                        var usersFromSession = JsonConvert.DeserializeObject<PagedList<UserDTO>>(jsonData);

                        // Kiểm tra tính hợp lệ của usersFromSession
                        if (usersFromSession != null && usersFromSession.Items != null && usersFromSession.Items.Any())
                        {
                            var resultSession = new ResponseWithDataDto<PagedList<UserDTO>>()
                            {
                                Data = usersFromSession,
                            };
                            return Ok(resultSession);
                        }
                        else
                        {
                           
                            _httpContextAccessor.HttpContext.Session.Remove(sessionKey); // Xóa session không hợp lệ
                        }
                    }
                    catch (JsonException ex)
                    {
                       
                        _httpContextAccessor.HttpContext.Session.Remove(sessionKey);
                    }
                }

                var resultGetted =  _accountService.GetDataByPage(searchDTO);

                // Kiểm tra resultGetted.Data trước khi serialize
                if (resultGetted != null && resultGetted.Data != null && resultGetted.Data.Items != null)
                {
                    jsonData = JsonConvert.SerializeObject(resultGetted.Data);
                    HttpContext.Session.SetString(sessionKey, jsonData);
                }

                return Ok(resultGetted); // Trả về resultGetted dù có hay không có dữ liệu
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseWithMessageDto { Status = "Lỗi", Message = "Đã có lỗi xảy ra." });
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var response = new ResponseWithMessageDto { Message = "Tạo tài khoản thành công" };
            if (ModelState.IsValid)
            {
                var modelDTO = _mapper.Map<RegisterDTO>(model);
                modelDTO.UserName = model.PhoneNumber;
                var result = await _accountService.RegisterAsync(modelDTO);
                if (result.Succeeded)
                {
                    //Gửi email chứa token xác thực khi đã đăng ký 
                    return Ok(response);
                }
                response.Message = result.Errors.FirstOrDefault().Code.ToString();
                return Ok(response);
            }
            response.Message = "Lỗi tạo tài khoản";
            return BadRequest(response);
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateAccountViewModel model)
        {
            var response = new ResponseWithMessageDto { Message = "Tạo tài khoản thành công" };
            if (ModelState.IsValid)
            {
                var modelDTO = _mapper.Map<RegisterDTO>(model);
                modelDTO.UserName = model.PhoneNumber;
                var result = await _accountService.RegisterAsync(modelDTO);
                if (result.Succeeded)
                {
                    //Gửi email chứa token xác thực khi đã đăng ký 
                    return Ok(response);
                }
                response.Message = result.Errors.FirstOrDefault().Code.ToString();
                return Ok(response);
            }
            response.Message = "Lỗi tạo tài khoản";
            return BadRequest(response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LogInViewModel model)
        {
            var response = new ResponseWithDataDto<string> { Message = "Đăng nhập thành công" };
            var modelDTO = _mapper.Map<LogInDTO>(model);
            var token = await _accountService.LogInAsync(modelDTO); // Chuỗi token
             response.Data = token;
           return Ok(response);
           
            
        }

      

        [HttpPost]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ChangePasswordViewModel model)
        {
            var userEmail = model.Email;
            var token = model.Token;
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return BadRequest("Invalid user");
            }
        
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok("Email confirmed successfully!");
            }
                
            return BadRequest("Invalid token");
        }
        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var response = new ResponseWithDataDto<string> { Message = "Gửi mã lấy lại mật khẩu thành công" };
            var modelDTO = _mapper.Map<ForgotPasswordDTO>(model);
           var result = await  _accountService.ForgotPassword(modelDTO);
           if(string.IsNullOrEmpty(result))
            {
                response.Data = "";
                response.Message = "Email chưa được đăng ký hoặc chưa xác thực";
                return Ok(response);
            }
            response.Data = "Hoàn thành";
            return Ok(response);
        }
        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ChangePasswordViewModel model)
        {
            var response = new ResponseWithDataDto<string> { Message = "Thay đổi mật khẩu thành công" };
            var modelDTO = _mapper.Map<ChangePasswordDTO>(model);
            var result = await _accountService.ResetPassword(modelDTO);
            if (!result)
            {
                response.Data = "";
                response.Message = "Mã xác thực không hợp lệ, vui lòng yêu cầu lại";
                return Ok(response);
            }
            response.Data = "Hoàn thành";
            return Ok(response);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var serverResponse = new ResponseWithMessageDto() { Message = "Xóa thành công" };

            try
            {
              
                var data = await _userManager.FindByIdAsync(id.ToString());
                if (data == null)
                {
                    serverResponse.Message = "Không tìm thấy dữ liệu";
                    serverResponse.Status = "Fail";
                    return Ok(serverResponse);
                }
                await _userManager.DeleteAsync(data);
                return Ok(serverResponse);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                            new ResponseWithMessageDto { Status = "Error", Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var serverResponse = new ResponseWithDataDto<UserDTO>() { Message = "Lấy thông tin thành công" };
            try
            {
                var result = await _userManager.FindByIdAsync(id.ToString());
                if (result != null)
                {
                    serverResponse.Data = _mapper.Map<UserDTO>(result);
                }
                return Ok(serverResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseWithMessageDto
                {
                    Status = "Fail",
                    Message = ex.Message
                }); ; ;
            }
        }


        [HttpPut("update/{id}")]
        //[Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateAccountViewModel model)
        {

            var serverResponse = new ResponseWithMessageDto() { Message = "Cập nhật thành công" };

            try
            {
                var modelDTO = _mapper.Map<UserDTO>(model);

                //var isExistCode = _user.CheckExitCode(modelDTO.Code, id);
                //if (isExistCode)
                //{
                //    serverResponse.Message = "Trùng mã danh mục";
                //    return Ok(serverResponse);
                //}

                //var isExistName = _categoryService.CheckExitName(modelDTO.Name, id);
                //if (isExistName)
                //{
                //    serverResponse.Message = "Trùng tên danh mục";
                //    return Ok(serverResponse);
                //}
                var EntityNeedUpdate = await _userManager.FindByIdAsync(id.ToString());
                EntityNeedUpdate.FullName = modelDTO.FullName;
                EntityNeedUpdate.PhoneNumber = modelDTO.PhoneNumber;
                EntityNeedUpdate.UserName = modelDTO.PhoneNumber;
                EntityNeedUpdate.Gender = modelDTO.Gender;
                EntityNeedUpdate.BirthDay = modelDTO.BirthDay;
                EntityNeedUpdate.Address = modelDTO.Address;
                EntityNeedUpdate.Avatar = modelDTO.Avatar;//Xử lý lưu ảnh sau
                if(EntityNeedUpdate.Email != modelDTO.Email)//Nếu đã cập nhật email
                {
                    //Thay đổi trạng thái xác nhận của email
                    EntityNeedUpdate.EmailConfirmed = false;
                    EntityNeedUpdate.Email = modelDTO.Email;
                }
                

                 var result = await _userManager.UpdateAsync(EntityNeedUpdate);
                if (result.Succeeded)
                {
                    return Ok(serverResponse);
                }
                serverResponse.Message = result.Errors.FirstOrDefault().Code.ToString();
                return Ok(serverResponse);
            }
            catch (Exception ex)
            {
                serverResponse.Message = "Có lỗi xảy ra khi cập nhật";
                // Có thể tùy chỉnh phản hồi lỗi cụ thể hơn dựa trên loại exception (ex)
                return StatusCode(500, serverResponse); // Trả về mã lỗi 500 (Internal Server Error)
            }

            
        }

    }
}//end namespace
