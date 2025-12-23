using System.ComponentModel.DataAnnotations;

namespace ProniaWebSeyid.Models
{
    public class Shipping
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; }=null!;
        public string? Description { get; set; }
        [Requeired]
        public string ImageUrl { get; set; }=null!;

    }
}
