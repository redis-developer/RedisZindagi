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
    public class UpdateUserInfoHandler : IRequestHandler<UpdateUserInfo, Result<UserDto>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UpdateUserInfoHandler(ICurrentUser currentUser, IUserRepository userRepository, IMapper mapper)
        {
            _currentUser = currentUser;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> Handle(UpdateUserInfo request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUser.GetOpenIdUser();
            if (currentUser.IsFailed)
                return Result<UserDto>.Error(currentUser.Errors.FirstOrDefault());

            var user = await _userRepository.UpdateAsync(currentUser.Value.Id, request);
            return Result<UserDto>.Success(_mapper.Map<UserDto>(user));
        }
    }
}
