using System.Collections.Generic;
using RestSharp.Deserializers;
using WolframAlphaNET.Objects.Output;

namespace WolframAlphaNET.Objects
{
    public class Info
    {
        public string Text { get; set; }

        [DeserializeAs(Name = "img")]
        public Image Image { get; set; }
        public List<Link> Links { get; set; }
    }
}