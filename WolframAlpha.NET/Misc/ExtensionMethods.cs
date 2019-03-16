using System.Collections.Generic;

namespace WolframAlpha.Misc
{
    public static class ExtensionMethods
    {
        public static bool HasElements<T>(this List<T> list)
        {
            return (list != null && list.Count >= 1);
        }
    }
}
