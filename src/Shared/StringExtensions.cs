using System.Runtime.CompilerServices;
namespace Shared;

public static class StringExtensions
{
    extension(string? str) 
    {
        public bool IsBlank()
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }

        public bool IsNotBlank()
        {
            return !str.IsBlank();
        }
    }
}


public static class BlankCheckExtensions
{
    private static void ThrowIfBlank(string? value, string? paramName)
    {
        if (value.IsBlank())
            throw new ArgumentException("Value cannot be null, empty, or whitespace.", paramName);
    }

    [OverloadResolutionPriority(2)]
    public static void ThrowIfAnyBlank(
        string? a, string? b,
        [CallerArgumentExpression(nameof(a))] string? aName = null,
        [CallerArgumentExpression(nameof(b))] string? bName = null)
    {
        ThrowIfBlank(a, aName);
        ThrowIfBlank(b, bName);
    }

    [OverloadResolutionPriority(3)]
    public static void ThrowIfAnyBlank(
        string? a, string? b, string? c,
        [CallerArgumentExpression(nameof(a))] string? aName = null,
        [CallerArgumentExpression(nameof(b))] string? bName = null,
        [CallerArgumentExpression(nameof(c))] string? cName = null)
    {
        ThrowIfBlank(a, aName);
        ThrowIfBlank(b, bName);
        ThrowIfBlank(c, cName);
    }

    [OverloadResolutionPriority(4)]
    public static void ThrowIfAnyBlank(
        string? a, string? b, string? c, string? d,
        [CallerArgumentExpression(nameof(a))] string? aName = null,
        [CallerArgumentExpression(nameof(b))] string? bName = null,
        [CallerArgumentExpression(nameof(c))] string? cName = null,
        [CallerArgumentExpression(nameof(d))] string? dName = null)
    {
        ThrowIfBlank(a, aName);
        ThrowIfBlank(b, bName);
        ThrowIfBlank(c, cName);
        ThrowIfBlank(d, dName);
    }

    [OverloadResolutionPriority(5)]
    public static void ThrowIfAnyBlank(
        string? a, string? b, string? c, string? d, string? e,
        [CallerArgumentExpression(nameof(a))] string? aName = null,
        [CallerArgumentExpression(nameof(b))] string? bName = null,
        [CallerArgumentExpression(nameof(c))] string? cName = null,
        [CallerArgumentExpression(nameof(d))] string? dName = null,
        [CallerArgumentExpression(nameof(e))] string? eName = null)
    {
        ThrowIfBlank(a, aName);
        ThrowIfBlank(b, bName);
        ThrowIfBlank(c, cName);
        ThrowIfBlank(d, dName);
        ThrowIfBlank(e, eName);
    }

    [OverloadResolutionPriority(6)]
    public static void ThrowIfAnyBlank(
        string? a, string? b, string? c, string? d, string? e, string? f,
        [CallerArgumentExpression(nameof(a))] string? aName = null,
        [CallerArgumentExpression(nameof(b))] string? bName = null,
        [CallerArgumentExpression(nameof(c))] string? cName = null,
        [CallerArgumentExpression(nameof(d))] string? dName = null,
        [CallerArgumentExpression(nameof(e))] string? eName = null,
        [CallerArgumentExpression(nameof(f))] string? fName = null)
    {
        ThrowIfBlank(a, aName);
        ThrowIfBlank(b, bName);
        ThrowIfBlank(c, cName);
        ThrowIfBlank(d, dName);
        ThrowIfBlank(e, eName);
        ThrowIfBlank(f, fName);
    }

    [OverloadResolutionPriority(7)]
    public static void ThrowIfAnyBlank(
        string? a, string? b, string? c, string? d, string? e, string? f, string? g,
        [CallerArgumentExpression(nameof(a))] string? aName = null,
        [CallerArgumentExpression(nameof(b))] string? bName = null,
        [CallerArgumentExpression(nameof(c))] string? cName = null,
        [CallerArgumentExpression(nameof(d))] string? dName = null,
        [CallerArgumentExpression(nameof(e))] string? eName = null,
        [CallerArgumentExpression(nameof(f))] string? fName = null,
        [CallerArgumentExpression(nameof(g))] string? gName = null)
    {
        ThrowIfBlank(a, aName);
        ThrowIfBlank(b, bName);
        ThrowIfBlank(c, cName);
        ThrowIfBlank(d, dName);
        ThrowIfBlank(e, eName);
        ThrowIfBlank(f, fName);
        ThrowIfBlank(g, gName);
    }
}