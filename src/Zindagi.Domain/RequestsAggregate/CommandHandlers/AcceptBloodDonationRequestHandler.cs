using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Zindagi.Domain.RequestsAggregate.Commands;

namespace Zindagi.Domain.RequestsAggregate.CommandHandlers
{
    public class AcceptBloodDonationRequestHandler : IRequestHandler<AcceptBloodDonationRequest, bool>
    {
        private readonly IBloodRequestRepository _repository;

        public AcceptBloodDonationRequestHandler(IBloodRequestRepository repository) => _repository = repository;

        public async Task<bool> Handle(AcceptBloodDonationRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.UpdateRequestStatus(request.RequestId, request.OpenIdKey, DetailedStatusList.Assigned);
            return result;
        }
    }
}
