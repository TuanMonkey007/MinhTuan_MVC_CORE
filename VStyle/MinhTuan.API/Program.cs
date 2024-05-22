using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MinhTuan.API.Helper;
using MinhTuan.Domain;
using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.EmailSender;
using MinhTuan.Service.Core;
using MinhTuan.Service.Core.Services;
using MinhTuan.Service.Services.CategoryService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "VStyle Api",
        Version = "v1",
        Description = "Api Service for VStyle web app",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Minh Tuấn contact info",
            Email = "dev.minhtuan07@gmail.com",
            Url = new Uri("https://fb.com/minhtuan.me")
        }
    });
});

//

builder.Services.AddHttpContextAccessor(); // đăng ký để truy cập thông tin
//Add service identity
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<VStyleContext>()
    .AddDefaultTokenProviders();  // Đăng ký provider với tên "Xác thực email"
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        // Định nghĩa thuật toán mã hóa
        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha512Signature }
    };
});
builder.Services.Configure<IdentityOptions>(options =>
{
    // Cấu hình định dạng mật khẩu
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
   //Yêu cầu xác thực tài khoản trước khi đăng nhập
    options.SignIn.RequireConfirmedEmail = true;
    
    // Cấu hình lockout
    options.Lockout.MaxFailedAccessAttempts = 5; // Tối đa 5 lần nhập sai
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa trong 5 phút
    options.Lockout.AllowedForNewUsers = true; // Áp dụng lockout cho cả người dùng mới
    options.User.RequireUniqueEmail = true; // Yêu cầu chỉ nhập 1 email
    

});
builder.Services.AddDataProtection(); //Thêm provider token
#region inject dependency
builder.Services.AddAutoMapper(typeof(Program)); // đăng ký sử dụng AutoMaper
//Cấu hình kết nói database
builder.Services.AddDbContext<VStyleContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VStyleContext")));
// Add services to the container.
builder.Services.AddControllersWithViews();
//cấu hình khởi tạo repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
var repositoryTypes = typeof(IRepository<>).Assembly.GetTypes()
                 .Where(x => !string.IsNullOrEmpty(x.Namespace) && x.Namespace.StartsWith("MinhTuan.Domain") && x.Name.EndsWith("Repository"));
foreach (var intf in repositoryTypes.Where(t => t.IsInterface))
{
    var impl = repositoryTypes.FirstOrDefault(c => c.IsClass && intf.Name.Substring(1) == c.Name);
    if (impl != null) builder.Services.AddScoped(intf, impl); //Tự động đăng ký Inject Repository 
}

builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
var serviceTypes = typeof(IService<>).Assembly.GetTypes()
     .Where(x => !string.IsNullOrEmpty(x.Namespace) && x.Namespace.StartsWith("MinhTuan.Service") && x.Name.EndsWith("Service"));
foreach (var intf in serviceTypes.Where(t => t.IsInterface))
{
    var impl = serviceTypes.FirstOrDefault(c => c.IsClass && intf.Name.Substring(1) == c.Name);
    if (impl != null) builder.Services.AddScoped(intf, impl);//Tự động đăng ký Inject Service 
}
builder.Services.AddScoped<ICategoryService, CategoryService>();
// Thêm logging vào container dịch vụ
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion 

//builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
//builder.Services.AddTransient<IMailService, MailService>();
//Add EmailConfiguration
var emailConfiguration = configuration.GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfiguration);
builder.Services.AddScoped<IEmailService, EmailService>();


// Đăng ký Data Protection
builder.Services.AddDataProtection()
    .SetApplicationName("VStyle")
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\VStyleKeys"))//Lưu ý khóa nằm ở ổ C/keys nhớ check khi build
    .SetDefaultKeyLifetime(TimeSpan.FromDays(30))
    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
    {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_GCM,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA512 // Sử dụng HMACSHA512 để xác thực
    });
     
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromMinutes(15);
});
builder.Services.AddDistributedMemoryCache(); // Thêm bộ nhớ đệm phân tán để lưu trữ session

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout của session (tùy chỉnh theo nhu cầu)
    options.Cookie.HttpOnly = true; // Chỉ cho phép truy cập cookie qua HTTP (tăng tính bảo mật)
    options.Cookie.IsEssential = true; // Đánh dấu cookie là cần thiết
});




builder.Services.AddControllers();
var app = builder.Build();

// Thêm middleware xử lý lỗi
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseCors(x => x
       .AllowAnyMethod()
       .AllowAnyHeader()
       .AllowCredentials()
       .SetIsOriginAllowed(origin => true));  // CORS configuration
app.UseSession();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//
app.UseHttpsRedirection();
app.UseAuthentication(); // Ensure authentication is used
app.UseAuthorization();
app.MapControllers();
app.Run();
