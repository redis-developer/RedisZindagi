using System;

namespace Zindagi.Domain.RequestsAggregate.ViewModels
{
    public class BloodRequestDto
    {
        public Guid Id { get; set; }
        public string PatientName { get; set; } = string.Empty;

        public string Reason { get; set; } = string.Empty;

        public BloodDonationTypeList DonationType { get; set; }

        public BloodGroupList BloodGroup { get; set; }

        public DetailedStatusList Status { get; set; }

        public BloodRequestPriorityList Priority { get; set; }

        public double QuantityInUnits { get; set; }

        public double QuantityInMl => QuantityInUnits * 450.00;
    }
}
