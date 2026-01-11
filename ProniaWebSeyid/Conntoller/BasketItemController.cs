using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaWebSeyid.Abstraction;
using ProniaWebSeyid.Services;
using ProniaWebSeyid.ViewModels.BasketItemViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProniaWebSeyid.Conntoller
{
    [Authorize]
    public class BasketItemController(AppDbContext _context,IBasketService _service) : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<BasketItemVM> basketItems = await _service.GetBasketItemsAsync();
            return View(basketItems);
        }
        public async Task<IActionResult> AddToBasket(int productId)
        {
            var isExistProduct=await _context.Products.AnyAsync(x=>x.Id==productId);
            if (!isExistProduct)
            {
                return NotFound();
            }
            string userId =User.FindFirstValue(ClaimTypes.NameIdentifier)??"";
            var isExistUser = await _context.Users.AnyAsync(x => x.Id == userId);
            if (!isExistUser)
            {
                return BadRequest();
            }
            var existingBasketItem = await _context.BasketItems.FirstOrDefaultAsync(x=>x.AppUserId==userId&&x.ProductId==productId);
            if (existingBasketItem is { })
            {
                existingBasketItem.Count += 1;
                _context.BasketItems.Update(existingBasketItem);
            }

            else
            {
                BasketItem basketItem = new()
                {
                    ProductId = productId,
                    Count = 1,
                    AppUserId = userId

                };
                await _context.BasketItems.AddAsync(basketItem);
            }
            await _context.SaveChangesAsync();
            string? returnUrl = Request.Headers["Referer"];
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> RemoveFromBasket(int productId)
        {

            var isExistProduct = await _context.Products.AnyAsync(x => x.Id == productId);
            if (!isExistProduct)
            {
                return NotFound();
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var isExistUser = await _context.Users.AnyAsync(x => x.Id == userId);
            if (!isExistUser)
            {
                return BadRequest();
            }
            var basketItem = await _context.BasketItems.FirstOrDefaultAsync(x => x.AppUserId == userId && x.ProductId == productId);
            if (basketItem is null)
            {
                return NotFound();
            }
                
            _context.BasketItems.Remove(basketItem);
            await _context.SaveChangesAsync();
            string? returnUrl = Request.Headers["Referer"];
            if (!string.IsNullOrWhiteSpace(returnUrl))
            { 
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DecreaseBasketItemCount(int productId)
        {
            var isExistProduct = await _context.Products.AnyAsync(x => x.Id == productId);
            if (!isExistProduct)
            {
                return NotFound();
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var isExistUser = await _context.Users.AnyAsync(x => x.Id == userId);
            if (!isExistUser)
            {
                return BadRequest();
            }
            var basketItem = await _context.BasketItems.FirstOrDefaultAsync(x => x.AppUserId == userId && x.ProductId == productId);
            if (basketItem is null)
            {
                return NotFound();
            }
            if (basketItem.Count > 1)
            {
                basketItem.Count--;
            }
            _context.BasketItems.Update(basketItem);
            await _context.SaveChangesAsync();

            var basketItems = await _service.GetBasketItemsAsync();
            return PartialView("_BasketPartialView", basketItems);

        }
        public async Task<IActionResult> IncreaseBasketItemCount(int productId)
        {
            var isExistProduct = await _context.Products.AnyAsync(x => x.Id == productId);
            if (!isExistProduct)
            {
                return NotFound();
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var isExistUser = await _context.Users.AnyAsync(x => x.Id == userId);
            if (!isExistUser)
            {
                return BadRequest();
            }
            var basketItem = await _context.BasketItems.FirstOrDefaultAsync(x => x.AppUserId == userId && x.ProductId == productId);
            if (basketItem is null)
            {
                return NotFound();
            }

                basketItem.Count++;
            
            _context.BasketItems.Update(basketItem);
            await _context.SaveChangesAsync();

            var basketItems = await _service.GetBasketItemsAsync();
            return PartialView("_BasketPartialView", basketItems);

        }


    }
}
