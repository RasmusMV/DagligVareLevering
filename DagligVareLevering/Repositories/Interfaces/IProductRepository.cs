using DagligVareLevering.Models;

namespace DagligVareLevering.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetAllProductsWithStoreAsync();
        Task<List<Product>> GetPopularProductsWithStoreAsync(int count);
    }
}
