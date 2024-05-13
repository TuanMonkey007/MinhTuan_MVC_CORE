using Microsoft.AspNetCore.Mvc;
using MinhTuan.Application.Helper.Constants;

namespace MinhTuan.Application.Controllers
{
    public class BaseController : Controller
    {
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            TempData["AlertType"] = type;
            switch (type)
            {
                case AlertType.SUCCEEDED:
                    TempData["ClassIcon"] = "bi bi-check-circle me-1";
                    break;
                case AlertType.WARNING:
                    TempData["ClassIcon"] = "bi bi-exclamation-triangle me-1";
                    break;
                default:
                    TempData["ClassIcon"] = "bi bi-exclamation-octagon me-1";
                    break;
            }
        }
    }
}
