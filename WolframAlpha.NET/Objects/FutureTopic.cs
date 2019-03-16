using RestSharp.Deserializers;

namespace WolframAlphaNET.Objects
{
    public class FutureTopic
    {
        public string Topic { get; set; }

        [DeserializeAs(Name = "msg")]
        public string Message { get; set; }
    }
}