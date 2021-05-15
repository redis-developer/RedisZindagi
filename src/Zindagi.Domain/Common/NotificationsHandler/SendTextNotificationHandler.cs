using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Zindagi.Domain.Common.Notifications;
using Zindagi.SeedWork;

namespace Zindagi.Domain.Common.NotificationsHandler
{
    public class SendTextNotificationHandler : INotificationHandler<SendTextNotification>
    {
        private readonly IMessaging _messaging;

        public SendTextNotificationHandler(IMessaging messaging) => _messaging = messaging;

        public async Task Handle(SendTextNotification notification, CancellationToken cancellationToken) =>
            await _messaging.SendText(notification.MobileNumber, notification.Message);
    }
}
