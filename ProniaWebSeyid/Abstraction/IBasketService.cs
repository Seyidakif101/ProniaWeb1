using ProniaWebSeyid.ViewModels.BasketItemViewModels;
namespace ProniaWebSeyid.Abstraction
{
    public interface IBasketService
    {
        Task<List<BasketItemVM>> GetBasketItemsAsync();
        //Task<List<BasketItem>> GetBasketItemsAsync();
    }
}
