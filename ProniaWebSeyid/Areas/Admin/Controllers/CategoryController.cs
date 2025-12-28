using Microsoft.AspNetCore.Mvc;
using ProniaWebSeyid.Models;
using ProniaWebSeyid.ViewModels.CategoryViewModels;
using ProniaWebSeyid.ViewModels.ShippingViewModels;

namespace ProniaWebSeyid.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    public class CategoryController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
          
            var categories = await _context.Categories.Select(category => new CategoryGetVM()
            {
                Id = category.Id,
                Name = category.Name,

            }).ToListAsync();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Category category = new()
            {
                Name = vm.Name
            };
            category.CreatedDate = DateTime.UtcNow.AddHours(4);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is null) return NotFound();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is not { }) return NotFound();
            CategoryUpdateVM vm = new CategoryUpdateVM
            {
                Id=category.Id,
                Name = category.Name
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existCategory = await _context.Categories.FindAsync(vm.Id);
            if (existCategory is null) return NotFound();
            existCategory.Name = vm.Name;
            existCategory.UpdatedDate = DateTime.UtcNow.AddHours(4);
            _context.Categories.Update(existCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Info(int id)
        {
            var category = await _context.Categories.Select(category => new CategoryGetVM()
            {
                Id = category.Id,
                Name = category.Name,
                CreatedDate = category.CreatedDate,
                UpdatedDate=category.UpdatedDate

            }).FirstOrDefaultAsync(x => x.Id == id);
            if (category is null) return NotFound();
            return View(category);

        }
    }
}
