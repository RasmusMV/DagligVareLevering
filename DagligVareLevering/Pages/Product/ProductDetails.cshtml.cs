using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.Product
{
    public class ProductDetailsModel : PageModel
    {
        private readonly IProductService _productService;

        public ProductDetailsModel(IProductService productService)
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