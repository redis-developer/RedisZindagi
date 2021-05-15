using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace Zindagi.SeedWork
{
    [Serializable]
    public abstract class ValueObject<T>
        where T : ValueObject<T>
    {
        private int? _cachedHashCode;

        public override bool Equals(object obj)
        {
            if (obj is not T valueObject)
                return false;

            if (ValueObject.GetUnproxiedType(this) != ValueObject.GetUnproxiedType(obj))
                return false;

            return EqualsCore(valueObject);
        }

        protected abstract bool EqualsCore(T other);

        public override int GetHashCode()
        {
            _cachedHashCode ??= GetHashCodeCore();

            return _cachedHashCode.Value;
        }

        protected abstract int GetHashCodeCore();

        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject<T> a, ValueObject<T> b) => !(a == b);
    }

    [Serializable]
    public abstract class ValueObject : IComparable, IComparable<ValueObject>
    {
        private int? _cachedHashCode;

        public virtual int CompareTo(object obj)
        {
            var thisType = GetUnproxiedType(this);
            var otherType = GetUnproxiedType(obj);

            if (thisType != otherType)
                return string.Compare(thisType.ToString(), otherType.ToString(), StringComparison.Ordinal);

            var other = (ValueObject)obj;

            var components = GetEqualityComponents().ToArray();
            var otherComponents = other.GetEqualityComponents().ToArray();

            for (var i = 0; i < components.Length; i++)
            {
                var comparison = CompareComponents(components[i], otherComponents[i]);
                if (comparison != 0)
                    return comparison;
            }

            return 0;
        }

        public virtual int CompareTo(ValueObject other) => CompareTo(other as object);

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (GetUnproxiedType(this) != GetUnproxiedType(obj))
                return false;

            var valueObject = (ValueObject)obj;

            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            if (!_cachedHashCode.HasValue)
            {
                _cachedHashCode = GetEqualityComponents()
                    .Aggregate(1, (current, obj) =>
                    {
                        unchecked
                        {
                            return (current * 23) + (obj?.GetHashCode() ?? 0);
                        }
                    });
            }

            return _cachedHashCode.Value;
        }

        private static int CompareComponents(object object1, object object2)
        {
            if (object1 is null && object2 is null)
                return 0;

            if (object1 is null)
                return -1;

            if (object2 is null)
                return 1;

            if (object1 is IComparable comparable1 && object2 is IComparable comparable2)
                return comparable1.CompareTo(comparable2);

            return object1.Equals(object2) ? 0 : -1;
        }

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b) => !(a == b);

        internal static Type GetUnproxiedType(object obj)
        {
            const string castleProxies = "Castle.Proxies.";
            const string nHibernateProxyPostfix = "Proxy";

            var type = obj.GetType();
            var typeString = type.ToString();

            if (typeString.Contains(castleProxies) || typeString.EndsWith(nHibernateProxyPostfix, StringComparison.InvariantCultureIgnoreCase))
                return type.BaseType;

            return type;
        }
    }
}
