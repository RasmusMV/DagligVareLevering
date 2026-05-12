using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.Product
{
    public class ProductDetailsModel : PageModel
    {
        private readonly IService<Models.Product> _productService;

        public ProductDetailsModel(IService<Models.Product> productService)
        {
            _productService = productService;
        }

        public Models.Product? Product { get; set; }

        public async Task OnGetAsync(int id)
        {
            var products = await _productService.GetObjectsAsync();
            Product = products.FirstOrDefault(p => p.ProductId == id);
        }
    }
}