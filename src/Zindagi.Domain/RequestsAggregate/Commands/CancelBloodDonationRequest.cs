using System;
using MediatR;
using Zindagi.SeedWork;

namespace Zindagi.Domain.RequestsAggregate.Commands
{
    public class CancelBloodDonationRequest : IRequest<bool>
    {
        public CancelBloodDonationRequest(Guid requestId, OpenIdKey cancelledBy)
        {
            RequestId = requestId;
            CancelledBy = cancelledBy;
        }
        public Guid RequestId { get; set; }
        public OpenIdKey CancelledBy { get; set; }
    }
}
