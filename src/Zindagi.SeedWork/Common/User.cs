using System.Threading.Tasks;

namespace Zindagi.SeedWork
{
    public interface ICurrentUser
    {
        Task<Result<OpenIdKey>> GetOpenIdKey();
        Task<Result<OpenIdUser>> GetOpenIdUser();
    }
}
