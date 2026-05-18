using DagligVareLevering.Models;

namespace DagligVareLevering.Service.Interfaces
{
    public interface IUserService : IService<User>
    {
        Task<int> GetTotalUsersAsync();
        Task<User?> LoginAsync(string email, string password);
        Task UpdateOfferEmailsAsync(int userId, bool wantsOfferEmails);
        Task RegisterUserAsync(User user);
    }
}
