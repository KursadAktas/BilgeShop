

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
// IRepository tipinde bir new'leme yap�ld���nda (DI) - SqlRepository kopyas� olu�tur.
// AddScoped -> Her istek i�in yeni bir kopya.

builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IUserService, UserManager>();
// IUserService tipinde bir DI newlemesi yap�l�rsa UserManager kullan�lacak demek.
// addscoped -> her bir istekte yeni ba��ms�z bir newleme

builder.Services.AddDataProtection();
// �ifreleme-�ifre a�ma i�in gerekli.

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/");
    options.LogoutPath = new PathString("/");
    options.AccessDeniedPath = new PathString("/");
    // giri� - ��k�� - eri�im reddi durumlar�nda ana sayfaya y�nlendiriyorum.
}); 

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles(); // wwwroot kullan�lacak demek. 

// area route - default route'un �st�nde yaz�lmal�.

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern:"{controller=home}/{action=index}/{id?}");


app.Run();
