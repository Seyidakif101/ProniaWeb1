using Microsoft.AspNetCore.Mvc;

namespace ProniaWebSeyid.Conntoller
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
