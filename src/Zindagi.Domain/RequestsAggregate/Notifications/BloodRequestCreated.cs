using MediatR;

namespace Zindagi.Domain.RequestsAggregate.Notifications
{
    public class BloodRequestCreated : INotification
    {
        public BloodRequestCreated(BloodRequest request) => Request = request;

        public BloodRequest Request { get; }
    }
}
