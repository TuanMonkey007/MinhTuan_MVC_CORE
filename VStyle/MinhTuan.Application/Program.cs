using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MinhTuan.Domain;

using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.Entities;
using MinhTuan.Service.Core;
using MinhTuan.Service.Core.Services;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Logging;
using MinhTuan.Service.Core.Services;
using MinhTuan.Service.Services.CategoryService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor(); // đăng ký để truy cập thông tin

//Add service identity
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<VStyleContext>().AddDefaultTokenProviders();
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
}).AddCookie().AddGoogle(options =>
{
    options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientID").Value;
    options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
}).AddFacebook(options=>
{
    options.ClientId = builder.Configuration.GetSection("FacebookKeys:ClientID").Value;
    options.ClientSecret = builder.Configuration.GetSection("FacebookKeys:ClientSecret").Value;
    // Yêu cầu các quyền cần thiết từ Facebook
    options.Scope.Add("email");
    options.Scope.Add("public_profile");
}); 
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<JwtMiddleware>();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Shared/Error");
    app.UseStatusCodePagesWithReExecute("/Shared/Error", "?statusCode={0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
      name: "areasDashboard",
      pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
     name: "areasCategory",
     pattern: "{area:exists}/{controller=Category}/{action=Index}/{id?}"
   );


});

app.Run();
