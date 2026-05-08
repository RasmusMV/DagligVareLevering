using DagligVareLevering.Models;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Service
{
    public class UserService : IUserService
    {
        private IService<Models.User> _userService;

        public UserService(IService<Models.User> userService)
        {
            _userService = userService;
        }

        public async Task<int> GetTotalUsers()
        {
            return await _userService.GetAllObjectInfoAsync().Where(x => x.Role == UserRole.Customer).CountAsync();
        }
    }
}
