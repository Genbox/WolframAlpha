using RestSharp.Deserializers;

namespace WolframAlpha.Objects.Errors
{
    public class Error
    {
        public int Code { get; set; }

        [DeserializeAs(Name = "msg")]
        public string Message { get; set; }
    }
}