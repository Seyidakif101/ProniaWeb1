namespace ProniaWebSeyid.ViewModels.BasketItemViewModels
{
    public class BasketItemVM
    {
        
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string? Image { get; set; }
            public decimal Price { get; set; }
            public int Count { get; set; }
    
    }
}
