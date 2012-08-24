using System.Collections.Generic;
using System.Linq;
using System.Net;
using RestSharp;
using RestSharp.Deserializers;
using WolframAlphaNET.Objects;

namespace WolframAlphaNET.Misc
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
            if (!string.IsNullOrEmpty(result.Recalculate) && !string.IsNullOrEmpty(result.Timedout))
            {
                WebClient client = new WebClient();
                string newPodsData = client.DownloadString(result.Recalculate);

                XmlAttributeDeserializer deserializer = new XmlAttributeDeserializer();
                RestResponse response = new RestResponse();
                response.Content = newPodsData;

                QueryResult asyncResult = deserializer.Deserialize<QueryResult>(response);
                result.Recalculate = asyncResult.Recalculate;
                result.TimedoutPods = asyncResult.TimedoutPods;

                List<Pod> newPods = asyncResult.Pods;

                if (newPods != null)
                {
                    newPods.Sort((p1, p2) => p1.Position.CompareTo(p2.Position));

                    if (includeInOriginal)
                    {
                        if (result.Pods == null)
                            result.Pods = newPods;
                        else
                        {
                            foreach (Pod newPod in newPods)
                            {
                                if (!result.Pods.Contains(newPod))
                                    result.Pods.Add(newPod);
                            }
                        }

                        //We make sure that the pods are in the right order.
                        result.Pods.Sort((p1, p2) => p1.Position.CompareTo(p2.Position));
                    }

                    return newPods;
                }
            }

            return new List<Pod>();
        }

        public static List<Pod> GetAsyncPods(this QueryResult result, bool includeInOriginal = true)
        {
            if (result.Pods.Any(p => !string.IsNullOrEmpty(p.Async)))
            {
                IEnumerable<string> asyncPods = result.Pods.Where(p => !string.IsNullOrEmpty(p.Async)).Select(p => p.Async);
                WebClient client = new WebClient();

                XmlAttributeDeserializer deserializer = new XmlAttributeDeserializer();
                RestResponse response = new RestResponse();
                List<Pod> newPods = new List<Pod>();

                foreach (string asyncPodLink in asyncPods)
                {
                    string newPodsData = client.DownloadString(asyncPodLink);
                    response.Content = newPodsData;
                    newPods.Add(deserializer.Deserialize<Pod>(response));
                }

                newPods.Sort((p1, p2) => p1.Position.CompareTo(p2.Position));

                if (includeInOriginal)
                {
                    foreach (Pod newPod in newPods)
                    {
                        //Replace the existing pod with the new pod.
                        //Note: Pods are compared by ID, so this is functional code.
                        if (result.Pods.Contains(newPod))
                        {
                            result.Pods.Remove(newPod);
                            result.Pods.Add(newPod);
                        }
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
