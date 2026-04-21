using DagligVareLevering.Models;

namespace DagligVareLevering.Service
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> SortById();
        Task<IEnumerable<Product>> SortByIdDescending();
        Task<IEnumerable<Product>> SortByPrice();
        Task<IEnumerable<Product>> SortByName();
        Task<IEnumerable<Product>> NameSearch(string name);
        Task<IEnumerable<Product>> PriceFilter(int maxPrice, int minPrice = 0);

    }
}
