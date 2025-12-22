using Microsoft.AspNetCore.Mvc;
using ProniaWebSeyid.Contexts;

namespace ProniaWebSeyid.Conntoller
{
    public class HomeController : Controller
    {
        private readonly AddDbContext _context;
        public HomeController(AddDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var shippings = _context.Shippings.ToList();
            return View(shippings);
        }

    }
}
