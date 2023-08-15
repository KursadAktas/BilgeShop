using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        [Route("urunler/{categoryName}/{categoryId}")]
        public IActionResult Index(int? categoryId)
        {

            ViewBag.CategoryId = categoryId;
            // Buradan ViewBag ile categoryId bilgisini ilgili View'e taşayacağım, View'de istediğim componente göndereceğim.

            return View();
        }
    }
}
