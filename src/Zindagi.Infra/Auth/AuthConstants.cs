using Microsoft.AspNetCore.Http;

namespace Zindagi.Infra.Auth
{
    public struct AuthConstants
    {
        public const string AuthenticationScheme = "Auth0";
        public const string ClaimsIssuer = "Auth0";

        public static readonly string CallbackPath = new PathString("/callback");
    }
}
