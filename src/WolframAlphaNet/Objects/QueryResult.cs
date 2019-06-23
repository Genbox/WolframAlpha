using System.Collections.Generic;
using RestSharp.Deserializers;
using WolframAlphaNET.Objects.Errors;
using WolframAlphaNET.Objects.Output;
using WolframAlphaNET.Objects.Warnings;

namespace WolframAlphaNET.Objects
{
    public class QueryResult
    {
        public bool Success { get; set; }
        public string Version { get; set; }
        public string DataTypes { get; set; }
        public float Timing { get; set; }
        public string Timedout { get; set; }
        public float ParseTiming { get; set; }
        public bool ParseTimedout { get; set; }
        public string TimedoutPods { get; set; }

        /// <summary>
        /// A link to calculate the pods that did not make it within the scan timeout.
        /// </summary>
        public string Recalculate { get; set; }
        public string ID { get; set; }
        public string Host { get; set; }
        public int Server { get; set; }
        public string Related { get; set; }
        public List<Pod> Pods { get; set; }

        public List<Assumption> Assumptions { get; set; }
        public List<Source> Sources { get; set; }
        public Warning Warnings { get; set; }
        public Generalization Generalization { get; set; }
        public List<Tip> Tips { get; set; }

        [DeserializeAs(Name = "didyoumeans")]
        public List<DidYouMean> DidYouMean { get; set; }

        [DeserializeAs(Name = "languagemsg")]
        public LanguageMessage LanguageMessage { get; set; }
        public FutureTopic FutureTopic { get; set; }
        public List<RelatedExample> RelatedExamples { get; set; }
        public ExamplePage ExamplePage { get; set; }
        public Error Error { get; set; }

        /// <summary>
        /// It will only appear if the requested result formats include html, and there will only be one. Its content is a CDATA section containing a series of <script> elements defining JavaScript functionality needed by the HTML in the <markup> elements. <scripts> has no attributes.
        /// </summary>
        public Script Scripts { get; set; }

        /// <summary>
        /// It will only appear if the requested result formats include html, and there will only be one. Its content is a CDATA section containing a series of <link> elements defining CSS files needed by the HTML in the <markup> elements. <css> has no attributes.
        /// </summary>
        public CSS CSS { get; set; }

        //TODO: Test
        /// <summary>
        /// MathML elements enclose the Presentation MathML representation of a single subpod. They only appear if the requested result formats include mathml. <mathml> has no attributes.
        /// </summary>
        public MathML MathML { get; set; }

        public bool Equals(QueryResult other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.ID, ID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(QueryResult)) return false;
            return Equals((QueryResult)obj);
        }

        public override int GetHashCode()
        {
            return (ID != null ? ID.GetHashCode() : 0);
        }
    }
}