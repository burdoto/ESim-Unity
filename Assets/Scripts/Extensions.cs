#nullable enable

using System;

public static class Extensions
{
    public static bool IsNull(this object it) => Equals(it, null);
    public static O? Use<I,O>(this I? it, Func<I,O> func)
    {
        if (it != null)
            return func(it);
        return (O?)(object?)null;
    }
}
