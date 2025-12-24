namespace ProniaWebSeyid.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    public class ProductController(AppDbContext _context) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(x => x.Category).ToListAsync();
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await ViewsBagCategoryId();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {

            if (!ModelState.IsValid)
            {
                await ViewsBagCategoryId();
                return View(product);
            }
            var isExistingCategory = await _context.Categories.AnyAsync(c => c.Id == product.CategoryId);
            if (!isExistingCategory)
            {
                await ViewsBagCategoryId();
                ModelState.AddModelError("CategoryId", "Secdiyiniz Kateqoriya yoxdu!");
                return View(product);
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null) return NotFound();
            await ViewsBagCategoryId();
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            if (!ModelState.IsValid)
            {
                await ViewsBagCategoryId();
                return View(product);
            }

            var existProduct = await _context.Products.FindAsync(product.Id);
            if (existProduct is null) return NotFound();
            var isExistingCategory = await _context.Categories.AnyAsync(c => c.Id == product.CategoryId);
            if (!isExistingCategory)
            {
                ModelState.AddModelError("CategoryId", "Secdiyiniz Kateqoriya yoxdu!");
                return View(product);
            }
            existProduct.Name = product.Name;
            existProduct.Price = product.Price;
            existProduct.ImageUrl = product.ImageUrl;
            existProduct.CategoryId = product.CategoryId;
            _context.Products.Update(existProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null) return NotFound();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private async Task ViewsBagCategoryId()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
        }
    }
}
