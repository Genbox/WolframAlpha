using RestSharp.Deserializers;

namespace WolframAlphaNET.Objects.Errors
{
    public class Error
    {
        public int Code { get; set; }

        [DeserializeAs(Name = "msg")]
        public string Message { get; set; }
    }
}