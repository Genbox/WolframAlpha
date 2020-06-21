using System.Linq;
using Genbox.WolframAlpha.Objects;
using Genbox.WolframAlpha.Responses;

namespace Genbox.WolframAlpha.Extensions
{
    public static class QueryResponseExtensions
    {
        /// <summary>Gets the primary pod</summary>
        public static Pod GetPrimaryPod(this FullResultResponse response)
        {
            if (response.Pods.HasElements())
                return response.Pods.FirstOrDefault(pod => pod.IsPrimary);

            return null;
        }
    }
}