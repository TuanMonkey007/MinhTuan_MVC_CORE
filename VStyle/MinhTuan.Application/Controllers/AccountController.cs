using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhTuan.Application.ViewModels.AccountViewModel;
using MinhTuan.Domain.DTOs.AccountDTO;
using MinhTuan.Domain.Helper;
using MinhTuan.Service.Services.AccountService;
using MinhTuan.Service.Services.CategoryService;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MinhTuan.Application.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AccountController(ILogger<HomeController> logger, IMapper mapper,IAccountService accountService,
            IHttpContextAccessor httpContextAccessor
           )
        {
            _logger = logger;
            _mapper = mapper;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
            
        }

        [AllowAnonymous]

        public IActionResult ShowView_LogIn()
        {
            // Kiểm tra nếu người dùng đã đăng nhập, chuyển hướng đến trang chính
            if (User.Identity.IsAuthenticated)
            {
                if(User.Claims.Any(c => c.Type == ClaimTypes.Role && (c.Value == AppRole.ADMINISTRATOR || c.Value == AppRole.STAFF)))
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Dashboard" });
                }
                return RedirectToAction("Index", "Home");
            }

            // Ngược lại, hiển thị view đăng nhập
            var model = new LogInViewModel();
            return PartialView("Login");
        }
        [AllowAnonymous]
        public PartialViewResult ShowView_Register()
        {
            return PartialView("Register");
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            if (!ModelState.IsValid)
            {
                return View(model); // Trả lại cùng một view với thông tin đăng ký ban đầu và thông báo lỗi
            }

            var modelDTO = _mapper.Map<RegisterDTO>(model);
            modelDTO.UserName = model.PhoneNumber;
            var result = await _accountService.RegisterAsync(modelDTO);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home"); // Chuyển hướng đến action Index của HomeController
            }

            ModelState.AddModelError(string.Empty, "Đăng ký thất bại. Vui lòng thử lại."); // Thêm thông báo lỗi vào ModelState
            return View(model); // Trả lại cùng một view với thông tin đăng ký ban đầu và thông báo lỗi
            
        }
        public async Task<IActionResult> Login(LogInViewModel model)
        {
            var modelDTO = _mapper.Map<LogInDTO>(model);
            var token = await _accountService.LogInAsync(modelDTO); // Chuỗi token
            if (!string.IsNullOrEmpty(token))
            {
				var tokenHandler = new JwtSecurityTokenHandler();
				var jwtToken = tokenHandler.ReadJwtToken(token);

                var issuer = jwtToken.Issuer;
				//làm sao để cài authorization lên request header
				// Store the token in a cookie
				Response.Cookies.Append("JwtToken", token);

				// Kiểm tra xem trong các claims có role là Admin không
				var isAdmin = jwtToken.Claims.Any(c => c.Type == ClaimTypes.Role && (c.Value == AppRole.ADMINISTRATOR || c.Value == AppRole.STAFF));
                if (isAdmin)
                {
                    
					return RedirectToAction("Index", "Dashboard", new { area = "Dashboard" }); // Chuyển hướng đến action Index của DashboardController trong Area Admin
                }
                return RedirectToAction("Index", "Home");
			}

            ModelState.AddModelError(string.Empty, "Sai thông tin đăng nhập. Vui lòng thử lại."); // Thêm thông báo lỗi vào ModelState
            return View(model); // Trả lại cùng một view với thông tin đăng ký ban đầu và thông báo lỗi
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogOutAsync();
            // Delete the JWT token cookie
            Response.Cookies.Delete("JwtToken");

            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult UserProfile()
        {
            return View();
        }

    }
}
