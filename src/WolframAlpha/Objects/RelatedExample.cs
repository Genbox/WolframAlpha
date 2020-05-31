using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha.Objects
{
    public class RelatedExample
    {
        public string Input { get; set; }

        [SerializeInfo(Name = "desc")]
        public string Description { get; set; }

        public string Category { get; set; }
        public string CategoryThumb { get; set; }
        public string CategoryPage { get; set; }
    }
}