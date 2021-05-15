using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using MimeKit;
using NRediSearch;
using NReJSON;
using StackExchange.Redis;
using Zindagi.Domain;
using Zindagi.Domain.Common.Notifications;
using Zindagi.Domain.RequestsAggregate;
using Zindagi.Domain.RequestsAggregate.ViewModels;
using Zindagi.Domain.UserAggregate;
using Zindagi.SeedWork;

namespace Zindagi.Infra.App.Repositories
{
    public class BloodRequestRepository : IBloodRequestRepository
    {
        private const string SEARCH_SCHEMA = "blood_requests";
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ILogger<UserRepository> _logger;
        private readonly IMediator _mediator;
        private readonly Client _redisSearchClient;
        private readonly IUserRepository _userRepository;

        public BloodRequestRepository(IConnectionMultiplexer connectionMultiplexer, JsonSerializerOptions jsonSerializerOptions, ILogger<UserRepository> logger, IMediator mediator, IUserRepository userRepository)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _jsonSerializerOptions = jsonSerializerOptions;
            _logger = logger;
            _mediator = mediator;
            _userRepository = userRepository;
            _redisSearchClient = new Client(SEARCH_SCHEMA, _connectionMultiplexer.GetDatabase());

            if (IndexExistsAsync(SEARCH_SCHEMA))
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

        public async Task<BloodRequest> CreateAsync(BloodRequest request)
        {
            var db = _connectionMultiplexer.GetDatabase();

            var json = JsonSerializer.Serialize(request, _jsonSerializerOptions);
            var persistenceResult = await db.JsonSetAsync(request.GetPersistenceKey(), json);

            _logger.LogDebug("[User] [INSERT] {json} [{result}]", json, persistenceResult.IsSuccess);

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

            var result = await GetAsync(request.Id);
            var userInfo = await _userRepository.GetAsync(request.OpenIdKey);

            await _mediator.Publish(new SendEmailNotification(new List<MailboxAddress> { new(userInfo.FullName, userInfo.Email) }, "New Request Created [Blood]", $"Request for blood is created.<br/> Request ID: {request.Id}"));
            return result;
        }

        public async Task<BloodRequest> GetAsync(Guid id)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var result = await db.JsonGetAsync<BloodRequest>(GetPersistenceKey(id));
            return result;
        }

        public async Task<List<BloodRequestSearchRecordDto>> SearchRequestsAsync(string searchString)
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

            foreach (var doc in searchResult.Documents)
            {
                var record = doc.GetProperties().ToList();

                var request = new BloodRequestSearchRecordDto
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
                    Status = Enumeration.FromValue<DetailedStatus>(record.FirstOrDefault(p => p.Key == "status").Value.ToString() ?? string.Empty).Name
                };

                requests.Add(request);
            }

            return requests;
        }

        private static string GetPersistenceKey(Guid id) => $"BLOODREQUEST:{id}".ToUpper(CultureInfo.InvariantCulture);

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
    }
}
