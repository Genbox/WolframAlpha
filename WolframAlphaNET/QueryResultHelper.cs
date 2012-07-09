using System.Collections.Generic;
using System.Linq;
using System.Net;
using RestSharp;
using RestSharp.Deserializers;
using WolframAlphaNET.Objects;

namespace WolframAlphaNET
{
    public static class QueryResultHelper
    {
        public static Pod GetPrimaryPod(this QueryResult results)
        {
            if (results.Pods.HasElements())
                return results.Pods.FirstOrDefault(pod => pod.Primary);

            return null;
        }

        /// <summary>
        /// In case ScanTimeout was set too low, some scanners might have timedout. This method tries to recalculate the query in such a way that
        /// only the timedout scanners return their result. This is especially useful in a multi-threaded environment.
        /// </summary>
        /// <param name="result">The new pods that have been calculated.</param>
        /// <param name="includeInOriginal">When true, it will add the new pods to the original QueryResult</param>
        public static List<Pod> RecalculateResults(this QueryResult result, bool includeInOriginal = true)
        {
            if (!string.IsNullOrEmpty(result.Recalculate) && result.Timedout.Split(',').Length >= 1)
            {
                WebClient client = new WebClient();
                client.Proxy = null;
                string newPodsData = client.DownloadString(result.Recalculate);

                XmlDeserializer deserializer = new XmlDeserializer();
                RestResponse response = new RestResponse();
                response.Content = newPodsData;
                List<Pod> newPods = deserializer.Deserialize<List<Pod>>(response);
                newPods.Sort((p1, p2) => p1.Position.CompareTo(p2.Position));

                if (includeInOriginal)
                {
                    foreach (Pod newPod in newPods)
                    {
                        if (!result.Pods.Contains(newPod))
                            result.Pods.Add(newPod);
                    }

                    //We make sure that the pods are in the right order.
                    result.Pods.Sort((p1, p2) => p1.Position.CompareTo(p2.Position));
                }

                return newPods;
            }

            return new List<Pod>();
        }
    }
}
