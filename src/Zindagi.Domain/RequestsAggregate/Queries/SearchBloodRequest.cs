using System.Collections.Generic;
using System.ComponentModel;
using MediatR;
using Zindagi.Domain.RequestsAggregate.ViewModels;

namespace Zindagi.Domain.RequestsAggregate.Queries
{
    public class SearchBloodRequest : IRequest<List<BloodRequestSearchRecordDto>>
    {
        [DisplayName("Search")]
        public string SearchQuery { get; set; } = string.Empty;
    }
}
