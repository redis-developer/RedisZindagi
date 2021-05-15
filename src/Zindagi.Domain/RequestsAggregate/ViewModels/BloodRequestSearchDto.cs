using System;

#nullable disable

namespace Zindagi.Domain.RequestsAggregate.ViewModels
{
    public class BloodRequestSearchRecordDto
    {
        public string SearchId { get; set; }
        public double SearchScore { get; set; }
        public Guid RequestId { get; set; }
        public string PatientName { get; set; }

        public string Reason { get; set; }

        public string DonationType { get; set; }

        public string BloodGroup { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public double QuantityInUnits { get; set; }
    }
}
