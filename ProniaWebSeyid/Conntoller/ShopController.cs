
namespace ProniaWebSeyid.Conntoller
{
    public class ShopController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products.Select(x => new ProductGetVM()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ImagesUrl = x.ProductImages.Select(x => x.ImageUrl).ToList(),
                CategoryName = x.Category.Name,
                HoverImageUrl = x.HoverImageUrl,
                MainImageUrl = x.MainImageUrl,
                Price = x.Price,
                TagNames = x.ProductTags.Select(x => x.Tag.Name).ToList()
            }).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return NotFound();
            return View(product);
           
        }
    }
}
