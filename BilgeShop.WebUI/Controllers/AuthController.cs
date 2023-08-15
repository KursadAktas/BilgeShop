using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.WebUI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BilgeShop.WebUI.Controllers
{
    // Authentication - Authorization
    // (Kimlik Doğrulama - Yetkilendirme)
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        // Dependency Injection
        // Auth controller'a her istek atıldığında, bana bir IUserService nesnesi gönder, ben bunu _userService'e bağlayıp, o değişken üzerinden metotlarını kullanayım.

        [HttpGet]
        [Route("kayit-ol")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Kayit-ol")]
        public IActionResult Register(RegisterViewModel formData)
        {

            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            var addUserDto = new AddUserDto()
            {
                FirstName = formData.FirstName.Trim(),
                LastName = formData.LastName.Trim(),
                Email = formData.Email.Trim(),
                Password = formData.Password.Trim()
            };

           var result =  _userService.AddUser(addUserDto);


            if (result.IsSucceed)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = result.Message;
                return View(formData);
            }

            
        }

        // bir metot içerisinde await kullanılacaksa, metot tanımlanırken async ve task olarak tanımlanır.
        public async Task<IActionResult> Login(LoginViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "home");

            }

            var loginDto = new LoginDto()
            {
                Email = formData.Email,
                Password = formData.Password
            };

           var userInfo = _userService.LoginUser(loginDto);

            if(userInfo is null)
            {

                //  ViewBag.ErrorMessage = "Kullanıcı adı veya şifre hatalı.";  -> return View() durumlarında çalışan ViewBag, RedirectToAction'da çalışmaz.

                TempData["ErrorMessage"] = "Kullanıcı adı veya şifre hatalı.";
            

                return RedirectToAction("Index", "Home");
            }

            // Buraya kadar gelebildiyse kodlar, demek ki kişinin email ve şifresi eşleşmiş. Gerekli bilgileri veritabanından çekilip bu aşamaya kadar userInfo içerisinde gelmiş.

            // Oturumda tutacağım her veri -> Claim
            // Bütün verilerin listesi -> List<Claim>

            var claims = new List<Claim>();

            claims.Add(new Claim("id", userInfo.Id.ToString()));
            claims.Add(new Claim("email", userInfo.Email));
            claims.Add(new Claim("firstName", userInfo.FirstName));
            claims.Add(new Claim("lastName", userInfo.LastName));
            claims.Add(new Claim("userType", userInfo.UserType.ToString()));

            // yetkilendirme için, özel olarak bir claim açmam gerekiyor.

            claims.Add(new Claim(ClaimTypes.Role, userInfo.UserType.ToString()));

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            // claims listesindeki verilerle bir oturum açılacağı için yukarıdaki identity nesnesini tanımladım.

            var autProperties = new AuthenticationProperties
            {
                AllowRefresh = true, // yenilenebilir oturum.
                ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(48)) // oturum açıldıktan sonra 48 saat geçerli.
            };

            
            // await asenkronize (eşzamansız) yapıların birbirlerini bekleyerek çalışmalırını sağlıyor.
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), autProperties);

            TempData["LoginMessage"] = "Giriş başarıyla yapıldı.";
            
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); // oturumu kapat.

            return RedirectToAction("Index", "Home");
        }
    }
}
