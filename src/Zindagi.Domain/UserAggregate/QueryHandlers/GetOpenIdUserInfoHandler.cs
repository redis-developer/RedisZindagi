using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Zindagi.Domain.UserAggregate.Queries;
using Zindagi.Domain.UserAggregate.ViewModels;
using Zindagi.SeedWork;

namespace Zindagi.Domain.UserAggregate.QueryHandlers
{
    public class GetOpenIdUserInfoHandler : IRequestHandler<GetOpenIdUserInfo, Result<UserDto>>
    {
        private readonly ICurrentUser _currentUser;

        public GetOpenIdUserInfoHandler(ICurrentUser currentUser) => _currentUser = currentUser;

        public async Task<Result<UserDto>> Handle(GetOpenIdUserInfo request, CancellationToken cancellationToken)
        {
            var result = await _currentUser.GetOpenIdUser();

            if (result.IsSuccess)
            {
                var openIdUser = result.Value;
                var user = new UserDto { AlternateId = openIdUser.Id.Value, Email = openIdUser.EmailAddress, IsEmailVerified = openIdUser.IsEmailVerified };
                return Result<UserDto>.Success(user);
            }

            return Result<UserDto>.Error(result.Errors.FirstOrDefault());
        }
    }
}
