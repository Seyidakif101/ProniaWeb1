using ProniaWebSeyid.Helpers;
using System.Runtime.Serialization;

namespace ProniaWebSeyid.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    public class ProductController(AppDbContext _context,IWebHostEnvironment _environment) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(x => x.Category).Select(product=>new ProductGetVM()
            {
                Id= product.Id,
                Name= product.Name,
                Description= product.Description,
                CategoryName=product.Category.Name,
                Price= product.Price,
                ReytingCount=product.ReytingCount,
                MainImageUrl =product.MainImageUrl,
                HoverImageUrl=product.HoverImageUrl

            }).ToListAsync();
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await ViewsBagItem();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {

            if (!ModelState.IsValid)
            {
                await ViewsBagItem();
                return View(vm);
            }
            var isExistingCategory = await _context.Categories.AnyAsync(c => c.Id == vm.CategoryId);
            if (!isExistingCategory)
            {
                await ViewsBagItem();
                ModelState.AddModelError("CategoryId", "Secdiyiniz Kateqoriya yoxdu!");
                return View(vm);
            }
            foreach(var tagId in vm.TagIds)
            {
                var isExistTag= await _context.Tags.AnyAsync(x=>x.Id== tagId);
                await ViewsBagItem();
                if (!isExistTag)
                {

                ModelState.AddModelError("TagIds", "Bele bir tag yoxdur");
                return View(vm);
                }
            }
            if (vm.ReytingCount > 6 || vm.ReytingCount < 0)
            {
                ModelState.AddModelError("ReytingCount", "Reyting0-5 arasi olmalidi!");
                return View(vm);
            }

            if (!vm.MainImage.CheckType())
            {
                ModelState.AddModelError("MainImage", "File sekil formatinda olmalidir!");
                return View(vm);
            }
            if (vm.MainImage.CheckSize(2))
            {
                ModelState.AddModelError("MainImage", "File olcusu maksimum 2MB ola biler!");
                return View(vm);
            }
            if (!vm.HoverImage.CheckType())
            {
                ModelState.AddModelError("HoverImage", "File sekil formatinda olmalidir!");
                return View(vm);
            }
            if (vm.HoverImage.CheckSize(2))
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
                ProductTags = []
            };
            foreach (var tagId in vm.TagIds)
            {
                ProductTag productTag = new()
                {
                    TagId = tagId,
                    Product = product
                };
                product.ProductTags.Add(productTag);

            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _context.Products.Include(x=>x.ProductTags).SingleOrDefaultAsync(x=>x.Id==id);
            if (product is null) return NotFound();
            await ViewsBagItem();
            ProductUpdateVM vm = new ProductUpdateVM()
            {
                Id = product.Id,
                Name= product.Name,
                Description= product.Description,
                Price= product.Price,
                CategoryId= product.CategoryId,
                MainImageUrl= product.MainImageUrl,
                HoverImageUrl= product.HoverImageUrl,
                TagIds = product.ProductTags.Select(x => x.TagId).ToList()
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                await ViewsBagItem();
                return View(vm);
            }

            var existProduct = await _context.Products.Include(x=>x.ProductTags).FirstOrDefaultAsync(x=>x.Id==vm.Id);
            if (existProduct is null) return NotFound();
            var isExistingCategory = await _context.Categories.AnyAsync(c => c.Id == vm.CategoryId);
            if (!isExistingCategory)
            {
                ModelState.AddModelError("CategoryId", "Secdiyiniz Kateqoriya yoxdu!");
                return View(vm);
            }
            foreach (var tagId in vm.TagIds)
            {
                var isExistTag = await _context.Tags.AnyAsync(x => x.Id == tagId);
                await ViewsBagItem();
                if (!isExistTag)
                {

                    ModelState.AddModelError("TagIds", "Bele bir tag yoxdur");
                    return View(vm);
                }
            }
            if (vm.ReytingCount > 6 || vm.ReytingCount < 0)
            {
                ModelState.AddModelError("ReytingCount", "Reyting0-5 arasi olmalidi!");
                return View(vm);
            }

            if (!vm.MainImage?.CheckType() ?? false)
            {
                ModelState.AddModelError("MainImage", "File sekil formatinda olmalidir!");
                return View(vm);
            }
            if (vm.MainImage?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("MainImage", "File olcusu maksimum 2MB ola biler!");
                return View(vm);
            }
            if (!vm.HoverImage?.CheckType() ?? false)
            {
                ModelState.AddModelError("HoverImage", "File sekil formatinda olmalidir!");
                return View(vm);
            }
            if (vm.HoverImage?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("HoverImage", "File olcusu maksimum 2MB ola biler!");
                return View(vm);
            }
            existProduct.Name = vm.Name;
            existProduct.Description = vm.Description;
            existProduct.CategoryId = vm.CategoryId;
            existProduct.Price = vm.Price;
            existProduct.ProductTags = [];
            foreach(var tagId in vm.TagIds)
            {
                ProductTag productTag = new()
                {
                    TagId = tagId,
                    ProductId = existProduct.Id
                };
                existProduct.ProductTags.Add(productTag);
            }


            string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");
            if(vm.MainImage is { })
            {
                string newMainImage = await vm.MainImage.SaveFileAsync(folderPath);
                string existMainImage =Path.Combine(folderPath,existProduct.MainImageUrl);

                ExtensionMethods.DeleteFile(existMainImage);
                existProduct.MainImageUrl = newMainImage;
            }
            if (vm.HoverImage is { })
            {
                string newHoverImage = await vm.HoverImage.SaveFileAsync(folderPath);
                string existHoverImage = Path.Combine(folderPath,existProduct.HoverImageUrl);

                ExtensionMethods.DeleteFile(existHoverImage);
                existProduct.HoverImageUrl = newHoverImage;
            }
           
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
        public async Task<IActionResult> Info(int id)
        {
            var product = await _context.Products.Include(x => x.Category).Select(product => new ProductGetVM()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryName = product.Category.Name,
                Price = product.Price,
                ReytingCount = product.ReytingCount,
                MainImageUrl = product.MainImageUrl,
                HoverImageUrl = product.HoverImageUrl,
                TagNames=product.ProductTags.Select(x=>x.Tag.Name).ToList()

            }).FirstOrDefaultAsync(x=>x.Id==id);
            if (product is null) return NotFound();
            return View(product);
        }
        private async Task ViewsBagItem()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;

            var tags = await _context.Tags.ToListAsync();
            ViewBag.Tags = tags;
        }
    }
}
