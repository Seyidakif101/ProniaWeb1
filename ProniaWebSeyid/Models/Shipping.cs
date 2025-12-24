namespace ProniaWebSeyid.Models
{
    public class Shipping: BaseEntity
    {
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; }=null!;
        public string? Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }=null!;

    }
}
