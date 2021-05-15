using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Zindagi.Domain.RequestsAggregate.Commands;
using Zindagi.Domain.RequestsAggregate.ViewModels;
using Zindagi.SeedWork;

namespace Zindagi.Domain.RequestsAggregate.CommandHandlers
{
    public class CreateBloodRequestHandler : IRequestHandler<CreateBloodRequest, Result<BloodRequestDto>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly IBloodRequestRepository _repository;

        public CreateBloodRequestHandler(ICurrentUser currentUser, IMapper mapper, IBloodRequestRepository repository)
        {
            _currentUser = currentUser;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<BloodRequestDto>> Handle(CreateBloodRequest request, CancellationToken cancellationToken)
        {
            var openIdResult = await _currentUser.GetOpenIdKey();
            var result = await _repository.CreateAsync(BloodRequest.Create(request, openIdResult.Value));
            return Result<BloodRequestDto>.Success(_mapper.Map<BloodRequestDto>(result));
        }
    }
}
