using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zindagi.Domain.RequestsAggregate.ViewModels;

namespace Zindagi.Domain.RequestsAggregate
{
    public interface IBloodRequestRepository
    {
        Task<BloodRequest> CreateAsync(BloodRequest request);

        Task<BloodRequest> GetAsync(Guid id);

        Task<List<BloodRequestSearchRecordDto>> SearchRequestsAsync(string searchString);
    }
}
