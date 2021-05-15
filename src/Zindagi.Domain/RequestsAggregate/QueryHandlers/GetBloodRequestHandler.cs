using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Zindagi.Domain.RequestsAggregate.Queries;
using Zindagi.Domain.RequestsAggregate.ViewModels;

namespace Zindagi.Domain.RequestsAggregate.QueryHandlers
{
    public class GetBloodRequestHandler : IRequestHandler<GetBloodRequest, BloodRequestDto>
    {
        private readonly IMapper _mapper;
        private readonly IBloodRequestRepository _repository;

        public GetBloodRequestHandler(IBloodRequestRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BloodRequestDto> Handle(GetBloodRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAsync(request.RequestId);

            return _mapper.Map<BloodRequestDto>(result);
        }
    }
}
