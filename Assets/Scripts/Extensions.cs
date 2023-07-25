#nullable enable

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class Extensions
{
    public static bool IsNull(this object it) => Equals(it, null);
    public static void Use<T>(this T? it, Action<T> func)
    {
        if (it != null)
            func(it);
    }
    public static O? Use<I,O>(this I? it, Func<I,O> func)
    {
        if (it != null)
            return func(it);
        return (O?)(object?)null;
    }
}
