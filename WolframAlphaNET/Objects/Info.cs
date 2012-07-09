using System.Collections.Generic;
using WolframAlphaNET.Objects.Output;

namespace WolframAlphaNET.Objects
{
    public class Info
    {
        public string Text { get; set; }
        public Image Img { get; set; }
        public List<Link> Links { get; set; }
    }
}