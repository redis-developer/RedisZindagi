using System.Collections.Generic;
using Zindagi.SeedWork;

namespace Zindagi.Domain
{
    public class MobileNumber : ValueObject
    {
        public const string CountryCodeIndia = "+91";

        private MobileNumber(string number, string countryCode)
        {
            Number = number;
            CountryCode = countryCode;
        }

        public string CountryCode { get; }
        public string Number { get; }

        public static Result<MobileNumber> Create(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return Result<MobileNumber>.Error("Mobile Number should not be empty");

            number = number.Trim().ToUpperInvariant();

            if (number.Length >= 12)
                return Result<MobileNumber>.Error("Mobile Number has invalid digits");

            return Result<MobileNumber>.Success(new MobileNumber(number, CountryCodeIndia));
        }

        public static implicit operator string(MobileNumber value) => $"{value?.CountryCode}-{value?.Number}";

        public override string ToString() => $"{CountryCode}{Number}";

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CountryCode;
            yield return Number;
        }
    }

    public class FullName : ValueObject
    {
        private FullName(string value) => Value = value;

        public string Value { get; set; }

        public static Result<FullName> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return Result<FullName>.Error("Full Name cannot be empty");

            value = value.Trim();

            if (value.Length > 200)
                return Result<FullName>.Error("Full Name is too long");

            return Result<FullName>.Success(new FullName(value));
        }

        public static implicit operator string(FullName input) => input?.Value ?? string.Empty;

        public override string ToString() => Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
