using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha.Objects
{
    public class FutureTopic
    {
        public string Topic { get; set; }

        [SerializeInfo(Name = "msg")]
        public string Message { get; set; }
    }
}