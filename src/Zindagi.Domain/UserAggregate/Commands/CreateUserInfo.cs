using MediatR;
using Zindagi.Domain.UserAggregate.ViewModels;
using Zindagi.SeedWork;

namespace Zindagi.Domain.UserAggregate.Commands
{
    public class CreateUserInfo : IRequest<Result<UserDto>> { }
}
