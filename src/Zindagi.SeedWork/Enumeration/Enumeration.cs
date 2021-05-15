using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable

namespace Zindagi.SeedWork
{
    public abstract class Enumeration : IComparable
    {
        protected Enumeration(int id, string name) => (Id, Name) = (id, name);
        public string Name { get; }

        public int Id { get; }

        public int CompareTo(object obj) => obj == null ? 0 : Id.CompareTo(((Enumeration)obj).Id);

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<T>();

        public override bool Equals(object obj)
        {
            if (obj is not Enumeration otherValue)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
            return absoluteDifference;
        }

        public static T FromValue<T>(int value) where T : Enumeration
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.Id == value);
            return matchingItem;
        }

        public static T FromValue<T>(string value) where T : Enumeration
        {
            var isInt = int.TryParse(value, out var parsedInt);
            if (!isInt)
                return FromValue<T>(0);

            var matchingItem = Parse<T, int>(parsedInt, "value", item => item.Id == parsedInt);
            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
            return matchingItem;
        }

        private static T Parse<T, TK>(TK value, string description, Func<T, bool> predicate) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

            return matchingItem;
        }

        public static KeyValuePair<int, string>[] GetKeyValues<T>() where T : Enumeration => GetAll<T>().Select(q => new KeyValuePair<int, string>(q.Id, q.Name)).ToArray();

        public static bool operator ==(Enumeration left, Enumeration right)
        {
            if (left is null) return right is null;

            return left.Equals(right);
        }

        public static bool operator !=(Enumeration left, Enumeration right) => !(left == right);

        public static bool operator <(Enumeration left, Enumeration right) => left is null ? right is not null : left.CompareTo(right) < 0;

        public static bool operator <=(Enumeration left, Enumeration right) => left is null || left.CompareTo(right) <= 0;

        public static bool operator >(Enumeration left, Enumeration right) => left is not null && left.CompareTo(right) > 0;

        public static bool operator >=(Enumeration left, Enumeration right) => left is null ? right is null : left.CompareTo(right) >= 0;
    }
}
