using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Zindagi.Domain.UserAggregate.Queries;
using Zindagi.Domain.UserAggregate.ViewModels;
using Zindagi.SeedWork;

namespace Zindagi.Domain.UserAggregate.QueryHandlers
{
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUser, Result<UserDto>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public GetCurrentUserHandler(ICurrentUser currentUser, IMapper mapper, IUserRepository userRepository)
        {
            _currentUser = currentUser;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<Result<UserDto>> Handle(GetCurrentUser request, CancellationToken cancellationToken)
        {
            var openIdKey = await _currentUser.GetOpenIdKey();

            if (openIdKey.IsSuccess)
            {
                var result = await _userRepository.GetAsync(openIdKey.Value);

                var user = _mapper.Map<UserDto>(result);
                return Result<UserDto>.Success(user);
            }

            return Result<UserDto>.Error(openIdKey.Errors.FirstOrDefault());
        }
    }
}
