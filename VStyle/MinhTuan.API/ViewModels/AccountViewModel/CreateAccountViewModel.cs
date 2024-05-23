namespace MinhTuan.API.ViewModels.AccountViewModel
{
    public class CreateAccountViewModel
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public  string?  Avatar { get; set; }
        public  Guid? Gender { get; set; }
        public string? Address { get; set; }
        public DateOnly? BirthDay { get; set; }
    }
}
