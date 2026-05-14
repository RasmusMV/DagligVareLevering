using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Repositories;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;

namespace DagligVareLevering.Service
{
    public class BasketItemService : GenericService<BasketItem>, IBasketItemService
    {
        public BasketItemService(IRepository<BasketItem> repository) : base(repository) { }

        public async Task ClearBasketAsync(int userId)
        {
            foreach (BasketItem item in (await GetObjectsAsync()).Where(b => b.UserId == userId))
            {
                await DeleteObjectAsync(item);
            }
        }

        public async Task AddOrIncrementAsync(int userId, int productId)
        {
            var existingBasketItem = (await GetObjectsAsync())
                .FirstOrDefault(x => x.UserId == userId && x.ProductId == productId);

            //Hvis der er en entity der har både userId og productId allerede, så øges Quantity propertyen med 1
            if (existingBasketItem != null)
            {
                existingBasketItem.Quantity += 1;
                await UpdateObjectAsync(existingBasketItem);
            }
            //Ellers bliver en ny entity lavet med productId og userId
            else
            {
                BasketItem newBasketItem = new BasketItem();
                newBasketItem.ProductId = productId;
                newBasketItem.UserId = userId;
                newBasketItem.Quantity = 1;
                await AddObjectAsync(newBasketItem);
            }
        }

    }
}
