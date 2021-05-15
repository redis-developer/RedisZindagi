using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

#nullable disable

namespace Zindagi.SeedWork
{
    [DebuggerDisplay("{GetPersistenceKey()}: {EmailAddress} (Verified: {IsEmailVerified})", Name = "Open ID User")]
    public class OpenIdUser
    {
        public OpenIdKey Id => OpenIdKey.Create(NameIdentifier);

        public string NameIdentifier { get; set; }

        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        public string PictureUrl { get; set; }

        public string EmailAddress { get; set; }
        public bool IsEmailVerified { get; set; }

        public static OpenIdUser Create(IReadOnlyList<Claim> claims)
        {
            var result = new OpenIdUser
            {
                NameIdentifier = claims.FirstOrDefault(q => q.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty,
                NickName = claims.FirstOrDefault(q => q.Type.Equals("nickname", StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty,
                EmailAddress = claims.FirstOrDefault(q => q.Type.Equals(ClaimTypes.Email, StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty,
                FirstName = claims.FirstOrDefault(q => q.Type.Equals(ClaimTypes.Name, StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty,
                IsEmailVerified = claims.FirstOrDefault(q => q.Type.Equals("email_verified", StringComparison.OrdinalIgnoreCase))?.Value.ToUpperInvariant().Equals("TRUE", StringComparison.OrdinalIgnoreCase) ?? false,
                PictureUrl = claims.FirstOrDefault(q => q.Type.Equals("picture", StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty
            };
            return result;
        }

        public string GetPersistenceKey() => Id.GetPersistenceKey();
    }
}
