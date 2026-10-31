using System.Runtime.CompilerServices;

namespace Shared;

public static class NullCheckExtensions
{
    public static void ThrowIfAnyNull(
        object? a, object? b,
        [CallerArgumentExpression(nameof(a))] string? aName = null,
        [CallerArgumentExpression(nameof(b))] string? bName = null)
    {
        ArgumentNullException.ThrowIfNull(a, aName);
        ArgumentNullException.ThrowIfNull(b, bName);
    }

    public static void ThrowIfAnyNull(
        object? a, object? b, object? c,
        [CallerArgumentExpression(nameof(a))] string? aName = null,
        [CallerArgumentExpression(nameof(b))] string? bName = null,
        [CallerArgumentExpression(nameof(c))] string? cName = null)
    {
        ArgumentNullException.ThrowIfNull(a, aName);
        ArgumentNullException.ThrowIfNull(b, bName);
        ArgumentNullException.ThrowIfNull(c, cName);
    }

    public static void ThrowIfAnyNull(
        object? a, object? b, object? c, object? d,
        [CallerArgumentExpression(nameof(a))] string? aName = null,
        [CallerArgumentExpression(nameof(b))] string? bName = null,
        [CallerArgumentExpression(nameof(c))] string? cName = null,
        [CallerArgumentExpression(nameof(d))] string? dName = null)
    {
        ArgumentNullException.ThrowIfNull(a, aName);
        ArgumentNullException.ThrowIfNull(b, bName);
        ArgumentNullException.ThrowIfNull(c, cName);
        ArgumentNullException.ThrowIfNull(d, dName);
    }

    public static void ThrowIfAnyNull(
        object? a, object? b, object? c, object? d, object? e,
        [CallerArgumentExpression(nameof(a))] string? aName = null,
        [CallerArgumentExpression(nameof(b))] string? bName = null,
        [CallerArgumentExpression(nameof(c))] string? cName = null,
        [CallerArgumentExpression(nameof(d))] string? dName = null,
        [CallerArgumentExpression(nameof(e))] string? eName = null)
    {
        ArgumentNullException.ThrowIfNull(a, aName);
        ArgumentNullException.ThrowIfNull(b, bName);
        ArgumentNullException.ThrowIfNull(c, cName);
        ArgumentNullException.ThrowIfNull(d, dName);
        ArgumentNullException.ThrowIfNull(e, eName);
    }

    public static void ThrowIfAnyNull(
        object? a, object? b, object? c, object? d, object? e, object? f,
        [CallerArgumentExpression(nameof(a))] string? aName = null,
        [CallerArgumentExpression(nameof(b))] string? bName = null,
        [CallerArgumentExpression(nameof(c))] string? cName = null,
        [CallerArgumentExpression(nameof(d))] string? dName = null,
        [CallerArgumentExpression(nameof(e))] string? eName = null,
        [CallerArgumentExpression(nameof(f))] string? fName = null)
    {
        ArgumentNullException.ThrowIfNull(a, aName);
        ArgumentNullException.ThrowIfNull(b, bName);
        ArgumentNullException.ThrowIfNull(c, cName);
        ArgumentNullException.ThrowIfNull(d, dName);
        ArgumentNullException.ThrowIfNull(e, eName);
        ArgumentNullException.ThrowIfNull(f, fName);
    }

    public static void ThrowIfAnyNull(
        object? a, object? b, object? c, object? d, object? e, object? f, object? g,
        [CallerArgumentExpression(nameof(a))] string? aName = null,
        [CallerArgumentExpression(nameof(b))] string? bName = null,
        [CallerArgumentExpression(nameof(c))] string? cName = null,
        [CallerArgumentExpression(nameof(d))] string? dName = null,
        [CallerArgumentExpression(nameof(e))] string? eName = null,
        [CallerArgumentExpression(nameof(f))] string? fName = null,
        [CallerArgumentExpression(nameof(g))] string? gName = null)
    {
        ArgumentNullException.ThrowIfNull(a, aName);
        ArgumentNullException.ThrowIfNull(b, bName);
        ArgumentNullException.ThrowIfNull(c, cName);
        ArgumentNullException.ThrowIfNull(d, dName);
        ArgumentNullException.ThrowIfNull(e, eName);
        ArgumentNullException.ThrowIfNull(f, fName);
        ArgumentNullException.ThrowIfNull(g, gName);
    }
}