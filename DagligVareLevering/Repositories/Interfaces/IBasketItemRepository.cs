using DagligVareLevering.Models;

namespace DagligVareLevering.Repositories.Interfaces
{
    public interface IBasketItemRepository : IRepository<BasketItem>
    {
        Task<List<BasketItem>> GetUserBasketItemsWithProductsAsync(int userId);
    }
}
