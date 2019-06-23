using RestSharp.Deserializers;

namespace WolframAlphaNet.Objects
{
    public class Generalization
    {
        public string Topic { get; set; }
        
        [DeserializeAs(Name = "desc")]
        public string Description { get; set; }
        public string Url { get; set; }
    }
}