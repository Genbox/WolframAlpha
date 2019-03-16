using System.Collections.Generic;
using RestSharp.Deserializers;
using WolframAlphaNET.Objects.Output;

namespace WolframAlphaNET.Objects
{
    public class SubPod
    {
        public string Minput { get; set; }

        public string Title { get; set; }
        public string Plaintext { get; set; }

        [DeserializeAs(Name = "img")]
        public Image Image { get; set; }
        public List<State> States { get; set; }
    }
}