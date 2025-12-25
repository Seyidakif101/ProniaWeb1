namespace ProniaWebSeyid.ViewModels.ShippingViewModels
{
    public class ShippingCreateVM
    {
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
