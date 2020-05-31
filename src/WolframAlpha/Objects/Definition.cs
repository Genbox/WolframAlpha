using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha.Objects
{
    public class Definition
    {
        public string Word { get; set; }

        [SerializeInfo(Name = "desc")]
        public string Description { get; set; }
    }
}