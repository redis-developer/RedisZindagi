using System.Collections.Generic;
using System.Diagnostics;

namespace Zindagi.SeedWork
{
    [DebuggerDisplay("{GetPersistenceKey()}", Name = "Open ID Key")]
    public class OpenIdKey : ValueObject
    {
        private OpenIdKey(string authId)
        {
            Fail.IfNullOrWhiteSpace(authId);
            Value = authId.Trim().ToUpperInvariant();
        }

        public string Value { get; }

        public static Result<OpenIdKey> Create(string authId)
        {
            if (string.IsNullOrWhiteSpace(authId))
                return Result<OpenIdKey>.Error("OpenID Key should not be empty");

            return Result<OpenIdKey>.Success(new OpenIdKey(authId));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => $"{Value}";

        public string GetPersistenceKey() => $"USER:{Value}";
    }
}
