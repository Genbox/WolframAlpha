using System;
using System.Collections.Generic;

namespace Genbox.WolframAlpha.Extensions
{
    internal static class EnumExtensions
    {
        internal static IEnumerable<Enum> GetFlags<T>(this T input) where T : Enum
        {
            foreach (Enum value in Enum.GetValues(typeof(T)))
            {
                if (value.Equals(default(T)))
                    continue;

                if (input.HasFlag(value))
                    yield return value;
            }
        }
    }
}