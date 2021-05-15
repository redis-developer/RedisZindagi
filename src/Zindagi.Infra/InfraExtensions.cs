using System.Reflection;

namespace Zindagi
{
    public static class InfraExtensions
    {
        public static Assembly Assembly() => typeof(InfraExtensions).Assembly;
    }
}
