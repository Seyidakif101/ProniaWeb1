namespace ProniaWebSeyid.Conntoller
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
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
