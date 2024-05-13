using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhTuan.Domain.Helper.Constants;

namespace MinhTuan.Application.Areas.Admin.Controllers
{
    [Area("Dashboard")]
    public class DashboardController : Controller
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public DashboardController(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}
		
			[Authorize(Roles =AppRole.ADMINISTRATOR + "," + AppRole.STAFF)] 
			public IActionResult Index()
			{
			
				return View();
			}
	}
}
