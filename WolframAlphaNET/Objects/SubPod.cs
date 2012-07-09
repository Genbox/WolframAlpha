using System.Collections.Generic;
using WolframAlphaNET.Objects.Output;

namespace WolframAlphaNET.Objects
{
    public class SubPod
    {
        public string Title { get; set; }
        public string Plaintext { get; set; }
        public Image Img { get; set; }
        public List<State> States { get; set; }
    }
}