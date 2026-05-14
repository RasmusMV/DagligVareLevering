using DagligVareLevering.Models;

namespace DagligVareLevering.Service.Interfaces
{
    public interface IBasketItemService : IService<BasketItem>
    {
        Task ClearBasketAsync(int userId);
        Task AddOrIncrementAsync(int  userId, int productId);
    }
}
