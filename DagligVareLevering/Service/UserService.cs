using DagligVareLevering.Models;
using DagligVareLevering.Models.Enums;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;

namespace DagligVareLevering.Service
{
    public class UserService : GenericService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) : base(userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _userRepository.GetTotalUsersAsync();
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            return (await GetObjectsAsync())
                .FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public async Task UpdateOfferEmailsAsync(int userId, bool wantsOfferEmails)
        {
            var user = await GetObjectByIdAsync(userId);

            user.WantsOfferEmails = wantsOfferEmails;
            await UpdateObjectAsync(user);
        }

        public async Task RegisterUserAsync(User user)
        {
            user.Role = UserRole.Customer;
            await AddObjectAsync(user);
        }
    }
}
