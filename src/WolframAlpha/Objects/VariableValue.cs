using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha.Objects
{
    public class VariableValue
    {
        public string Name { get; set; }

        [SerializeInfo(Name = "desc")]
        public string Description { get; set; }
    }
}