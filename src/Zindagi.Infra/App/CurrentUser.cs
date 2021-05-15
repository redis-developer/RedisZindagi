using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Zindagi.SeedWork;

namespace Zindagi.Infra.App
{
    public class CurrentUser : ICurrentUser
    {
        private readonly AuthenticationStateProvider _authProvider;

        public CurrentUser(AuthenticationStateProvider authProvider) => _authProvider = authProvider;

        public async Task<Result<OpenIdKey>> GetOpenIdKey()
        {
            var user = (await _authProvider.GetAuthenticationStateAsync()).User;
            return user?.Identity?.IsAuthenticated ?? false ? user.GetOpenIdKey() : Result<OpenIdKey>.Error("User not logged in");
        }

        public async Task<Result<OpenIdUser>> GetOpenIdUser()
        {
            var authState = await _authProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated ?? false)
                return Result<OpenIdUser>.Success(OpenIdUser.Create(user.Claims.ToArray()));

            return Result<OpenIdUser>.Error("User not logged in");
        }
    }
}
