using DagligVareLevering.Models;

namespace DagligVareLevering.Service.Interfaces
{
    public interface IUserService : IService<User>
    {
        Task<int> GetTotalUsersAsync();
    }
}
