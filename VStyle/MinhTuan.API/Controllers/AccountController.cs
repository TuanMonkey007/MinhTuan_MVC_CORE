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
using MinhTuan.API.Helper;
using System.IO;
using MinhTuan.Service.Services.DataCategoryService;
using Microsoft.AspNetCore.Authorization;

namespace MinhTuan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;
        private readonly IDataCategoryService _dataCategoryService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(
          
            IAccountService accountService,
            IDataCategoryService dataCategoryService,
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
            _dataCategoryService = dataCategoryService;
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
                foreach(var item in resultGetted.Data.Items)
                {
                    if(item.Gender.HasValue)
                    {
                        item.NameGender = _dataCategoryService.GetNameById((Guid)item.Gender);
                    }
                    
                }

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
        public async Task<IActionResult> Create([FromForm] CreateAccountViewModel model)
        {
            var response = new ResponseWithMessageDto { Message = "Tạo tài khoản thành công" };
            if (ModelState.IsValid)
            {
                if(model.AvatarFile != null)
                {
                    var avatarFileName = new UploadHandler().UploadProfileImage(model.AvatarFile);//validate file ảnh
                    if (avatarFileName.Contains("Kích") || avatarFileName.Contains("Định"))
                    {
                        response.Message = avatarFileName;
                        return Ok(response);
                    }
                    var modelDTO = _mapper.Map<RegisterDTO>(model);
                    modelDTO.UserName = model.PhoneNumber;

                    var result = await _accountService.RegisterAsync(modelDTO);
                    if (result.Succeeded)
                    {
                        var userCreated = await _userManager.FindByNameAsync(modelDTO.UserName);
                        userCreated.Avatar = avatarFileName;
                        await _userManager.UpdateAsync(userCreated);
                        //Gửi email chứa token xác thực khi đã đăng ký 
                        return Ok(response);
                    }
                    response.Message = result.Errors.FirstOrDefault().Code.ToString();
                    return Ok(response);
                }
                else
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
        public async Task<IActionResult> ConfirmEmail(ResetPasswordViewModel model)
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
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var response = new ResponseWithDataDto<string> { Message = "Thay đổi mật khẩu thành công" };
            var modelDTO = _mapper.Map<ResetPasswordDTO>(model);
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
        [HttpGet("get-by-phone/{id}")]
        public async Task<IActionResult> GetByPhoneAsync(string id)
        {
            var serverResponse = new ResponseWithDataDto<UserDTO>() { Message = "Lấy thông tin thành công" };
            try
            {
                var result = await _userManager.FindByNameAsync(id);
                if (result != null)
                {
                    var userWithImage = _mapper.Map<UserDTO>(result);
                    if (!string.IsNullOrEmpty(userWithImage.Avatar))
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Avatar", userWithImage.Avatar);

                        if (System.IO.File.Exists(path))
                        {
                            // Đọc file ảnh dưới dạng mảng byte
                            byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(path);

                            // Chuyển đổi mảng byte thành base64
                            userWithImage.AvatarBase64 = Convert.ToBase64String(imageBytes);

                            // Lấy ContentType dựa trên phần mở rộng của file
                            string fileExtension = Path.GetExtension(result.Avatar).ToLowerInvariant();
                            userWithImage.AvatarContentType = fileExtension switch
                            {
                                ".jpg" or ".jpeg" => "image/jpeg",
                                ".png" => "image/png",
                                _ => "application/octet-stream",
                            };
                        }
                        else
                        {
                            userWithImage.AvatarBase64 = null;
                        }
                    }

                    serverResponse.Data = userWithImage;
                }
                return Ok(serverResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseWithMessageDto
                {
                    Status = "Fail",
                    Message = ex.Message
                });
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
                    var userWithImage = _mapper.Map<UserDTO>(result);
                    if (!string.IsNullOrEmpty(userWithImage.Avatar))
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Avatar", userWithImage.Avatar);

                        if (System.IO.File.Exists(path))
                        {
                            // Đọc file ảnh dưới dạng mảng byte
                            byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(path);

                            // Chuyển đổi mảng byte thành base64
                            userWithImage.AvatarBase64 = Convert.ToBase64String(imageBytes);

                            // Lấy ContentType dựa trên phần mở rộng của file
                            string fileExtension = Path.GetExtension(result.Avatar).ToLowerInvariant();
                            userWithImage.AvatarContentType = fileExtension switch
                            {
                                ".jpg" or ".jpeg" => "image/jpeg",
                                ".png" => "image/png",
                                _ => "application/octet-stream",
                            };
                        }
                        else
                        {
                            userWithImage.AvatarBase64 = null;
                        }
                    }

                    serverResponse.Data = userWithImage;
                }
                return Ok(serverResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseWithMessageDto
                {
                    Status = "Fail",
                    Message = ex.Message
                });
            }
        }

        [HttpGet("get-user-by-email/{email}")]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            var serverResponse = new ResponseWithDataDto<UserDTO>() { Message = "Lấy thông tin thành công" };
            try
            {

                var result = await _userManager.FindByEmailAsync(email);
                if (result != null)
                {
                    
                    var userWithImage = _mapper.Map<UserDTO>(result);
                    if (!string.IsNullOrEmpty(userWithImage.Avatar))
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Avatar", userWithImage.Avatar);

                        if (System.IO.File.Exists(path))
                        {
                            // Đọc file ảnh dưới dạng mảng byte
                            byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(path);

                            // Chuyển đổi mảng byte thành base64
                            userWithImage.AvatarBase64 = Convert.ToBase64String(imageBytes);

                            // Lấy ContentType dựa trên phần mở rộng của file
                            string fileExtension = Path.GetExtension(result.Avatar).ToLowerInvariant();
                            userWithImage.AvatarContentType = fileExtension switch
                            {
                                ".jpg" or ".jpeg" => "image/jpeg",
                                ".png" => "image/png",
                                _ => "application/octet-stream",
                            };
                        }
                        else
                        {
                            userWithImage.AvatarBase64 = null;
                        }
                    }
                    userWithImage.NameGender = _dataCategoryService.GetNameById((Guid)result.Gender);
                    serverResponse.Data = userWithImage;
                }
                return Ok(serverResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseWithMessageDto
                {
                    Status = "Fail",
                    Message = ex.Message
                });
            }
        }

        [HttpPut("update/{id}")]
        //[Authorize]
        public async Task<IActionResult> Update(Guid id, [FromForm] CreateAccountViewModel model)
        {

            var serverResponse = new ResponseWithMessageDto() { Message = "Cập nhật thành công" };

            try
            {
                var modelDTO = _mapper.Map<UserDTO>(model);
                string avatarFileName = model.Avatar;
                if(model.AvatarFile != null)
                {
                     avatarFileName = new UploadHandler().UploadProfileImage(model.AvatarFile);//validate file ảnh
                    if (avatarFileName.Contains("Kích") || avatarFileName.Contains("Định"))
                    {
                        serverResponse.Message = avatarFileName;
                        return Ok(serverResponse);
                    }
                }
            
                
               
                var EntityNeedUpdate = await _userManager.FindByIdAsync(id.ToString());
                EntityNeedUpdate.FullName = modelDTO.FullName;
                EntityNeedUpdate.PhoneNumber = modelDTO.PhoneNumber;
                EntityNeedUpdate.UserName = modelDTO.PhoneNumber;
                EntityNeedUpdate.Gender = modelDTO.Gender;
                EntityNeedUpdate.BirthDay = modelDTO.BirthDay;
                EntityNeedUpdate.Address = modelDTO.Address;
                EntityNeedUpdate.Avatar = avatarFileName ;//Đổi link ảnh mối
                EntityNeedUpdate.ProvinceId = modelDTO.ProvinceId;
                EntityNeedUpdate.DistrictId = modelDTO.DistrictId;
                EntityNeedUpdate.WardId = modelDTO.WardId;
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
        [HttpPost("valid-upload")]

        public IActionResult ValidataImage([FromForm]IFormFile file)
        {
            var message = new UploadHandler().ValidateImage(file);
            if (message.Contains("Định") || message.Contains("Kích")){
                return BadRequest(message);
            }
            return Ok(message);
        }
        [HttpGet("change-lock/{id}")]
        public async Task<IActionResult> ChangeLockUser(Guid id)
        {
            var userNeedChange = await _userManager.FindByIdAsync(id.ToString());

            if (userNeedChange == null)
            {
                return NotFound(); // Hoặc trả về BadRequest nếu bạn muốn
            }

            // Kiểm tra xem người dùng có đang bị khóa hay không (bằng cách xem LockoutEnd có giá trị và lớn hơn thời điểm hiện tại hay không)
            if (userNeedChange.LockoutEnd.HasValue && userNeedChange.LockoutEnd > DateTimeOffset.UtcNow)
            {
                // Nếu đang bị khóa, mở khóa
                await _userManager.SetLockoutEndDateAsync(userNeedChange, null);
                return Ok(new { message = "Mở khóa tài khoản thành công" });
            }
            else
            {
                // Nếu chưa bị khóa, khóa vĩnh viễn
                await _userManager.SetLockoutEndDateAsync(userNeedChange, DateTimeOffset.MaxValue);
                return Ok(new { message = "Khóa tài khoản thành công" });
            }
        }


        [HttpGet("get-roles/{id}")]
      //  [Authorize]
        public async Task<IActionResult> GetRoleByID(Guid id)
        {

            var listRoles = await _accountService.GetRoleByIdAsync(id);
            return Ok(listRoles);
            
        }
        [HttpPut("set-role/{id}")]
        [Authorize]
        public async Task<IActionResult> SetRoleUser(Guid id, [FromBody] List<string> listRole)
        {
            var response = new ResponseWithMessageDto() { Message = "Cập nhật quyền thành công" };

            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    response.Message = "Không tìm thấy người dùng";
                    response.Status = "Fail";
                    return NotFound(response);
                }

                // Nếu listRole rỗng, gán mặc định là CUSTOMER
                if (listRole == null || !listRole.Any())
                {
                    listRole = new List<string> { "CUSTOMER" };
                }

                var currentRoles = await _userManager.GetRolesAsync(user);

                // Xóa các quyền hiện tại của người dùng
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    response.Message = "Lỗi khi xóa quyền hiện tại: " + string.Join(", ", removeResult.Errors.Select(e => e.Description));
                    response.Status = "Fail";
                    return BadRequest(response);
                }

                // Thêm các quyền mới cho người dùng
                var addResult = await _userManager.AddToRolesAsync(user, listRole);
                if (!addResult.Succeeded)
                {
                    response.Message = "Lỗi khi thêm quyền mới: " + string.Join(", ", addResult.Errors.Select(e => e.Description));
                    response.Status = "Fail";
                    return BadRequest(response);
                }

                response.Status = "Success";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = "Lỗi khi cập nhật quyền: " + ex.Message;
                response.Status = "Fail";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("change-email-confirmed/{id}")]
        [Authorize]
        public async Task<IActionResult> ChangeEmailConfirmed(Guid id)
        {
            var response = new ResponseWithMessageDto();

            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    response.Message = "Không tìm thấy người dùng";
                    response.Status = "Fail";
                    return NotFound(response);
                }

                user.EmailConfirmed = !user.EmailConfirmed; // Đảo ngược giá trị EmailConfirmed

                var updateResult = await _userManager.UpdateAsync(user); // Cập nhật thông tin người dùng
                if (!updateResult.Succeeded)
                {
                    response.Message = "Lỗi khi cập nhật trạng thái xác nhận email: " + string.Join(", ", updateResult.Errors.Select(e => e.Description));
                    response.Status = "Fail";
                    return BadRequest(response);
                }

                response.Message = "Cập nhật trạng thái xác nhận email thành công";
                response.Status = "Success";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = "Lỗi khi cập nhật trạng thái xác nhận email: " + ex.Message;
                response.Status = "Fail";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut("change-password/{id}")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(Guid id, ChangePasswordViewModel model)
        {
            var response = new ResponseWithMessageDto() { Message = "Đổi mật khẩu thành công" };
            var user = await _userManager.FindByIdAsync(id.ToString());
            var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok(response);
            }
            response.Message = result.Errors.First().Description;
            return Ok(response);

        }

    }


}//end namespace
