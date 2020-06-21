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
using Genbox.WolframAlpha.Misc;
using Genbox.WolframAlpha.Objects;
using Genbox.WolframAlpha.Requests;
using Genbox.WolframAlpha.Responses;
using Genbox.WolframAlpha.Serialization;
using Microsoft.Extensions.ObjectPool;

namespace Genbox.WolframAlpha
{
    /// <summary>A client to interact with the Wolfram|Alpha APIs</summary>
    public class WolframAlphaClient : IWolframAlphaClient
    {
        private readonly WolframAlphaConfig _config;
        private readonly HttpClient _httpClient;
        private readonly IXmlSerializer _serializer;
        private readonly ObjectPool<StringBuilder> _sbPool;
        private readonly ObjectPool<List<(string, string)>> _queryPool;
        private const string _apiV2 = "https://api.wolframalpha.com/v2/";

        /// <summary>Creates a new instance of the WolframAlphaClient.</summary>
        /// <param name="appId">The AppId you have obtained from Wolfram|Alpha</param>
        public WolframAlphaClient(string appId)
        {
            _httpClient = new HttpClient();
            _serializer = new ReflectedXmlSerializer();
            _config = new WolframAlphaConfig { AppId = appId };

            DefaultObjectPoolProvider objectPoolProvider = new DefaultObjectPoolProvider();
            _sbPool = objectPoolProvider.CreateStringBuilderPool();
            _queryPool = objectPoolProvider.Create(new ListPoolPolicy<(string, string)>());
        }

        /// <summary>Creates a new instance of the WolframAlphaClient. You can use this constructor if you want to utilize Dependency Injection.</summary>
        public WolframAlphaClient(HttpClient client, IXmlSerializer serializer, ObjectPoolProvider poolProvider, WolframAlphaConfig config)
        {
            _httpClient = client;
            _serializer = serializer;
            _sbPool = poolProvider.CreateStringBuilderPool();
            _queryPool = poolProvider.Create(new ListPoolPolicy<(string, string)>());
            _config = config;
        }

        /// <summary>Queries the Full Results API.</summary>
        public Task<QueryResponse> FullQueryAsync(string input, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("You must supply an input", nameof(input));

            List<(string, string)> query = _queryPool.Get();
            query.Add(("appid", _config.AppId));
            query.Add(("input", input));

            string url = EncodeUrl(_apiV2, "query", query);
            _queryPool.Return(query);

            return ExecuteRequestAsync<QueryResponse>(url, token);
        }

        /// <summary>Queries the Full Results API.</summary>
        public Task<QueryResponse> FullQueryAsync(QueryRequest request, CancellationToken token = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            List<(string, string)> query = _queryPool.Get();
            query.Add(("appid", _config.AppId));
            query.Add(("input", request.Input));

            if (request.Formats != Format.Unknown)
                query.Add(("format", string.Join(",", request.Formats.GetFlags().Select(x => x.ToString().ToLowerInvariant()))));

            //Pod Selection
            if (request.IncludePodIds.HasElements())
            {
                foreach (string include in request.IncludePodIds)
                {
                    query.Add(("includepodid", include));
                }
            }

            if (request.ExcludePodIds.HasElements())
            {
                foreach (string exclude in request.ExcludePodIds)
                {
                    query.Add(("excludepodid", exclude));
                }
            }

            if (request.PodTitles.HasElements())
            {
                foreach (string podTitle in request.PodTitles)
                {
                    query.Add(("podtitle", podTitle));
                }
            }

            if (request.PodIndex.HasElements())
                query.Add(("podindex", string.Join(",", request.PodIndex)));

            if (request.Scanners.HasElements())
                query.Add(("scanner", string.Join(",", request.Scanners)));

            //Location
            if (request.IpAddress != null)
                query.Add(("ip", request.IpAddress.ToString()));

            if (!string.IsNullOrEmpty(request.Location))
                query.Add(("location", request.Location));

            if (request.GeoLocation != null)
                query.Add(("latlong", request.GeoLocation.ToString()));

            //Size
            if (request.Width > 0)
                query.Add(("width", request.Width.ToString(NumberFormatInfo.InvariantInfo)));

            if (request.MaxWidth > 0)
                query.Add(("maxwidth", request.MaxWidth.ToString(NumberFormatInfo.InvariantInfo)));

            if (request.PlotWidth > 0)
                query.Add(("plotwidth", request.PlotWidth.ToString(NumberFormatInfo.InvariantInfo)));

            if (request.Magnification > 0)
                query.Add(("mag", request.Magnification.ToString(CultureInfo.InvariantCulture)));

            //Timeout/Async
            if (request.ScanTimeout > double.Epsilon)
                query.Add(("scantimeout", request.ScanTimeout.ToString(CultureInfo.InvariantCulture)));

            if (request.PodTimeout > double.Epsilon)
                query.Add(("podtimeout", request.PodTimeout.ToString(CultureInfo.InvariantCulture)));

            if (request.FormatTimeout > double.Epsilon)
                query.Add(("formattimeout", request.FormatTimeout.ToString(CultureInfo.InvariantCulture)));

            if (request.ParseTimeout > double.Epsilon)
                query.Add(("parsetimeout", request.ParseTimeout.ToString(CultureInfo.InvariantCulture)));

            if (request.TotalTimeout > double.Epsilon)
                query.Add(("totaltimeout", request.TotalTimeout.ToString(CultureInfo.InvariantCulture)));

            if (request.UseAsync.HasValue)
                query.Add(("async", request.UseAsync.Value.ToString().ToLowerInvariant()));

            //Misc
            if (request.Reinterpret.HasValue)
                query.Add(("reinterpret", request.Reinterpret.Value.ToString().ToLowerInvariant()));

            if (request.Translation.HasValue)
                query.Add(("translation", request.Translation.Value.ToString().ToLowerInvariant()));

            if (request.IgnoreCase.HasValue)
                query.Add(("ignorecase", request.IgnoreCase.Value.ToString().ToLowerInvariant()));

            if (!string.IsNullOrEmpty(request.Signature))
                query.Add(("sig", request.Signature));

            if (request.Assumptions.HasElements())
            {
                foreach (string assumption in request.Assumptions)
                {
                    query.Add(("assumption", assumption));
                }
            }

            if (request.PodStates.HasElements())
            {
                foreach (string podState in request.PodStates)
                {
                    query.Add(("podstate", podState));
                }
            }

            if (request.OutputUnit != Unit.Unknown)
                query.Add(("units", request.OutputUnit == Unit.Metric ? "metric" : "nonmetric")); //We map manually since the values do not match the enum

            string url = EncodeUrl(_apiV2, "query", query);
            _queryPool.Return(query);

            return ExecuteRequestAsync<QueryResponse>(url, token);
        }

        /// <summary>Validate a query to see if Wolfram|Alpha has any issues with it.</summary>
        public Task<ValidateQueryResponse> ValidateQueryAsync(string input, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("You must supply an input", nameof(input));

            List<(string, string)> query = _queryPool.Get();
            query.Add(("appid", _config.AppId));
            query.Add(("input", input));

            string url = EncodeUrl(_apiV2, "validatequery", query);
            _queryPool.Return(query);

            return ExecuteRequestAsync<ValidateQueryResponse>(url, token);
        }

        /// <summary>Queries the Simple API.</summary>
        public Task<byte[]> SimpleQueryAsync(string input, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("You must supply an input", nameof(input));

            List<(string, string)> query = _queryPool.Get();
            query.Add(("appid", _config.AppId));
            query.Add(("i", input));

            string url = EncodeUrl(_apiV2, "simple", query);
            _queryPool.Return(query);
            return ExecuteRequestDataAsync(url, token);
        }

        /// <summary>Queries the Simple API.</summary>
        public Task<byte[]> SimpleQueryAsync(SimpleQueryRequest request, CancellationToken token = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            List<(string, string)> query = _queryPool.Get();
            query.Add(("appid", _config.AppId));
            query.Add(("i", request.Input));

            if (request.Layout != Layout.Unknown)
                query.Add(("format", request.Layout.ToString().ToLowerInvariant()));

            if (request.BackgroundColor != null)
                query.Add(("background", request.BackgroundColor));

            if (request.ForegroundColor != null)
                query.Add(("foreground", request.ForegroundColor));

            if (request.FontSize > 0)
                query.Add(("fontsize", request.FontSize.ToString(NumberFormatInfo.InvariantInfo)));

            if (request.Width > 0)
                query.Add(("width", request.Width.ToString(NumberFormatInfo.InvariantInfo)));

            if (request.OutputUnit != Unit.Unknown)
                query.Add(("units", request.OutputUnit.ToString().ToLowerInvariant()));

            if (request.Timeout > 0)
                query.Add(("timeout", request.Timeout.ToString(NumberFormatInfo.InvariantInfo)));

            string url = EncodeUrl(_apiV2, "simple", query);
            _queryPool.Return(query);
            return ExecuteRequestDataAsync(url, token);
        }

        /// <summary>Queries the Short Answers API.</summary>
        public Task<string> ShortAnswerAsync(string input, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("You must supply an input", nameof(input));

            List<(string, string)> query = _queryPool.Get();
            query.Add(("appid", _config.AppId));
            query.Add(("i", input));

            string url = EncodeUrl(_apiV2, "result", query);
            _queryPool.Return(query);

            return ExecuteRequestStringAsync(url, token);
        }

        /// <summary>Queries the Short Answers API.</summary>
        public Task<string> ShortAnswerAsync(ShortAnswerRequest request, CancellationToken token = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            List<(string, string)> query = _queryPool.Get();
            query.Add(("appid", _config.AppId));
            query.Add(("i", request.Input));

            if (request.OutputUnit != Unit.Unknown)
                query.Add(("units", request.OutputUnit.ToString().ToLowerInvariant()));

            if (request.Timeout > 0)
                query.Add(("timeout", request.Timeout.ToString(NumberFormatInfo.InvariantInfo)));

            string url = EncodeUrl(_apiV2, "result", query);
            _queryPool.Return(query);

            return ExecuteRequestStringAsync(url, token);
        }

        /// <summary>
        /// In case ScanTimeout was set too low, some scanners might have timed out. This method recalculate the query in
        /// such a way that only the timed out scanners return their result.
        /// </summary>
        public async Task RecalculateAsync(QueryResponse response, CancellationToken token = default)
        {
            if (response.RecalculateUrl == null || string.IsNullOrEmpty(response.TimedOut))
                return;

            QueryResponse newResponse = await ExecuteRequestAsync<QueryResponse>(response.RecalculateUrl.ToString(), token).ConfigureAwait(false);
            response.RecalculateUrl = newResponse.RecalculateUrl;
            response.TimedOut = newResponse.TimedOut;
        }

        /// <summary>Updates your <see cref="QueryResponse" /> with pod results that are async.</summary>
        public async Task GetAsyncPodsAsync(QueryResponse result, CancellationToken token = default)
        {
            for (int i = 0; i < result.Pods.Count; i++)
            {
                Pod pod = result.Pods[i];

                if (pod.AsyncUrl == null)
                    continue;

                result.Pods[i] = await ExecuteRequestAsync<Pod>(pod.AsyncUrl.ToString(), token).ConfigureAwait(false);
            }
        }

        private string EncodeUrl(string baseUrl, string controller, IList<(string, string)> query)
        {
            if (query.Count == 0)
                return null;

            StringBuilder sb = _sbPool.Get();
            sb.Append(baseUrl);
            sb.Append(controller);

            for (int i = 0; i < query.Count; i++)
            {
                (string key, string value) = query[i];

                sb.Append(i == 0 ? '?' : '&');
                sb.Append(key).Append('=').Append(HttpUtility.UrlEncode(value));
            }

            string result = sb.ToString();
            _sbPool.Return(sb);

            return result;
        }

        private async Task<T> ExecuteRequestAsync<T>(string url, CancellationToken token) where T : class
        {
            using (HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, url))
            using (HttpResponseMessage httpResponse = await _httpClient.SendAsync(httpRequest, token).ConfigureAwait(false))
            using (Stream httpStream = await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false))
                return _serializer.Deserialize<T>(httpStream);
        }

        private async Task<string> ExecuteRequestStringAsync(string url, CancellationToken token)
        {
            using (HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, url))
            using (HttpResponseMessage httpResponse = await _httpClient.SendAsync(httpRequest, token).ConfigureAwait(false))
                return await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private async Task<byte[]> ExecuteRequestDataAsync(string url, CancellationToken token)
        {
            using (HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, url))
            using (HttpResponseMessage httpResponse = await _httpClient.SendAsync(httpRequest, token).ConfigureAwait(false))
                return await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }
    }
}