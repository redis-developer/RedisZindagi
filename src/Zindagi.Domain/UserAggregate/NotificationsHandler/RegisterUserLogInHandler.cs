using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Zindagi.Domain.UserAggregate.Notifications;
using Zindagi.SeedWork;

namespace Zindagi.Domain.UserAggregate.NotificationsHandler
{
    public class RegisterUserLogInHandler : INotificationHandler<RegisterUserLogIn>
    {
        private readonly ILogger<RegisterUserLogInHandler> _logger;
        private readonly IUserRepository _userRepository;

        public RegisterUserLogInHandler(IUserRepository userRepository, ILogger<RegisterUserLogInHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Handle(RegisterUserLogIn notification, CancellationToken cancellationToken)
        {
            var openIdUser = OpenIdUser.Create(notification.Claims);

            var result = await _userRepository.CreateAsync(User.Create(openIdUser));
            _logger.LogDebug("[User] [Login] {user}", result);
        }
    }
}
