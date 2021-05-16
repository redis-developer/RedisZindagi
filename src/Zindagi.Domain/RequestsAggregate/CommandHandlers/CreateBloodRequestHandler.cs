using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MimeKit;
using Zindagi.Domain.Common.Notifications;
using Zindagi.Domain.RequestsAggregate.Commands;
using Zindagi.Domain.RequestsAggregate.ViewModels;
using Zindagi.SeedWork;

namespace Zindagi.Domain.RequestsAggregate.CommandHandlers
{
    public class CreateBloodRequestHandler : IRequestHandler<CreateBloodRequest, Result<BloodRequestDto>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IBloodRequestRepository _bloodRequestRepository;

        public CreateBloodRequestHandler(ICurrentUser currentUser, IMapper mapper, IBloodRequestRepository repository, IMediator mediator)
        {
            _currentUser = currentUser;
            _mapper = mapper;
            _bloodRequestRepository = repository;
            _mediator = mediator;
        }

        public async Task<Result<BloodRequestDto>> Handle(CreateBloodRequest request, CancellationToken cancellationToken)
        {
            var openIdResult = await _currentUser.GetOpenIdUser();
            var result = await _bloodRequestRepository.CreateAsync(BloodRequest.Create(request, openIdResult.Value.Id));

            await _mediator.Publish(new SendEmailNotification(new List<MailboxAddress> { new(openIdResult.Value.NickName, openIdResult.Value.EmailAddress) }, "New Request Created [Blood]", $"Request for blood is created.<br/> Request ID: {result.Id}"), cancellationToken);

            return Result<BloodRequestDto>.Success(_mapper.Map<BloodRequestDto>(result));
        }
    }
}
