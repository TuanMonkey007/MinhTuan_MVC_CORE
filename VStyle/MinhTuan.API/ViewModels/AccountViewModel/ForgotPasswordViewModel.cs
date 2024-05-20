using System.ComponentModel.DataAnnotations;

namespace MinhTuan.API.ViewModels.AccountViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required, EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string  Email { get; set; }
    }
}
