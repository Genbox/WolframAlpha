using Genbox.WolframAlpha.Requests;
using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha.Objects
{
    [SerializeInfo(Name = "value")]
    public class AssumptionValue
    {
        public string Name { get; set; }

        [SerializeInfo(Name = "desc")]
        public string Description { get; set; }

        /// <summary>
        /// The assumption input. Can be used with <see cref="QueryRequest.Assumptions" /> to make a query with an
        /// assumption.
        /// </summary>
        public string Input { get; set; }
    }
}