using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Zindagi.SeedWork
{
    public static class Fail
    {
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfNull(object? argumentValue,
            [CallerArgumentExpression("argumentValue")]
            string? argumentName = default,
            string? message = default)
        {
            if (argumentValue == null || (argumentValue is string text && string.IsNullOrWhiteSpace(text)))
                throw new ArgumentNullException(argumentName, message);
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfNull<T>(T? argumentValue,
            [CallerArgumentExpression("argumentValue")]
            string? argumentName = default,
            string? message = default) where T : class
        {
            if (argumentValue == null)
                throw new ArgumentNullException(argumentName, message);
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfNullOrDefault(object? argumentValue,
            [CallerArgumentExpression("argumentValue")]
            string? argumentName = default,
            string? message = default)
        {
            if (argumentValue == null || (argumentValue is string text && string.IsNullOrWhiteSpace(text)))
                throw new ArgumentNullException(argumentName, message);
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfNullOrWhiteSpace(string? argumentValue,
            [CallerArgumentExpression("argumentValue")]
            string? argumentName = default,
            string? message = default)
        {
            if (argumentValue == null) throw new ArgumentNullException(argumentName, message);

            if (string.IsNullOrWhiteSpace(argumentValue)) throw new ArgumentNullException(argumentName, message);
        }
    }
}
