

using BilgeShop.Business.Managers;
using BilgeShop.Business.Services;
using BilgeShop.Data.Context;
using BilgeShop.Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BilgeShopContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(SqlRepository<>));
// IRepository tipinde bir new'leme yapýldýðýnda (DI) - SqlRepository kopyasý oluþtur.
// AddScoped -> Her istek için yeni bir kopya.

builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IUserService, UserManager>();
// IUserService tipinde bir DI newlemesi yapýlýrsa UserManager kullanýlacak demek.
// addscoped -> her bir istekte yeni baðýmsýz bir newleme

builder.Services.AddDataProtection();
// Þifreleme-Þifre açma için gerekli.

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/");
    options.LogoutPath = new PathString("/");
    options.AccessDeniedPath = new PathString("/");
    // giriþ - çýkýþ - eriþim reddi durumlarýnda ana sayfaya yönlendiriyorum.
}); 

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles(); // wwwroot kullanýlacak demek. 

// area route - default route'un üstünde yazýlmalý.

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern:"{controller=home}/{action=index}/{id?}");


app.Run();
