using DagligVareLevering.Models;
using DagligVareLevering.Models.DTOs;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;

namespace DagligVareLevering.Service
{
    public class ProductService : GenericService<Product>, IProductService
    {

        public ProductService(IRepository<Product> repository) : base(repository){ }

        public async Task CreateProductAsync(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Information = productDto.Information,
                StoreId = productDto.StoreId
            };
            await AddObjectAsync(product);
        }

        public async Task<Dictionary<string, List<Product>>> GetGroupedProductsAsync(decimal? maxPrice, int? storeId)
        {
            var products = await GetObjectsAsync();

            // filtrer før gruppering
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value).ToList();
            }

            if (storeId.HasValue)
            {
                products = products.Where(p => p.StoreId == storeId.Value).ToList();
            }

            return products.OrderBy(p => p.Price)
                .GroupBy(p => p.Name)
                .ToDictionary(g => g.Key, g => g.ToList());

        }

        public async Task<List<Product>> SearchProductsAsync(string searchText)
        {
            return (await GetObjectsAsync())
                .Where(p => p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                    || p.Information.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task<List<Product>> GetPopularProductsAsync(int count)
        {
            return (await GetObjectsAsync()).Take(count).ToList();
        }

        public async Task<IEnumerable<Product>> NameSearch(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await GetObjectsAsync();
            }
            return (await GetObjectsAsync())
                .Where(x => x.Name.ToLower().Contains(name.ToLower()));
        }

        public async Task<IEnumerable<Product>> PriceFilter(int maxPrice, int minPrice = 0)
        {
            return (await GetObjectsAsync())
                .Where(x => (minPrice == 0 && x.Price <= maxPrice) || (maxPrice == 0 && x.Price >= minPrice) || (x.Price >= minPrice && x.Price <= maxPrice));
        }

        public async Task<IEnumerable<Product>> SortById()
        {
            return (await GetObjectsAsync()).OrderBy(x => x.ProductId);
        }

        public async Task<IEnumerable<Product>> SortByIdDescending()
        {
            return (await GetObjectsAsync()).OrderByDescending(x => x.ProductId);
        }

        public async Task<IEnumerable<Product>> SortByName()
        {
            return (await GetObjectsAsync()).OrderBy(x => x.Name);
        }

        public async Task<IEnumerable<Product>> SortByPrice()
        {
            return (await GetObjectsAsync()).OrderBy(x => x.Price);
        }
    }
}
