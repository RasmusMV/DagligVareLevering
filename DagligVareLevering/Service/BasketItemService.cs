using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Models.DTOs;
using DagligVareLevering.Repositories;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;

namespace DagligVareLevering.Service
{
    public class BasketItemService : GenericService<BasketItem>, IBasketItemService
    {
        private readonly IBasketItemRepository _basketItemRepository;
        private readonly CartEventService _cartEventService;

        public BasketItemService(IBasketItemRepository basketItemRepository, CartEventService cartEventService) : base(basketItemRepository)
        {
            _basketItemRepository = basketItemRepository;
            _cartEventService=cartEventService;
        }

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

                _cartEventService.OnCartItemAdded(existingBasketItem);

            }
            //Ellers bliver en ny entity lavet med productId og userId
            else
            {
                BasketItem newBasketItem = new BasketItem();
                newBasketItem.ProductId = productId;
                newBasketItem.UserId = userId;
                newBasketItem.Quantity = 1;
                await AddObjectAsync(newBasketItem);

                _cartEventService.OnCartItemAdded(existingBasketItem);

            }
        }

        public async Task RemoveItemAsync(int userId, int productId)
        {
            var item = (await GetObjectsAsync()).FirstOrDefault(b => b.ProductId == productId && b.UserId == userId);
            if (item != null)
            {
                await DeleteObjectAsync(item);
            }
        }

        public async Task IncreaseQuantityAsync(int userId, int productId)
        {
            var item = (await GetObjectsAsync())
                .FirstOrDefault(b => b.ProductId == productId && b.UserId == userId);
            if (item != null && item.Quantity < 100)
            {
                item.Quantity++;
                await UpdateObjectAsync(item);
            }
        }

        public async Task DecreaseQuantityAsync(int userId, int productId)
        {
            var item = (await GetObjectsAsync())
                .FirstOrDefault(b => b.ProductId == productId && b.UserId == userId);
            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity <= 0)
                {
                    await DeleteObjectAsync(item);
                }
                else
                {
                    await UpdateObjectAsync(item);
                }
            }
        }

        public async Task<List<BasketItem>> GetUserBasketItemsWithProductsAsync(int userId)
        {
            return await _basketItemRepository.GetUserBasketItemsWithProductsAsync(userId);
        }

        public async Task<CartSummary> GetCartSummaryAsync(int userId)
        {
            var items = await GetUserBasketItemsWithProductsAsync(userId);
            var deliveryPrice = items.Any() ? 29m : 0m;
            var itemsTotal = items.Where(i => i.Product != null).Sum(i => i.Product.Price * i.Quantity);

            return new CartSummary
            {
                Items = items,
                DeliveryPrice = deliveryPrice,
                ItemsTotalPrice = itemsTotal,
                TotalWithDelivery = itemsTotal + deliveryPrice
            };
        }

    }
}
