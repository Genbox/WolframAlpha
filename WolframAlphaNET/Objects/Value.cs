using RestSharp.Deserializers;

namespace WolframAlphaNET.Objects
{
    public class Value
    {
        public string Name { get; set; }

        [DeserializeAs(Name = "desc")]
        public string Description { get; set; }
        public string Input { get; set; }
    }
}