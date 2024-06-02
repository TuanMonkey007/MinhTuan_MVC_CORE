using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace MinhTuan.API.ViewModels.AccountViewModel
{
    public class ResetPasswordViewModel
    {
        public  string? Token { get; set; }
        public string?  Password { get; set; }
        public string?  Email { get; set; }

    }
    public class ChangePasswordViewModel
    {
        public string? NewPassword { get; set; }
        public string? Password { get; set; }

    }

}
