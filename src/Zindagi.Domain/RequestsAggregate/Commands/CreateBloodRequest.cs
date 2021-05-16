using MediatR;
using Zindagi.Domain.RequestsAggregate.ViewModels;
using Zindagi.SeedWork;

namespace Zindagi.Domain.RequestsAggregate.Commands
{
    public class CreateBloodRequest : IRequest<Result<BloodRequestDto>>
    {
        public string PatientName { get; set; } = string.Empty;

        public string Reason { get; set; } = string.Empty;

        public BloodDonationTypeList DonationType { get; set; }

        public BloodGroupList BloodGroup { get; set; }

        public BloodRequestPriorityList Priority { get; set; }

        public double QuantityInUnits { get; set; }
    }
}
