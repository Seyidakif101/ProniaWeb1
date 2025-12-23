using Microsoft.AspNetCore.Mvc;

namespace ProniaWebSeyid.Conntoller
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
