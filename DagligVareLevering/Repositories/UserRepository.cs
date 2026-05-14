using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Models.Enums;
using DagligVareLevering.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<int> GetTotalUsersAsync()
        {
            return await Query().Where(x => x.Role == UserRole.Customer).CountAsync();
        }
    }
}
