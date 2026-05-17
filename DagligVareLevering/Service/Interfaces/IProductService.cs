using DagligVareLevering.Models;
using DagligVareLevering.Models.DTOs;

namespace DagligVareLevering.Service.Interfaces
{
    public interface IProductService : IService<Product>
    {
        Task CreateProductAsync(ProductDto productDto);
        Task<Dictionary<string, List<Product>>> GetGroupedProductsAsync(decimal? maxPrice, int? storeId);
        Task<List<Product>> SearchProductsAsync(string searchText);
        Task<List<Product>> GetPopularProductsAsync(int count);
        Task<List<Product>> GetPopularProductsWithStoreAsync(int count);
        Task<IEnumerable<Product>> SortById();
        Task<IEnumerable<Product>> SortByIdDescending();
        Task<IEnumerable<Product>> SortByPrice();
        Task<IEnumerable<Product>> SortByName();
        Task<IEnumerable<Product>> NameSearch(string name);
        Task<IEnumerable<Product>> PriceFilter(int maxPrice, int minPrice = 0);

    }
}
