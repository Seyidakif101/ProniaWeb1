namespace ProniaWebSeyid.ViewModels.ShippingViewModels
{
    public class ShippingUpdateVM
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; } = null!;
        [MaxLength(50)]
        [MinLength(3)]
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
    }
}
