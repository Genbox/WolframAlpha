using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Genbox.WolframAlpha.Abstract;
using Genbox.WolframAlpha.Enums;
using Genbox.WolframAlpha.Extensions;
using Genbox.WolframAlpha.Objects;
using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha
{
    public class WolframAlphaClient : IWolframAlphaClient
    {
        private readonly WolframAlphaConfig _config;
        private readonly HttpClient _httpClient;
        private readonly IXmlSerializer _serializer;

        public WolframAlphaClient(string appId)
        {
            _httpClient = new HttpClient();
            _serializer = new ReflectedXmlSerializer();
            _config = new WolframAlphaConfig { AppId = appId };

            Initialize();
        }

        public WolframAlphaClient(HttpClient client, IXmlSerializer serializer, WolframAlphaConfig config)
        {
            _httpClient = client;
            _serializer = serializer;
            _config = config;

            Initialize();
        }

        /// <summary>Queries the full Wolfram|Alpha API.</summary>
        public Task<QueryResponse> QueryAsync(QueryRequest request, CancellationToken token = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrEmpty(request.Input))
                throw new ArgumentException("You must supply an input");

            List<(string, string)> queryStrings = new List<(string, string)>();
            queryStrings.Add(("appid", _config.AppId));
            queryStrings.Add(("input", request.Input));

            if (request.Formats != Format.Unknown)
                queryStrings.Add(("format", string.Join(",", request.Formats.GetFlags().Select(x => x.ToString().ToLowerInvariant()))));

            //Pod Selection
            if (request.IncludePodIds.HasElements())
            {
                foreach (string include in request.IncludePodIds)
                {
                    queryStrings.Add(("includepodid", include));
                }
            }

            if (request.ExcludePodIds.HasElements())
            {
                foreach (string exclude in request.ExcludePodIds)
                {
                    queryStrings.Add(("excludepodid", exclude));
                }
            }

            if (request.PodTitles.HasElements())
            {
                foreach (string podTitle in request.PodTitles)
                {
                    queryStrings.Add(("podtitle", podTitle));
                }
            }

            if (request.PodIndex.HasElements())
                queryStrings.Add(("podindex", string.Join(",", request.PodIndex)));

            if (request.Scanners.HasElements())
                queryStrings.Add(("scanner", string.Join(",", request.Scanners)));

            //Location
            if (request.IpAddress != null)
                queryStrings.Add(("ip", request.IpAddress.ToString()));

            if (!string.IsNullOrEmpty(request.Location))
                queryStrings.Add(("location", request.Location));

            if (request.GeoLocation != null)
                queryStrings.Add(("latlong", request.GeoLocation.ToString()));

            //Size
            if (request.Width > 0)
                queryStrings.Add(("width", request.Width.ToString(NumberFormatInfo.InvariantInfo)));

            if (request.MaxWidth > 0)
                queryStrings.Add(("maxwidth", request.MaxWidth.ToString(NumberFormatInfo.InvariantInfo)));

            if (request.PlotWidth > 0)
                queryStrings.Add(("plotwidth", request.PlotWidth.ToString(NumberFormatInfo.InvariantInfo)));

            if (request.Magnification > 0)
                queryStrings.Add(("mag", request.Magnification.ToString(CultureInfo.InvariantCulture)));

            //Timeout/Async
            if (request.ScanTimeout > double.Epsilon)
                queryStrings.Add(("scantimeout", request.ScanTimeout.ToString(CultureInfo.InvariantCulture)));

            if (request.PodTimeout > double.Epsilon)
                queryStrings.Add(("podtimeout", request.PodTimeout.ToString(CultureInfo.InvariantCulture)));

            if (request.FormatTimeout > double.Epsilon)
                queryStrings.Add(("formattimeout", request.FormatTimeout.ToString(CultureInfo.InvariantCulture)));

            if (request.ParseTimeout > double.Epsilon)
                queryStrings.Add(("parsetimeout", request.ParseTimeout.ToString(CultureInfo.InvariantCulture)));

            if (request.TotalTimeout > double.Epsilon)
                queryStrings.Add(("totaltimeout", request.TotalTimeout.ToString(CultureInfo.InvariantCulture)));

            if (request.UseAsync.HasValue)
                queryStrings.Add(("async", request.UseAsync.Value.ToString().ToLowerInvariant()));

            //Misc
            if (request.Reinterpret.HasValue)
                queryStrings.Add(("reinterpret", request.Reinterpret.Value.ToString().ToLowerInvariant()));

            if (request.Translation.HasValue)
                queryStrings.Add(("translation", request.Translation.Value.ToString().ToLowerInvariant()));

            if (request.IgnoreCase.HasValue)
                queryStrings.Add(("ignorecase", request.IgnoreCase.Value.ToString().ToLowerInvariant()));

            if (!string.IsNullOrEmpty(request.Signature))
                queryStrings.Add(("sig", request.Signature));

            if (request.Assumptions.HasElements())
            {
                foreach (string assumption in request.Assumptions)
                {
                    queryStrings.Add(("assumption", assumption));
                }
            }

            if (request.PodStates.HasElements())
            {
                foreach (string podState in request.PodStates)
                {
                    queryStrings.Add(("podstate", podState));
                }
            }

            if (request.OutputUnit != Unit.Unknown)
                queryStrings.Add(("units", request.OutputUnit == Unit.Metric ? "metric" : "nonmetric")); //We map manually since the values do not match the enum

            return ExecuteRequestAsync<QueryResponse>("query" + EncodeQueryString(queryStrings), token);
        }

        public Task<ValidateQueryResponse> ValidateQueryAsync(string input, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("You must supply an input", nameof(input));

            List<(string, string)> queryStrings = new List<(string, string)>();
            queryStrings.Add(("appid", _config.AppId));
            queryStrings.Add(("input", input));

            return ExecuteRequestAsync<ValidateQueryResponse>("validatequery" + EncodeQueryString(queryStrings), token);
        }

        /// <summary>Queries the full Wolfram|Alpha API.</summary>
        public Task<QueryResponse> QueryAsync(string query, CancellationToken token = default)
        {
            QueryRequest req = new QueryRequest(query);
            return QueryAsync(req, token);
        }

        /// <summary>Queries the simple Wolfram|Alpha API.</summary>
        public async Task<byte[]> SimpleQueryAsync(SimpleQueryRequest request, CancellationToken token = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrEmpty(request.Input))
                throw new ArgumentException("You must supply an input");

            List<(string, string)> queryStrings = new List<(string, string)>();
            queryStrings.Add(("appid", _config.AppId));
            queryStrings.Add(("i", request.Input));

            if (request.Layout != Layout.Unknown)
                queryStrings.Add(("format", request.Layout.ToString().ToLowerInvariant()));

            if (request.BackgroundColor != null)
                queryStrings.Add(("background", request.BackgroundColor));

            if (request.ForegroundColor != null)
                queryStrings.Add(("foreground", request.ForegroundColor));

            if (request.FontSize > 0)
                queryStrings.Add(("fontsize", request.FontSize.ToString(NumberFormatInfo.InvariantInfo)));

            if (request.Width > 0)
                queryStrings.Add(("width", request.Width.ToString(NumberFormatInfo.InvariantInfo)));

            if (request.OutputUnit != Unit.Unknown)
                queryStrings.Add(("units", request.OutputUnit.ToString().ToLowerInvariant()));

            if (request.Timeout > 0)
                queryStrings.Add(("timeout", request.Timeout.ToString(NumberFormatInfo.InvariantInfo)));

            using (HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, "simple" + EncodeQueryString(queryStrings)))
            using (HttpResponseMessage httpResponse = await _httpClient.SendAsync(httpRequest, token).ConfigureAwait(false))
                return await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        /// <summary>Queries the simple Wolfram|Alpha API.</summary>
        public Task<byte[]> SimpleQueryAsync(string input, CancellationToken token = default)
        {
            SimpleQueryRequest req = new SimpleQueryRequest(input);
            return SimpleQueryAsync(req, token);
        }

        /// <summary>
        /// In case ScanTimeout was set too low, some scanners might have timed out. This method recalculate the query in
        /// such a way that only the timed out scanners return their result.
        /// </summary>
        public async Task RecalculateAsync(QueryResponse response, CancellationToken token = default)
        {
            if (response.RecalculateUrl == null || string.IsNullOrEmpty(response.TimedOut))
                return;

            using (HttpResponseMessage httpResponse = await _httpClient.GetAsync(response.RecalculateUrl, token).ConfigureAwait(false))
            using (Stream stream = await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                QueryResponse newResponse = _serializer.Deserialize<QueryResponse>(stream);

                response.RecalculateUrl = newResponse.RecalculateUrl;
                response.TimedOut = newResponse.TimedOut;
            }
        }

        public async Task GetAsyncPodsAsync(QueryResponse result, CancellationToken token = default)
        {
            for (int i = 0; i < result.Pods.Count; i++)
            {
                Pod pod = result.Pods[i];

                if (pod.AsyncUrl == null)
                    continue;

                using (HttpResponseMessage httpResponse = await _httpClient.GetAsync(pod.AsyncUrl, token).ConfigureAwait(false))
                using (Stream stream = await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    Pod newPod = _serializer.Deserialize<Pod>(stream);
                    result.Pods[i] = newPod;
                }
            }
        }

        private void Initialize()
        {
            if (string.IsNullOrEmpty(_config.AppId))
                throw new ArgumentException("App Id is required.");

            _httpClient.BaseAddress = new Uri("https://api.wolframalpha.com/v2/");
        }

        private static string EncodeQueryString(ICollection<(string, string)> query)
        {
            if (query.Count == 0)
                return null;

            StringBuilder sb = new StringBuilder();

            int count = 0;
            foreach ((string key, string value) in query)
            {
                if (count == 0)
                    sb.Append("?");
                else
                    sb.Append("&");

                sb.Append(key + "=" + HttpUtility.UrlEncode(value));

                count++;
            }

            return sb.ToString();
        }

        private async Task<T> ExecuteRequestAsync<T>(string url, CancellationToken token) where T : class
        {
            using (HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, url))
            using (HttpResponseMessage httpResponse = await _httpClient.SendAsync(httpRequest, token).ConfigureAwait(false))
            using (Stream httpStream = await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false))
                return _serializer.Deserialize<T>(httpStream);
        }
    }
}