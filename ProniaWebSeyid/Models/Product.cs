
namespace ProniaWebSeyid.Models
{
    public class Product:BaseEntity
    {
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Name { get; set; }=null!;
        [Required]
        public string Description { get; set; }=null!;
        [Precision(18,2)]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        public string ImageUrl { get; set; }=null!;
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
