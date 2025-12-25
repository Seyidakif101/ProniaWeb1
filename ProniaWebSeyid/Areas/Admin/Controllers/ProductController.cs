namespace ProniaWebSeyid.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    public class ProductController(AppDbContext _context,IWebHostEnvironment _environment) : Controller
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
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {

            if (!ModelState.IsValid)
            {
                await ViewsBagCategoryId();
                return View(vm);
            }
            var isExistingCategory = await _context.Categories.AnyAsync(c => c.Id == vm.CategoryId);
            if (!isExistingCategory)
            {
                await ViewsBagCategoryId();
                ModelState.AddModelError("CategoryId", "Secdiyiniz Kateqoriya yoxdu!");
                return View(vm);
            }
            if (vm.ReytingCount > 6 || vm.ReytingCount < 0)
            {
                ModelState.AddModelError("ReytingCount", "Reyting0-5 arasi olmalidi!");
                return View(vm);
            }
            
            if (!vm.MainImage.ContentType.Contains("image"))
            {
                ModelState.AddModelError("MainImage", "File sekil formatinda olmalidir!");
                return View(vm);
            }
            if(vm.MainImage.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("MainImage", "File olcusu maksimum 2MB ola biler!");
                return View(vm);
            }
            if (!vm.HoverImage.ContentType.Contains("image"))
            {
                ModelState.AddModelError("HoverImage", "File sekil formatinda olmalidir!");
                return View(vm);
            }
            if (vm.HoverImage.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("HoverImage", "File olcusu maksimum 2MB ola biler!");
                return View(vm);
            }
            string mainImageFileName = Guid.NewGuid().ToString() + vm.MainImage.FileName;
            string mainImagePath = Path.Combine(_environment.WebRootPath,"assets","images","website-images", mainImageFileName);
            using FileStream mainStream = new(mainImagePath, FileMode.Create);
            await vm.MainImage.CopyToAsync(mainStream);

            string hoverImageFileName = Guid.NewGuid().ToString() + vm.HoverImage.FileName;
            string hoverImagePath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images", hoverImageFileName);
            using FileStream hoverStream = new(hoverImagePath, FileMode.Create);
            await vm.HoverImage.CopyToAsync(hoverStream);

            Product product = new()
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                CategoryId = vm.CategoryId,
                MainImageUrl = mainImageFileName,
                HoverImageUrl = hoverImageFileName,
                ReytingCount= vm.ReytingCount,

            };

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
            //existProduct.ImageUrl = product.ImageUrl;
            existProduct.CategoryId = product.CategoryId;
            _context.Products.Update(existProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null) return NotFound();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            string folderUrl = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");
            string hoverImageUrl = Path.Combine(folderUrl, product.HoverImageUrl);
            string mainImageUrl = Path.Combine(folderUrl, product.MainImageUrl);

            if (System.IO.File.Exists(hoverImageUrl))
            {
                System.IO.File.Delete(hoverImageUrl);
            }
            if (System.IO.File.Exists(mainImageUrl))
            {
                System.IO.File.Delete(mainImageUrl);
            }

            return RedirectToAction(nameof(Index));
        }
        private async Task ViewsBagCategoryId()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
        }
    }
}
