using DagligVareLevering.Models;

namespace DagligVareLevering.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<int> GetTotalUsersAsync();
    }
}
