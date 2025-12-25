namespace ProniaWebSeyid.Models
{
    public class ProductImage:BaseEntity
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        [Required]
        [MaxLength(100)]
        public string ImageUrl { get; set; }=null!;
    }
}
