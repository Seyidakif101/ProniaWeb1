namespace ProniaWebSeyid.ViewModels.ProductViewModels
{
    public class ProductGetVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int  ReytingCount { get; set; }
        public string CategoryName { get; set; } = null!;
        public string MainImageUrl { get; set; } = null!;
        public string HoverImageUrl { get; set; } = null!;
        public List<string> TagNames { get; set; } = [];
        public List<string> ImagesUrl { get; set; } = [];

    }
}
