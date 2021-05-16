using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Zindagi.Domain.RequestsAggregate.Notifications;

namespace Zindagi.Domain.RequestsAggregate.NotificationsHandler
{
    public class BloodRequestCreatedHandler : INotificationHandler<BloodRequestCreated>
    {
        private readonly IBloodRequestsSearchRepository _repository;

        public BloodRequestCreatedHandler(IBloodRequestsSearchRepository repository) => _repository = repository;

        public async Task Handle(BloodRequestCreated notification, CancellationToken cancellationToken)
        {
            await _repository.CreateBloodRequestRecord(notification.Request);
        }
    }
}
