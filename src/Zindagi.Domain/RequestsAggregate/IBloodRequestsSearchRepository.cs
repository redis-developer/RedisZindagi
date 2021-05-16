using System.Collections.Generic;
using System.Threading.Tasks;
using Zindagi.Domain.RequestsAggregate.ViewModels;

namespace Zindagi.Domain.RequestsAggregate
{
    public interface IBloodRequestsSearchRepository
    {
        Task CreateBloodRequestRecord(BloodRequest request);
        Task<List<BloodRequestSearchRecordDto>> GetSearchResultsAsync(string searchString);
        Task<List<BloodRequestSearchRecordDto>> GetSearchResultsFromDbAsync(string searchString);
    }
}
