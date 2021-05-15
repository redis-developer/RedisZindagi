using System;
using MediatR;
using Zindagi.SeedWork;

namespace Zindagi.Domain.RequestsAggregate.Commands
{
    public class AcceptBloodDonationRequest : IRequest<bool>
    {
        public AcceptBloodDonationRequest(Guid requestId, OpenIdKey openIdKey)
        {
            RequestId = requestId;
            OpenIdKey = openIdKey;
        }

        public Guid RequestId { get; set; }
        public OpenIdKey OpenIdKey { get; set; }
    }
}
