using System;
using System.ComponentModel;

namespace Zindagi
{
    public static class ZindagiSeedWorkExtensions
    {
        public static bool IsNotNullOrWhiteSpace(this string value) => !string.IsNullOrWhiteSpace(value);

        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
                throw new ArgumentException($"{nameof(enumerationValue)} must be of Enum type", nameof(enumerationValue));
            var memberInfo = type.GetMember(enumerationValue.ToString() ?? string.Empty);
            if (memberInfo.Length <= 0)
                return enumerationValue.ToString() ?? string.Empty;

            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs.Length > 0)
                return ((DescriptionAttribute)attrs[0]).Description;
            return enumerationValue.ToString() ?? string.Empty;
        }
    }
}
