using RestSharp.Deserializers;
using WolframAlpha.Objects.Output;

namespace WolframAlpha.Objects
{
    public class Unit
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }

        [DeserializeAs(Name = "img")]
        public Image Image { get; set; }
    }
}