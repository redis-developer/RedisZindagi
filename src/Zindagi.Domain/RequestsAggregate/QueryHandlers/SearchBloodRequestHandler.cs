using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Zindagi.Domain.RequestsAggregate.Queries;
using Zindagi.Domain.RequestsAggregate.ViewModels;

namespace Zindagi.Domain.RequestsAggregate.QueryHandlers
{
    public class SearchBloodRequestHandler : IRequestHandler<SearchBloodRequest, List<BloodRequestSearchRecordDto>>
    {
        private readonly IBloodRequestRepository _repository;

        public SearchBloodRequestHandler(IBloodRequestRepository repository) => _repository = repository;

        public async Task<List<BloodRequestSearchRecordDto>> Handle(SearchBloodRequest request, CancellationToken cancellationToken) => await _repository.SearchRequestsAsync(request.SearchQuery);
    }
}
