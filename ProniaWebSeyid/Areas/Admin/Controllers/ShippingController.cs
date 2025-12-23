using Microsoft.AspNetCore.Mvc;
using ProniaWebSeyid.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ProniaWebSeyid.Models;

namespace ProniaWebSeyid.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShippingController(AddDbContext _context) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var shippings = await _context.Shippings.ToListAsync();
            return View(shippings);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Shipping shipping)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
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
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var shipping = await _context.Shippings.FindAsync(id);
            if (shipping is not { }) return NotFound();
            return View(shipping);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Shipping shipping)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existShipping = await _context.Shippings.FindAsync(shipping.Id);
            if (existShipping is null) return NotFound();
            existShipping.Name = shipping.Name;
            existShipping.Description = shipping.Description;
            existShipping.ImageUrl = shipping.ImageUrl;
            _context.Shippings.Update(existShipping);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
