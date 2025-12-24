namespace ProniaWebSeyid.Models
{
    public class Category: BaseEntity
    {
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; }=null!;
    }
}
