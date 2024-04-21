using Microsoft.EntityFrameworkCore;
using MinhTuan.Domain;
using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Service.Core;

var builder = WebApplication.CreateBuilder(args);
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
    if (impl != null) builder.Services.AddScoped(intf, impl);
}

builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
var serviceTypes = typeof(IService<>).Assembly.GetTypes()
     .Where(x => !string.IsNullOrEmpty(x.Namespace) && x.Namespace.StartsWith("MinhTuan.Service") && x.Name.EndsWith("Service"));
foreach (var intf in serviceTypes.Where(t => t.IsInterface))
{
    var impl = serviceTypes.FirstOrDefault(c => c.IsClass && intf.Name.Substring(1) == c.Name);
    if (impl != null) builder.Services.AddScoped(intf, impl);
}
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
