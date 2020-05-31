using System.Collections.Generic;

namespace Genbox.WolframAlpha.Extensions
{
    internal static class ListExtensions
    {
        internal static bool HasElements<T>(this IList<T> list)
        {
            return list != null && list.Count >= 1;
        }
    }
}