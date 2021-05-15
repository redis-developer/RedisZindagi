using System.Reflection;

namespace Zindagi.Application
{
    public static class Extensions
    {
        public static Assembly Assembly() => typeof(Extensions).Assembly;
    }
}
