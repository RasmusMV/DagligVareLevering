using DagligVareLevering.Models;

namespace DagligVareLevering.Service
{
    public class ProductService : IProductService
    {
        private readonly IService<Order> _dbService;

        public ProductService(IService<Order> dbService)
        {
            _dbService = dbService;
        }

        public Task<IEnumerable<Product>> NameSearch(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> PriceFilter(int maxPrice, int minPrice = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> SortById()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> SortByIdDescending()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> SortByName()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> SortByPrice()
        {
            throw new NotImplementedException();
        }
    }
}
