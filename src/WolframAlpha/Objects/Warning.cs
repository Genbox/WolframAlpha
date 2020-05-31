using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha.Objects
{
    [SerializeInfo(Name = "warnings")]
    public class Warning
    {
        public Translation Translation { get; set; }
        public SpellCheck SpellCheck { get; set; }
        public Delimiters Delimiters { get; set; }
        public Reinterpret Reinterpret { get; set; }
    }
}