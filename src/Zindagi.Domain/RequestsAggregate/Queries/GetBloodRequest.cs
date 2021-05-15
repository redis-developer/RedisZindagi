using System;
using MediatR;
using Zindagi.Domain.RequestsAggregate.ViewModels;

namespace Zindagi.Domain.RequestsAggregate.Queries
{
    public class GetBloodRequest : IRequest<BloodRequestDto>
    {
        public Guid RequestId { get; set; }
    }
}
