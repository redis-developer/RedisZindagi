using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Zindagi.Domain.UserAggregate.Notifications;

namespace Zindagi.Infra.Auth
{
    public static class DependencyInjection
    {
        public static void AddAuth0(this IServiceCollection services, IConfiguration config)
        {
            var domain = config["Auth0:Domain"];
            var clientId = config["Auth0:ClientId"];
            var clientSecret = config["Auth0:ClientSecret"];

            if (string.IsNullOrWhiteSpace(domain) || string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                throw new MissingFieldException("Auth0 config missing, please check.");

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options => options.Cookie.Name = "zg.auth")
                .AddOpenIdConnect(AuthConstants.AuthenticationScheme, options =>
                {
                    options.Authority = $"https://{domain}";
                    options.ClientId = clientId;
                    options.ClientSecret = clientSecret;

                    options.ResponseType = OpenIdConnectResponseType.Code;

                    options.CorrelationCookie.Name = "zg.correlation.";
                    options.NonceCookie.Name = "zg.nonce.";

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");

                    options.SaveTokens = true;

                    options.CallbackPath = AuthConstants.CallbackPath;

                    options.ClaimsIssuer = AuthConstants.ClaimsIssuer;

                    options.Events = new OpenIdConnectEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var builder = services.BuildServiceProvider();
                            var mediator = builder.GetService<IMediator>();
                            mediator?.Publish(new RegisterUserLogIn(context.Principal?.Claims.ToList() ?? new List<Claim>()));

                            return Task.CompletedTask;
                        },
                        OnRedirectToIdentityProviderForSignOut = context =>
                        {
                            var logoutUri = $"https://{config["Auth0:Domain"]}/v2/logout?client_id={config["Auth0:ClientId"]}";

                            var postLogoutUri = context.Properties.RedirectUri;
                            if (!string.IsNullOrEmpty(postLogoutUri))
                            {
                                if (postLogoutUri.StartsWith("/", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    var request = context.Request;
                                    postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                                }

                                logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                            }

                            context.Response.Redirect(logoutUri);
                            context.HandleResponse();

                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}
