using System;
using Zindagi.Domain.RequestsAggregate.Commands;
using Zindagi.SeedWork;

#nullable disable

namespace Zindagi.Domain.RequestsAggregate
{
    public class BloodRequest : AggregateBase
    {
        public double QuantityInUnits { get; set; }
        public double QuantityInMl => QuantityInUnits * 450.00;
        public OpenIdKey OpenIdKey { get; set; }
        public string PatientName { get; set; }
        public string Reason { get; set; }
        public BloodDonationTypeList DonationType { get; set; }
        public BloodGroupList BloodGroup { get; set; }
        public BloodRequestPriorityList Priority { get; set; }
        public DetailedStatusList Status { get; set; }
        public void CancelRequest() => Status = DetailedStatusList.Cancelled;
        public bool IsActive() => Equals(Status, DetailedStatusList.Open) || Equals(Status, DetailedStatusList.Assigned);

        public static BloodRequest Create(CreateBloodRequest dto, OpenIdKey openIdKey)
        {
            var result = new BloodRequest
            {
                OpenIdKey = openIdKey,
                BloodGroup = dto.BloodGroup,
                DonationType = dto.DonationType,
                PatientName = dto.PatientName,
                Priority = dto.Priority,
                QuantityInUnits = dto.QuantityInUnits,
                Reason = dto.Reason,
                Status = DetailedStatusList.Open,
                Id = Guid.NewGuid()
            };
            return result;
        }
    }
}
