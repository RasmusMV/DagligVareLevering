using DagligVareLevering.Models;
using DagligVareLevering.Models.DTOs;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;

namespace DagligVareLevering.Service
{
    public class ProductService : GenericService<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository) : base(productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task CreateProductAsync(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Information = productDto.Information,
                StoreId = productDto.StoreId
            };
            await _productRepository.AddObjectAsync(product);
        }

        public async Task<Dictionary<string, List<Product>>> GetGroupedProductsAsync(decimal? maxPrice, int? storeId)
        {
            var products = await _productRepository.GetObjectsAsync();

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
            return (await _productRepository.GetObjectsAsync())
                .Where(p => p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                    || p.Information.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task<List<Product>> GetPopularProductsAsync(int count)
        {
            return (await _productRepository.GetObjectsAsync()).Take(count).ToList();
        }
        public async Task<List<Product>> GetPopularProductsWithStoreAsync(int count)
        {
            return await _productRepository.GetPopularProductsWithStoreAsync(count);
        }

        public async Task<IEnumerable<Product>> NameSearch(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await _productRepository.GetObjectsAsync();
            }
            return (await _productRepository.GetObjectsAsync())
                .Where(x => x.Name.ToLower().Contains(name.ToLower()));
        }

        public async Task<IEnumerable<Product>> PriceFilter(int maxPrice, int minPrice = 0)
        {
            return (await _productRepository.GetObjectsAsync())
                .Where(x => (minPrice == 0 && x.Price <= maxPrice) || (maxPrice == 0 && x.Price >= minPrice) || (x.Price >= minPrice && x.Price <= maxPrice));
        }

        public async Task<IEnumerable<Product>> SortById()
        {
            return (await _productRepository.GetObjectsAsync()).OrderBy(x => x.ProductId);
        }

        public async Task<IEnumerable<Product>> SortByIdDescending()
        {
            return (await _productRepository.GetObjectsAsync()).OrderByDescending(x => x.ProductId);
        }

        public async Task<IEnumerable<Product>> SortByName()
        {
            return (await _productRepository.GetObjectsAsync()).OrderBy(x => x.Name);
        }

        public async Task<IEnumerable<Product>> SortByPrice()
        {
            return (await _productRepository.GetObjectsAsync()).OrderBy(x => x.Price);
        }
    }
}
