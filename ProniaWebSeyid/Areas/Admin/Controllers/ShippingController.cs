using ProniaWebSeyid.Helpers;
using ProniaWebSeyid.Models;
using ProniaWebSeyid.ViewModels.ShippingViewModels;
using ProniaWebSeyid.ViewModels.TagViewModels;

namespace ProniaWebSeyid.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    public class ShippingController(AppDbContext _context, IWebHostEnvironment _environment) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var shippings = await _context.Shippings.Select(shipping => new ShippingGetVM()
            {
                Id = shipping.Id,
                Name = shipping.Name,
                Description = shipping.Description,
                ImageUrl = shipping.ImageUrl
            }).ToListAsync();
            return View(shippings);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ShippingCreateVM svm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!svm.Image?.CheckType() ?? false)
            {
                ModelState.AddModelError("MainImage", "File sekil formatinda olmalidir!");
                return View(svm);
            }
            if (svm.Image?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("MainImage", "File olcusu maksimum 2MB ola biler!");
                return View(svm);
            }
            string ImageFileName = Guid.NewGuid().ToString() + svm.Image.FileName;
            string ImageUrl = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images", ImageFileName);
            using FileStream Stream = new(ImageUrl, FileMode.Create);
            await svm.Image.CopyToAsync(Stream);
            Shipping shipping = new()
            {
                Name = svm.Name,
                Description = svm.Description,
                ImageUrl= ImageFileName

            };
            await _context.Shippings.AddAsync(shipping);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var shipping = await _context.Shippings.FindAsync(id);
            if (shipping is null) return NotFound();
            _context.Shippings.Remove(shipping);
            await _context.SaveChangesAsync();
            string folderUrl = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");
            string ImageUrl = Path.Combine(folderUrl, shipping.ImageUrl);

            if (System.IO.File.Exists(ImageUrl))
            {
                System.IO.File.Delete(ImageUrl);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var shipping = await _context.Shippings.FindAsync(id);
            if (shipping is not { }) return NotFound();
            ShippingUpdateVM vm = new ShippingUpdateVM
            {
                Id = shipping.Id,
                Name = shipping.Name,
                Description = shipping.Description,
                ImageUrl = shipping.ImageUrl
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ShippingUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existShipping = await _context.Shippings.FindAsync(vm.Id);
            if (existShipping is null) return NotFound();
            if (!vm.Image?.CheckType() ?? false)
            {
                ModelState.AddModelError("MainImage", "File sekil formatinda olmalidir!");
                return View(vm);
            }
            if (vm.Image?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("MainImage", "File olcusu maksimum 2MB ola biler!");
                return View(vm);
            }
            existShipping.Name = vm.Name;
            existShipping.Description = vm.Description;
            existShipping.ImageUrl = vm.ImageUrl;
            string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");
            if (vm.Image is { })
            {
                string newImage = await vm.Image.SaveFileAsync(folderPath);
                string existImage = Path.Combine(folderPath, existShipping.ImageUrl);

                ExtensionMethods.DeleteFile(existImage);
                existShipping.ImageUrl = newImage;
            }
            _context.Shippings.Update(existShipping);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Info(int id)
        {
            var shipping = await _context.Shippings.FindAsync(id);
            if (shipping is null) return NotFound();
            return View(shipping);
        }
    }
}
