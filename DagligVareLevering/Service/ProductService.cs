using DagligVareLevering.Models;

namespace DagligVareLevering.Service
{
    public class ProductService : IProductService
    {
        private readonly IService<Product> _dbService;

        public ProductService(IService<Product> dbService)
        {
            _dbService = dbService;
        }

        public async Task<IEnumerable<Product>> NameSearch(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await _dbService.GetObjectsAsync();
            }
            return (await _dbService.GetObjectsAsync())
                .Where(x => x.Name.ToLower().Contains(name.ToLower()));
        }

        public async Task<IEnumerable<Product>> PriceFilter(int maxPrice, int minPrice = 0)
        {
            return (await _dbService.GetObjectsAsync()).Where(x => (minPrice == 0 && x.Price <= maxPrice) || (maxPrice == 0 && x.Price >= minPrice) || (x.Price >= minPrice && x.Price <= maxPrice));
        }

        public async Task<IEnumerable<Product>> SortById()
        {
            return (await _dbService.GetObjectsAsync()).OrderBy(x => x.ProductId);
        }

        public async Task<IEnumerable<Product>> SortByIdDescending()
        {
            return (await _dbService.GetObjectsAsync()).OrderByDescending(x => x.ProductId);
        }

        public async Task<IEnumerable<Product>> SortByName()
        {
            return (await _dbService.GetObjectsAsync()).OrderBy(x => x.Name);
        }

        public async Task<IEnumerable<Product>> SortByPrice()
        {
            return (await _dbService.GetObjectsAsync()).OrderBy(x => x.Price);
        }
    }
}
