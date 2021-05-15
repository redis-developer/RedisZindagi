using System.Reflection;

namespace Zindagi
{
    public static class DomainExtensions
    {
        public static Assembly Assembly() => typeof(DomainExtensions).Assembly;
    }
}
