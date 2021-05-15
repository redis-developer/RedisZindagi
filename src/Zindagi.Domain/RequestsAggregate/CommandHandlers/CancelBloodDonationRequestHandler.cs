using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Zindagi.Domain.RequestsAggregate.Commands;

namespace Zindagi.Domain.RequestsAggregate.CommandHandlers
{
    public class CancelBloodDonationRequestHandler : IRequestHandler<CancelBloodDonationRequest, bool>
    {
        private readonly IBloodRequestRepository _repository;

        public CancelBloodDonationRequestHandler(IBloodRequestRepository repository) => _repository = repository;

        public async Task<bool> Handle(CancelBloodDonationRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.UpdateRequestStatus(request.RequestId, request.CancelledBy, DetailedStatusList.Cancelled);
            return result;
        }
    }
}
