using RestSharp.Deserializers;
using WolframAlphaNET.Objects.Output;

namespace WolframAlphaNET.Objects
{
    public class Unit
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }

        [DeserializeAs(Name = "img")]
        public Image Image { get; set; }
    }
}