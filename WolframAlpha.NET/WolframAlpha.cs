using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Net;
using RestSharp;
using RestSharp.Deserializers;
using WolframAlphaNET.Enums;
using WolframAlphaNET.Misc;
using WolframAlphaNET.Objects;
using Unit = WolframAlphaNET.Enums.Unit;

namespace WolframAlphaNET
{
    public class WolframAlpha
    {
        private RestClient _client = new RestClient();
        private CultureInfo _culture = new CultureInfo("en");
        private const float Epsilon = 0.00001f;
        private string _appId;
        private bool _useTls;

        /// <summary>
        /// Creates a new instance of the Wolfram Alpha API
        /// </summary>
        /// <param name="appId">An ID provided by Wolfram Research that identifies the application or organization making the request.</param>
        public WolframAlpha(string appId)
        {
            if (string.IsNullOrEmpty(appId))
                throw new ArgumentException("App ID is required.", "appId");

            _appId = appId;

            ApiUrl = "api.wolframalpha.com/v2/";
            Formats = new List<Format>();
            Assumptions = new List<string>();
            IncludePodIDs = new List<string>();
            ExcludePodIDs = new List<string>();
            PodTitles = new List<string>();
            PodIndex = new List<int>();
            Scanners = new List<string>();
        }

        /// <summary>
        /// Set to false to use HTTP instead of HTTPS. HTTPS is used by default.
        /// </summary>
        public bool UseTLS
        {
            get { return _useTls; }
            set
            {
                _useTls = value;

                string oldUrl = ApiUrl;

                if (string.IsNullOrWhiteSpace(oldUrl))
                    return;

                if (oldUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
                    oldUrl = oldUrl.Substring(8);
                else if (oldUrl.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                    oldUrl = oldUrl.Substring(7);

                _client.BaseUrl = _useTls ? new Uri("https://" + oldUrl) : new Uri("http://" + oldUrl);
            }
        }

        /// <summary>
        /// The URL which the service listens on. IF you don't set the scheme to http:// or https:// it will default to https.
        /// </summary>
        public string ApiUrl
        {
            get { return _client.BaseUrl.ToString(); }
            set
            {
                string newUrl = value.Trim();

                if (string.IsNullOrWhiteSpace(newUrl))
                    return;

                if (newUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
                {
                    _useTls = true;
                    newUrl = newUrl.Substring(8);
                }
                else if (newUrl.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                {
                    _useTls = false;
                    newUrl = newUrl.Substring(7);
                }
                else
                    _useTls = true;

                _client.BaseUrl = _useTls ? new Uri("https://" + newUrl) : new Uri("http://" + newUrl);
            }
        }

        //Output
        /// <summary>
        /// Optional; Determines the formats of the output
        /// Defaults to "plaintext,image"
        /// </summary>
        public List<Format> Formats { get; set; }

        /// <summary>
        /// Optional; Specifies an assumption, such as the meaning of a word or the value of a formula variable.
        /// </summary>
        public List<string> Assumptions { get; set; }

        /// <summary>
        /// Optional; Lets you specify the preferred measurement system, either "metric" or "nonmetric" (U.S. customary units).
        /// Note: Defaults to making a decision based on the caller's geographic location. If the location is set.
        /// </summary>
        public Unit OutputUnit { get; set; }

        //Filtering
        /// <summary>
        /// Optional; Specifies a pod ID to include. You can specify more than one of these elements in the query. Only pods with the given IDs will be returned.
        /// Default is all pods.
        /// </summary>
        public List<string> IncludePodIDs { get; set; }

        /// <summary>
        /// Optional; Specifies a pod ID to exclude. You can specify more than one of these elements in the query. Pods with the given IDs will be excluded from the result.
        /// Default is to exclude no pods.
        /// </summary>
        public List<string> ExcludePodIDs { get; set; }

        /// <summary>
        /// Optional; Specifies a pod title. You can specify more than one of these elements in the query. Only pods with the given titles will be returned. You can use * as a wildcard to match zero or more characters in pod titles.
        /// Default is all pods.
        /// </summary>
        public List<string> PodTitles { get; set; }

        /// <summary>
        /// Optional; Specifies the index of the pod(s) to return. This is an alternative to specifying pods by title or ID. You can give a single number or a sequence like "2,3,5".
        /// Default is all pods.
        /// </summary>
        public List<int> PodIndex { get; set; }

        /// <summary>
        /// Optional; Specifies that only pods produced by the given scanner should be returned. You can specify more than one of these elements in the query.
        /// Default is all pods.
        /// </summary>
        public List<string> Scanners { get; set; }

        //Timeouts
        /// <summary>
        /// Optional; The number of seconds to allow Wolfram|Alpha to spend in the "parsing" stage of processing.
        /// Default is 5.0 seconds
        /// </summary>
        public float ParseTimeout { get; set; }

        /// <summary>
        /// Optional; The number of seconds to allow Wolfram|Alpha to compute results in the "scan" stage of processing.
        /// Default is 3.0 seconds
        /// </summary>
        public float ScanTimeout { get; set; }

        /// <summary>
        /// Optional; The number of seconds to allow Wolfram|Alpha to spend in the "format" stage for any one pod.
        /// Default is 4.0 seconds
        /// </summary>
        public float PodTimeout { get; set; }

        /// <summary>
        /// Optional; The number of seconds to allow Wolfram|Alpha to spend in the "format" stage for the entire collection of pods.
        /// </summary>
        public float FormatTimeout { get; set; }

        //Async
        //TODO: Revise here
        //TODO: Async can actually be set to a time such as 0.2. See manual.
        /// <summary>
        /// Optional; Wolfram|Alpha can use an asynchronous mode that allows partial results to come back before all the pods are computed.
        /// The number of pods that comes in the partial request depends on their size and number.
        /// <see cref="PodTimeout"/> gets set to 0.4 if UseAsync is set to true.
        /// Use ReCalculate() as a means of getting more partial results
        /// </summary>
        public bool UseAsync { get; set; }

        //Location
        /// <summary>
        /// Optional; By default, Wolfram|Alpha attempts to determine the caller's location from the caller IP address, but you can override the IP here.
        /// </summary>
        public IPAddress IpAddress { get; set; }

        /// <summary>
        /// Optional; You can specify your physical location here in the form "Los Angeles, CA" or similar.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Optional; Lets you specify a latitude/longitude pair like "40.42,-3.71". Negative latitude values are South, and negative longitude values are West.
        /// </summary>
        public GeoCoordinate GPSLocation { get; set; }

        //Sizes
        /// <summary>
        /// The width of the images drawn.
        /// When Wolfram|Alpha formats results to the width value, it will attempt to detect if undesirable line breaks were forced to be used, and if so it will automatically re-format to your larger <see cref="MaxWidth"/>
        /// Default is 500 pixels.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The maximum width of images drawn.
        /// Default is 500 pixels.
        /// </summary>
        public int MaxWidth { get; set; }

        /// <summary>
        /// The width of ploits that are drawn.
        /// Default is 200 pixels.
        /// </summary>
        public int PlotWidth { get; set; }

        /// <summary>
        /// The amount of magnification to use in images.
        /// Default is 1.0
        /// </summary>
        public float Magnification { get; set; }

        //Misc
        /// <summary>
        /// Optional; A special signature that can be applied to guard against misuse of your AppID. Talk to Wolfram Alpha on how to get a signature.
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// Optional; Whether to allow Wolfram|Alpha to reinterpret queries that would otherwise not be understood.
        /// Default is false.
        /// </summary>
        public bool? ReInterpret { get; set; }

        /// <summary>
        /// Whether to force Wolfram|Alpha to ignore case in queries.
        /// Default is false.
        /// </summary>
        public bool? IgnoreCase { get; set; }

        /// <summary>
        /// Whether to allow Wolfram|Alpha to try to translate simple queries into English.
        /// Default is true.
        /// </summary>
        public bool? EnableTranslate { get; set; }

        public bool ValidateQuery(string query, out ValidateQueryResult result)
        {
            //http://api.wolframalpha.com/v2/validatequery?input=xx&appid=xxxxx
            RestRequest request = CreateRequest("validatequery",query);
            
            ValidateQueryResult results = GetResponse<ValidateQueryResult>(request);

            if (results != null)
            {
                result = results;
                return !results.Error && results.Success;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Query on Wolfram Alpha using the specified 
        /// </summary>
        /// <param name="query">The query you would like to search for on Wolfram Alpha</param>
        /// <returns>The results of the query</returns>
        public QueryResult Query(string query)
        {
            //http://api.wolframalpha.com/v2/query?input=xx&appid=xxxxx
            RestRequest request = CreateRequest("query", query);

            //Output
            if (Formats.HasElements())
                request.AddParameter("format", string.Join(",", Formats));

            if (OutputUnit != Unit.NotSet)
                request.AddParameter("units", OutputUnit.ToString().ToLower());

            if (Assumptions.HasElements())
            {
                foreach (string assumption in Assumptions)
                {
                    request.AddParameter("assumption", assumption);
                }
            }

            //Filtering
            if (IncludePodIDs.HasElements())
            {
                foreach (string include in IncludePodIDs)
                {
                    request.AddParameter("includepodid", include);
                }
            }

            if (ExcludePodIDs.HasElements())
            {
                foreach (string exclude in ExcludePodIDs)
                {
                    request.AddParameter("excludepodid", exclude);
                }
            }

            if (PodTitles.HasElements())
            {
                foreach (string podTitle in PodTitles)
                {
                    request.AddParameter("podtitle", podTitle);
                }
            }

            if (PodIndex.HasElements())
                request.AddParameter("podindex", string.Join(",", PodIndex));

            if (Scanners.HasElements())
                request.AddParameter("scanner", string.Join(",", Scanners));

            //Timeout
            if (ParseTimeout >= Epsilon)
                request.AddParameter("parsetimeout", ParseTimeout.ToString(_culture));

            if (ScanTimeout >= Epsilon)
                request.AddParameter("scantimeout", ScanTimeout.ToString(_culture));

            if (PodTimeout >= Epsilon)
                request.AddParameter("podtimeout", PodTimeout.ToString(_culture));

            if (FormatTimeout >= Epsilon)
                request.AddParameter("formattimeout", FormatTimeout.ToString(_culture));

            //Async
            if (UseAsync)
                request.AddParameter("async", UseAsync.ToString().ToLower());

            //Location
            if (IpAddress != null)
                request.AddParameter("ip", IpAddress.ToString());

            if (!string.IsNullOrEmpty(Location))
                request.AddParameter("location", Location);

            if (GPSLocation != null)
                request.AddParameter("latlong", GPSLocation.ToString());

            //Size
            if (Width >= 1f)
                request.AddParameter("width", Width);

            if (MaxWidth >= 1f)
                request.AddParameter("maxwidth", MaxWidth);

            if (PlotWidth >= 1f)
                request.AddParameter("plotwidth", PlotWidth);

            if (Magnification >= 0.1f)
                request.AddParameter("mag", Magnification.ToString(_culture));

            //Misc
            if (!string.IsNullOrEmpty(Signature))
                request.AddParameter("sig", Signature);

            if (ReInterpret.HasValue)
                request.AddParameter("reinterpret", ReInterpret.ToString().ToLower());

            if (IgnoreCase.HasValue)
                request.AddParameter("ignorecase", IgnoreCase.ToString().ToLower());

            if (EnableTranslate.HasValue)
                request.AddParameter("translation", EnableTranslate.ToString().ToLower());

            QueryResult results = GetResponse<QueryResult>(request);
            return results;
        }

        private RestRequest CreateRequest(string function, string query)
        {
            RestRequest request = new RestRequest(function, Method.GET);
            request.AddParameter("appid", _appId);
            request.AddParameter("input", query);
            return request;
        }

        private T GetResponse<T>(RestRequest request)
        {
            RestResponse response = (RestResponse)_client.Execute(request);
            XmlAttributeDeserializer deserializer = new XmlAttributeDeserializer();
            T results = deserializer.Deserialize<T>(response);
            return results;
        }
    }
}