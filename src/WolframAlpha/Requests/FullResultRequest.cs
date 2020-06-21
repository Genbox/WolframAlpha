using System;
using System.Collections.Generic;
using System.Net;
using Genbox.WolframAlpha.Enums;
using Genbox.WolframAlpha.Objects;

namespace Genbox.WolframAlpha.Requests
{
    public class FullResultRequest
    {
        public FullResultRequest(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("You must supply an input", nameof(input));

            Input = input;
            PodStates = new List<string>(0);
            PodTitles = new List<string>(0);
            PodIndex = new List<int>(0);
            IncludePodIds = new List<string>(0);
            ExcludePodIds = new List<string>(0);
            Scanners = new List<string>(0);
            Assumptions = new List<string>(0);
        }

        //Basic parameters
        /// <summary>The query you want to make to Wolfram|Alpha.</summary>
        public string Input { get; set; }

        /// <summary>Optional; Determines the formats of the output Defaults to "plaintext,image"</summary>
        public Format Formats { get; set; }

        //Pod selection
        /// <summary>
        /// Optional; Specifies a pod ID to include. You can specify more than one of these elements in the query. Only
        /// pods with the given IDs will be returned. Default is all pods.
        /// </summary>
        public List<string> IncludePodIds { get; set; }

        /// <summary>
        /// Optional; Specifies a pod ID to exclude. You can specify more than one of these elements in the query. Pods
        /// with the given IDs will be excluded from the result. Default is to exclude no pods.
        /// </summary>
        public List<string> ExcludePodIds { get; set; }

        /// <summary>
        /// Optional; Specifies a pod title. You can specify more than one of these elements in the query. Only pods with
        /// the given titles will be returned. You can use * as a wildcard to match zero or more characters in pod titles. Default
        /// is all pods.
        /// </summary>
        public List<string> PodTitles { get; set; }

        /// <summary>
        /// Optional; Specifies the index of the pod(s) to return. This is an alternative to specifying pods by title or
        /// ID. You can give a single number or a sequence like "2,3,5". Default is all pods.
        /// </summary>
        public List<int> PodIndex { get; set; }

        /// <summary>
        /// Optional; Specifies that only pods produced by the given scanner should be returned. You can specify more than
        /// one of these elements in the query. Default is all pods.
        /// </summary>
        public List<string> Scanners { get; set; }

        //Timeouts
        /// <summary>
        /// Optional; The number of seconds to allow Wolfram|Alpha to compute results in the "scan" stage of processing.
        /// Default is 3 seconds.
        /// </summary>
        public double ScanTimeout { get; set; }

        /// <summary>
        /// Optional; The number of seconds to allow Wolfram|Alpha to spend in the "format" stage for any one pod. Default
        /// is 4 seconds.
        /// </summary>
        public double PodTimeout { get; set; }

        /// <summary>
        /// Optional; The number of seconds to allow Wolfram|Alpha to spend in the "format" stage for the entire
        /// collection of pods. Default is 8 seconds.
        /// </summary>
        public double FormatTimeout { get; set; }

        /// <summary>
        /// Optional; The number of seconds to allow Wolfram|Alpha to spend in the "parsing" stage of processing. Default
        /// is 5 seconds.
        /// </summary>
        public double ParseTimeout { get; set; }

        /// <summary>Optional; The total number of seconds to allow Wolfram|Alpha to spend on a query. Defaults is 20 seconds.</summary>
        public double TotalTimeout { get; set; }

        /// <summary>
        /// Optional; Wolfram|Alpha can use an asynchronous mode that allows partial results to come back before all the
        /// pods are computed. The number of pods that comes in the partial request depends on their size and number.
        /// <see cref="PodTimeout" /> gets set to 0.4 if UseAsync is set to true. Use
        /// <see cref="WolframAlphaClient.RecalculateQueryAsync" /> as a means of getting more partial results.
        /// </summary>
        public bool? UseAsync { get; set; }

        //Location
        /// <summary>
        /// Optional; By default, Wolfram|Alpha attempts to determine the caller's location from the caller IP address,
        /// but you can override the IP here.
        /// </summary>
        public IPAddress IpAddress { get; set; }

        /// <summary>
        /// Optional; You can specify your physical location here in the form "Los Angeles, CA" or similar. By default
        /// Wolfram|Alpha tries to determine the location using the callers IP address.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Optional; Lets you specify a latitude/longitude pair like "40.42,-3.71". Negative latitude values are South,
        /// and negative longitude values are West.
        /// </summary>
        public GeoCoordinate GeoLocation { get; set; }

        //Sizes
        /// <summary>
        /// The width of the images drawn. When Wolfram|Alpha formats results to the width value, it will attempt to
        /// detect if undesirable line breaks were forced to be used, and if so it will automatically re-format to your larger
        /// <see cref="MaxWidth" /> Default is 500 pixels.
        /// </summary>
        public int Width { get; set; }

        /// <summary>The maximum width of images drawn. Default is 500 pixels.</summary>
        public int MaxWidth { get; set; }

        /// <summary>The width of plots that are drawn. Default is 200 pixels.</summary>
        public int PlotWidth { get; set; }

        /// <summary>The amount of magnification to use in images. Default is 1.0</summary>
        public double Magnification { get; set; }

        //Misc
        /// <summary>
        /// Optional; Whether to allow Wolfram|Alpha to reinterpret queries that would otherwise not be understood.
        /// Default is false.
        /// </summary>
        public bool? Reinterpret { get; set; }

        /// <summary>Whether to allow Wolfram|Alpha to try to translate simple queries into English. Default is true.</summary>
        public bool? Translation { get; set; }

        /// <summary>Whether to force Wolfram|Alpha to ignore case in queries. Default is false.</summary>
        public bool? IgnoreCase { get; set; }

        /// <summary>
        /// Optional; A special signature that can be applied to guard against misuse of your AppID. Talk to Wolfram Alpha
        /// on how to get a signature.
        /// </summary>
        public string Signature { get; set; }

        /// <summary>Optional; Specifies an assumption, such as the meaning of a word or the value of a formula variable.</summary>
        public List<string> Assumptions { get; set; }

        /// <summary>
        /// Optional; Specifies a pod state change, which replaces a pod with a modified version, such as a switch from
        /// Imperial to metric units.
        /// </summary>
        public List<string> PodStates { get; set; }

        /// <summary>
        /// Optional; Lets you specify the preferred measurement system, either <see cref="Unit.Metric" /> or
        /// <see cref="Unit.Imperial" /> (U.S. customary units). Note: Defaults to making a decision based on the caller's
        /// geographic location.
        /// </summary>
        public Unit OutputUnit { get; set; }
    }
}