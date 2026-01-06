using Microsoft.AspNetCore.Authorization;

namespace ProniaWebSeyid.Conntoller
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            var shippings = _context.Shippings.ToList();
            return View(shippings);
        }

    }
}
