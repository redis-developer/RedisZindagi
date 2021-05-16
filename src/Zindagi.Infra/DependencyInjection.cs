using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NReJSON;
using StackExchange.Redis;
using Zindagi.Domain.RequestsAggregate;
using Zindagi.Domain.UserAggregate;
using Zindagi.Infra.App;
using Zindagi.Infra.App.Repositories;
using Zindagi.Infra.BackgroundJobs;
using Zindagi.Infra.JsonConverters;
using Zindagi.Infra.Options;
using Zindagi.Infra.Redis;
using Zindagi.SeedWork;

namespace Zindagi.Infra
{
    public static class DependencyInjection
    {
        public static void AddInfra(this IServiceCollection services, IConfiguration config)
        {
            var redisOptions = ConfigurationOptions.Parse(config.GetConnectionString("redis"));
            services.AddSingleton(redisOptions);
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisOptions));

            services.AddSignalR().AddStackExchangeRedis(config.GetConnectionString("redis"));

            var jsonOptions = new JsonSerializerOptions
            {
                AllowTrailingCommas = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                ReadCommentHandling = JsonCommentHandling.Skip
            };
            jsonOptions.Converters.Add(new OpenIdKeyConverter());
            jsonOptions.Converters.Add(new StatusConverter());
            jsonOptions.Converters.Add(new BloodGroupConverter());

            services.AddSingleton(jsonOptions);

            var smtpOptions = new SmtpOptions();
            var smsOptions = new SmsOptions();
            config.Bind("smtp", smtpOptions);
            config.Bind("sms", smsOptions);

            services.AddSingleton(smtpOptions);
            services.AddSingleton(smsOptions);

            NReJSONSerializer.SerializerProxy = new ReJsonSerializerProxy(jsonOptions);

            services.AddHostedService<NewRequestProcessingService>();

            // Singleton: creates a new instance only once during the application lifetime
            // Scoped: creates a new instance for every request
            // Transient: creates a new instance every time you request it

            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddSingleton<IMessaging, Messaging>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IBloodRequestRepository, BloodRequestRepository>();

            services.AddTransient<IBloodRequestsSearchRepository, BloodRequestsSearchRepository>();
        }
    }
}
