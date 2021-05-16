using System;
using System.Threading.Tasks;
using Zindagi.SeedWork;

namespace Zindagi.Domain.RequestsAggregate
{
    public interface IBloodRequestRepository
    {
        Task<BloodRequest> CreateAsync(BloodRequest request);

        Task<BloodRequest> GetAsync(Guid id);

        Task<bool> UpdateRequestStatus(Guid id, OpenIdKey openIdKey, DetailedStatusList status);
    }
}
