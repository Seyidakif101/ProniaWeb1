using Microsoft.AspNetCore.Mvc;
using ProniaWebSeyid.ViewModels.CategoryViewModels;
using ProniaWebSeyid.ViewModels.TagViewModels;

namespace ProniaWebSeyid.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    public class TagController(AppDbContext _context) : Controller
    {

        public async Task<IActionResult> Index()
        {


            var tags = await _context.Tags.Select(tag => new TagGetVM()
            {
                Id = tag.Id,
                Name = tag.Name,

            }).ToListAsync();
            return View(tags);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TagCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Tag tag = new()
            {
                Name = vm.Name
            };
            tag.CreatedDate = DateTime.UtcNow.AddHours(4);
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag is null) return NotFound();
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag is not { }) return NotFound();
            TagUpdateVM vm = new TagUpdateVM
            {
                Id = tag.Id,
                Name = tag.Name
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(TagUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existTag = await _context.Tags.FindAsync(vm.Id);
            if (existTag is null) return NotFound();
            existTag.Name = vm.Name;
            existTag.UpdatedDate = DateTime.UtcNow.AddHours(4);
            _context.Tags.Update(existTag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Info(int id)
        {
            var tag = await _context.Tags.Select(tag => new TagGetVM()
            {
                Id = tag.Id,
                Name = tag.Name,
                CreatedDate = tag.CreatedDate,
                UpdatedDate = tag.UpdatedDate

            }).FirstOrDefaultAsync(x => x.Id == id);
            if (tag is null) return NotFound();
            return View(tag);

        }
    }
}
