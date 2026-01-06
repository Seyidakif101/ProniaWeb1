
namespace ProniaWebSeyid.Models
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }=null!;
        public string Description { get; set; }=null!;
        public decimal Price { get; set; }
        public int ReytingCount { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? MainImageUrl { get; set; }
        public string? HoverImageUrl { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; } = [];
        public ICollection<ProductTag> ProductTags { get; set; } = [];
    }
}
