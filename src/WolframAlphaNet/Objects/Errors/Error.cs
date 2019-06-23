using RestSharp.Deserializers;

namespace WolframAlphaNet.Objects.Errors
{
    public class Error
    {
        public int Code { get; set; }

        [DeserializeAs(Name = "msg")]
        public string Message { get; set; }
    }
}