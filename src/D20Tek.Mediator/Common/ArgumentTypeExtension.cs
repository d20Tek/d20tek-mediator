using System.Runtime.CompilerServices;

namespace D20Tek.Mediator.Common;

internal static class ArgumentTypeExtension
{
    public static void ThrowIfNotAssignableTo<TBase>(
        Type type,
        [CallerArgumentExpression(nameof(type))] string? paramName = "none")
    {
        ArgumentNullException.ThrowIfNull(type, paramName);
        if (!typeof(TBase).IsAssignableFrom(type))
            ThrowNotAssignableTo(type.FullName, typeof(TBase).FullName, paramName, Argument_TypeNotAssignable);
    }

    public const string Argument_TypeNotAssignable = "Parameter {0} with type '{1}' must be assignable to '{2}'.";

    [DoesNotReturn]
    private static void ThrowNotAssignableTo(
        string? typeName, string? assignableTo, string? paramName, string messageFormat) =>
        throw new ArgumentException(string.Format(messageFormat, paramName, typeName, assignableTo), paramName);
}