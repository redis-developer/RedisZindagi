using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Zindagi.Domain.UserAggregate.Commands;
using Zindagi.Domain.UserAggregate.ViewModels;
using Zindagi.SeedWork;

namespace Zindagi.Domain.UserAggregate.CommandHandlers
{
    public class CreateUserInfoHandler : IRequestHandler<CreateUserInfo, Result<UserDto>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public CreateUserInfoHandler(ICurrentUser currentUser, IUserRepository userRepository, IMapper mapper)
        {
            _currentUser = currentUser;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> Handle(CreateUserInfo request, CancellationToken cancellationToken)
        {
            var openIdUser = await _currentUser.GetOpenIdUser();
            if (openIdUser.IsFailed)
                return Result<UserDto>.Error(openIdUser.Errors.FirstOrDefault());

            var user = await _userRepository.CreateAsync(User.Create(openIdUser.Value));
            return user == null ? Result<UserDto>.Error("Failed to create user") : Result<UserDto>.Success(_mapper.Map<UserDto>(user));
        }
    }
}
