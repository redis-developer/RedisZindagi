using System.Threading.Tasks;
using Zindagi.Domain.UserAggregate.Commands;
using Zindagi.SeedWork;

namespace Zindagi.Domain.UserAggregate
{
    public interface IUserRepository
    {
        Task<User> GetAsync(OpenIdKey openIdKey);
        Task<User?> CreateAsync(User newUser);
        Task<User> UpdateAsync(OpenIdKey openIdKey, UpdateUserInfo userInfo);
        Task<int> DeleteAsync(OpenIdKey openIdKey);
    }
}
