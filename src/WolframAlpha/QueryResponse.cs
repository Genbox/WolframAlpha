using System;
using System.Collections.Generic;
using Genbox.WolframAlpha.Objects;
using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha
{
    public class QueryResponse
    {
        [SerializeInfo(Name = "languagemsg")]
        public LanguageMessage LanguageMessage { get; set; }

        public List<Tip> Tips { get; set; }

        public List<Pod> Pods { get; set; }

        public List<Assumption> Assumptions { get; set; }

        public ExamplePage ExamplePage { get; set; }

        public List<Warning> Warnings { get; set; }

        [SerializeInfo(Name = "success")]
        public bool IsSuccess { get; set; }

        [SerializeInfo(IsAttribute = true, Name = "error")]
        public bool IsError { get; set; }

        public string DataTypes { get; set; }

        public string TimedOut { get; set; }

        public string TimedOutPods { get; set; }

        public double Timing { get; set; }

        public double ParseTiming { get; set; }

        public bool ParseTimedOut { get; set; }

        [SerializeInfo(Name = "recalculate")]
        public Uri RecalculateUrl { get; set; }

        public string Id { get; set; }

        public string Host { get; set; }

        public int Server { get; set; }

        public string Related { get; set; }

        public Version Version { get; set; }

        public List<Source> Sources { get; set; }
        public Generalization Generalization { get; set; }
        public List<DidYouMean> DidYouMeans { get; set; }

        [SerializeInfo(Name = "error")]
        public Error ErrorDetails { get; set; }

        public FutureTopic FutureTopic { get; set; }

        public List<RelatedExample> RelatedExamples { get; set; }

        /// <summary>
        /// It will only appear if the requested result formats include html, and there will only be one. Its content is a
        /// CDATA section containing a series of &lt;script&gt; elements defining JavaScript functionality needed by the HTML in
        /// the &lt;markup&gt; elements.
        /// </summary>
        [SerializeInfo(Name = "scripts")]
        public string Scripts { get; set; }

        /// <summary>
        /// It will only appear if the requested result formats include html, and there will only be one. Its content is a
        /// CDATA section containing a series of &lt;link&gt; elements defining CSS files needed by the HTML in the &lt;markup&gt;
        /// elements. &lt;css&gt; has no attributes.
        /// </summary>
        [SerializeInfo(Name = "css")]
        public string Css { get; set; }
    }
}