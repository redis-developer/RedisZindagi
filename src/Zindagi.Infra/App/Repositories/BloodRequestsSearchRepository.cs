using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NRediSearch;
using NReJSON;
using StackExchange.Redis;
using Zindagi.Domain;
using Zindagi.Domain.RequestsAggregate;
using Zindagi.Domain.RequestsAggregate.ViewModels;
using Zindagi.Infra.Redis;
using Zindagi.SeedWork;

namespace Zindagi.Infra.App.Repositories
{
    public class BloodRequestsSearchRepository : IBloodRequestsSearchRepository
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ILogger<UserRepository> _logger;
        private readonly Client _redisSearchClient;

        public BloodRequestsSearchRepository(IConnectionMultiplexer connectionMultiplexer, ILogger<UserRepository> logger)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _logger = logger;

            _redisSearchClient = new Client(RedisConstants.BloodRequestsSearchSchema, _connectionMultiplexer.GetDatabase());

            if (IndexExistsAsync(RedisConstants.BloodRequestsSearchSchema))
                return;
            var schema = new Schema();
            schema.AddTextField("requestId", 2)
                .AddTextField("patientName")
                .AddTextField("reason")
                .AddTextField("donationType")
                .AddTextField("bloodGroup")
                .AddTextField("priority", 2)
                .AddNumericField("units");
            _redisSearchClient.CreateIndex(schema, new Client.ConfiguredIndexOptions());
        }

        private bool IndexExistsAsync(string checkIndexName)
        {
            try
            {
                var resultParsed = _redisSearchClient.GetInfoParsed();
                var indexName = resultParsed.IndexName;
                return indexName == checkIndexName;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task CreateBloodRequestRecord(BloodRequest request)
        {
            var fields = new Dictionary<string, RedisValue>
            {
                { "requestId", request.Id.ToString() },
                { "userId", request.OpenIdKey.GetPersistenceKey() },
                { "patientName", request.PatientName },
                { "reason", request.Reason },
                { "donationType", (int)request.DonationType },
                { "bloodGroup", (int)request.BloodGroup },
                { "status", (int)request.Status },
                { "priority", (int)request.Priority },
                { "units", request.QuantityInUnits }
            };
            await _redisSearchClient.AddDocumentAsync(new Document(request.Id.ToString(), fields));
        }

        public async Task<List<BloodRequestSearchRecordDto>> GetSearchResultsFromDbAsync(string searchString)
        {
            var requests = new List<BloodRequestSearchRecordDto>();
            SearchResult? searchResult = null;

            try
            {
                searchResult = await _redisSearchClient.SearchAsync(new Query(searchString) { WithPayloads = true });
                _logger.LogInformation("search result: {info}", searchResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis Search for Blood Requests");
            }

            if (searchResult == null)
                return requests;

            var db = _connectionMultiplexer.GetDatabase();
            var result = await db.JsonMultiGetAsync<BloodRequest>(searchResult.Documents.Select(q => new RedisKey(RedisConstants.GetBloodRequestPersistenceKey(q.Id))).ToArray());
            var filteredResult = result.Where(q => q.Status is DetailedStatusList.Open or DetailedStatusList.Assigned)
                .Select(q => new BloodRequestSearchRecordDto()
                {
                    Status = q.Status.GetDescription(),
                    RequestId = q.Id,
                    PatientName = q.PatientName,
                    Reason = q.Reason,
                    BloodGroup = q.BloodGroup.GetDescription(),
                    DonationType = q.DonationType.GetDescription(),
                    Priority = q.Priority.GetDescription(),
                    QuantityInUnits = q.QuantityInUnits
                });
            requests.AddRange(filteredResult);
            return requests;
        }

        public async Task<List<BloodRequestSearchRecordDto>> GetSearchResultsAsync(string searchString)
        {
            var requests = new List<BloodRequestSearchRecordDto>();
            SearchResult? searchResult = null;

            try
            {
                searchResult = await _redisSearchClient.SearchAsync(new Query(searchString) { WithPayloads = true });
                _logger.LogInformation("search result: {info}", searchResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis Search for Blood Requests");
            }

            if (searchResult == null)
                return requests;

            requests.AddRange(from doc in searchResult.Documents
                              let record = doc.GetProperties().ToList()
                              let status = int.Parse(record.FirstOrDefault(p => p.Key == "status").Value.ToString() ?? string.Empty, CultureInfo.InvariantCulture)
                              where status <= 2
                              select new BloodRequestSearchRecordDto
                              {
                                  SearchId = doc.Id,
                                  SearchScore = doc.Score,
                                  RequestId = Guid.Parse(doc.Id),
                                  PatientName = record.FirstOrDefault(p => p.Key == "patientName").Value.ToString(),
                                  Reason = record.FirstOrDefault(p => p.Key == "reason").Value.ToString(),
                                  QuantityInUnits = double.Parse(record.FirstOrDefault(p => p.Key == "units").Value.ToString(), CultureInfo.InvariantCulture),
                                  DonationType = Enumeration.FromValue<BloodDonationType>(record.FirstOrDefault(p => p.Key == "donationType").Value.ToString()).Name,
                                  BloodGroup = Enumeration.FromValue<BloodGroup>(record.FirstOrDefault(p => p.Key == "bloodGroup").Value.ToString()).Name,
                                  Priority = Enumeration.FromValue<BloodRequestPriority>(record.FirstOrDefault(p => p.Key == "priority").Value.ToString()).Name,
                                  Status = Enumeration.FromValue<DetailedStatus>(status).Name
                              });
            return requests;
        }
    }
}
