using RestSharp.Deserializers;
using WolframAlphaNet.Objects.Output;

namespace WolframAlphaNet.Objects
{
    public class Unit
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }

        [DeserializeAs(Name = "img")]
        public Image Image { get; set; }
    }
}