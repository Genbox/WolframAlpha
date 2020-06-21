using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha.Objects
{
    public class Query
    {
        [SerializeInfo(Name = "i")]
        public string Input { get; set; }

        /// <summary>
        /// The primary output of the Fast Query Recognizer, the accepted parameter returns "true" if the input is believed to be an appropriate query for Wolfram|Alpha to process and "false" otherwise.
        /// </summary>
        public bool Accepted { get; set; }

        /// <summary>
        /// This parameter gives the CPU time (in milliseconds) used by the Recognizer to process the input.
        /// </summary>
        public double Timing { get; set; }

        /// <summary>
        /// For accepted inputs (i.e. those that return "true" for the accepted parameter), this parameter indicates the content domain under which the input is categorized. This gives a rough idea of the type of result you'd expect from sending this input to Wolfram|Alpha (either via the website or the other APIs).
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// This score is an initial estimate of how the Wolfram|Alpha result is expected to rank relative to search engine results, ranging from 100 (best) to 0 (worst, usually accompanied by a "false" value for the accepted parameter). You can think of this as a measure of how certain Wolfram|Alpha is about a result, with higher scores indicating a higher degree of certainty.
        /// </summary>
        public int ResultSignificanceScore { get; set; }

        public SummaryBox SummaryBox { get; set; }

        public Formula Formula { get; set; }
    }
}