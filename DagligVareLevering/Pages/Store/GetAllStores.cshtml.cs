using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.Store
{
    public class GetAllStoresModel : PageModel
    {
        private IService<Models.Store> _dbService;

        public GetAllStoresModel(IService<Models.Store> dbService)
        {
            _dbService = dbService;
        }

        public List<Models.Store> Stores { get; private set; }

        public async Task OnGetAsync()
        {
            Stores = (await _dbService.GetObjectsAsync()).ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            Models.Store store = await _dbService.GetObjectByIdAsync(id);
            await _dbService.DeleteObjectAsync(store);
            return RedirectToPage("GetAllStores");
        }
    }
}
