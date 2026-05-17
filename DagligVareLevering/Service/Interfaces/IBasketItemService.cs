using DagligVareLevering.Models;
using DagligVareLevering.Models.DTOs;

namespace DagligVareLevering.Service.Interfaces
{
    public interface IBasketItemService : IService<BasketItem>
    {
        Task ClearBasketAsync(int userId);
        Task AddOrIncrementAsync(int  userId, int productId);
        Task RemoveItemAsync(int userId, int productId);
        Task IncreaseQuantityAsync(int userId, int productId);
        Task DecreaseQuantityAsync(int userId, int productId);
        Task<List<BasketItem>> GetUserBasketItemsWithProductsAsync(int userId);
        Task<CartSummary> GetCartSummaryAsync(int userId);
    }
}
