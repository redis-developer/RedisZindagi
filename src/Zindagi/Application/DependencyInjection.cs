using System.Globalization;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zindagi.Infra.Auth;
using Zindagi.Infra.Behaviors;

namespace Zindagi.Application
{
    public static class DependencyInjection
    {
        public static void AddApp(this IServiceCollection services, IConfiguration config)
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.ConsentCookie.IsEssential = true;
                options.Secure = CookieSecurePolicy.Always;
                options.HttpOnly = HttpOnlyPolicy.Always;
            });

            services.Configure<KestrelServerOptions>(options => options.AddServerHeader = false);

            services.AddAutoMapper(DomainExtensions.Assembly(), InfraExtensions.Assembly(), Extensions.Assembly());

            services.AddMediatR(DomainExtensions.Assembly(), InfraExtensions.Assembly(), Extensions.Assembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));

            services.AddValidatorsFromAssemblies(new[] { DomainExtensions.Assembly(), InfraExtensions.Assembly(), Extensions.Assembly() });

            services.AddAuth0(config);
        }
    }
}
