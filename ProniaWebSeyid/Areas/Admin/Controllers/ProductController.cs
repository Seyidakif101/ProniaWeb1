using CloudinaryDotNet;
using ProniaWebSeyid.Abstraction;
using ProniaWebSeyid.Helpers;
using System.Runtime.Serialization;

namespace ProniaWebSeyid.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    public class ProductController(AppDbContext _context,IWebHostEnvironment _environment,ICloudinaryService _cloudinaryService) : Controller
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
            //string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");

            foreach(var image in vm.Images)
            {
                if (!image.CheckType())
                {
                    ModelState.AddModelError("Images", "File sekil formatinda olmalidir!");
                    return View(vm);
                }
                if (image.CheckSize(2))
                {
                    ModelState.AddModelError("Images", "File olcusu maksimum 2MB ola biler!");
                    return View(vm);
                }
            }
            string hoverImageFileName = await _cloudinaryService.FileUploadAsync(vm.HoverImage);    
            string mainImageFileName = await _cloudinaryService.FileUploadAsync(vm.MainImage);  

            Product product = new()
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                CategoryId = vm.CategoryId,
                MainImageUrl = mainImageFileName,
                HoverImageUrl = hoverImageFileName,
                ReytingCount= vm.ReytingCount,
                ProductTags = [],
                ProductImages = []
            };
            foreach(var image in vm.Images)
            {
                string ImagesFileName= await _cloudinaryService.FileUploadAsync(image);
                ProductImage productImage = new()
                {
                    ImageUrl = ImagesFileName,
                    Product = product
                };
                product.ProductImages.Add(productImage);    
            }
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
            var product = await _context.Products.Include(x=>x.ProductTags).Include(i=>i.ProductImages).SingleOrDefaultAsync(x=>x.Id==id);
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
                TagIds = product.ProductTags.Select(x => x.TagId).ToList(),
                ImagesUrl=product.ProductImages.Select(x => x.ImageUrl).ToList(),
                ImagesUrlIds = product.ProductImages.Select(x => x.Id).ToList()
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

            var existProduct = await _context.Products.Include(x=>x.ProductTags).Include(i => i.ProductImages).FirstOrDefaultAsync(x=>x.Id==vm.Id);
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
            

            //string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");
            if(vm.MainImage is { })
            {
                string newMainImage = await _cloudinaryService.FileUploadAsync(vm.MainImage);
                await _cloudinaryService.FileDeleteAsync(existProduct.MainImageUrl);
                existProduct.MainImageUrl = newMainImage;
            }
            if (vm.HoverImage is { })
            {
                string newHoverImage = await _cloudinaryService.FileUploadAsync(vm.HoverImage);
                await _cloudinaryService.FileDeleteAsync(existProduct.HoverImageUrl);
                existProduct.HoverImageUrl = newHoverImage;
            }
            var existImages = existProduct.ProductImages.ToList();
            foreach (var image in existImages)
            {
                var existImageId = vm.ImagesUrlIds?.Any(x => x == image.Id) ?? false;
                if (!existImageId)
                {
                   await _cloudinaryService.FileDeleteAsync(image.ImageUrl);
                    existProduct.ProductImages.Remove(image);
                }
            }
            foreach (var image in vm.Images ?? [])
            {
                string ImageFileUrl = await _cloudinaryService.FileUploadAsync(image);
                ProductImage productImage = new()
                {
                    ImageUrl = ImageFileUrl,
                    ProductId = existProduct.Id,
                };
                existProduct.ProductImages.Add(productImage);
            }

            _context.Products.Update(existProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.Include(x=>x.ProductImages).FirstOrDefaultAsync();
            if (product is null) return NotFound();
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            await _cloudinaryService.FileDeleteAsync(product.MainImageUrl);
            await _cloudinaryService.FileDeleteAsync(product.HoverImageUrl);

            foreach (var image in product.ProductImages)
            {
                await _cloudinaryService.FileDeleteAsync(image.ImageUrl);
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
                TagNames=product.ProductTags.Select(x=>x.Tag.Name).ToList(),
                ImagesUrl=product.ProductImages.Select(x=>x.ImageUrl).ToList()

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
