
namespace ProniaWebSeyid.Conntoller
{
    public class ShopController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }
    }
}
