namespace ProniaWebSeyid.ViewModels.ProductViewModels
{
    public class ProductCreateVM
    {
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Precision(18, 2)]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public List<int> TagIds { get; set; }
        [Required]
        [Range(0,5)]
        public int ReytingCount {  get; set; }
        public IFormFile MainImage { get; set; }
        public IFormFile HoverImage { get; set; }
        public List<IFormFile>? Images { get;set; }
    }
}
