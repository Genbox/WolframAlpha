using System.Collections.Generic;
using RestSharp.Deserializers;
using WolframAlpha.Objects.Output;

namespace WolframAlpha.Objects
{
    public class Info
    {
        public string Text { get; set; }

        [DeserializeAs(Name = "img")]
        public Image Image { get; set; }
        public List<Link> Links { get; set; }
    }
}