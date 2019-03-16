using System.Collections.Generic;

namespace WolframAlpha.Objects.Warnings
{
    public class Reinterpret
    {
        public string Text { get; set; }
        public string New { get; set; }
        public List<Alternative> Alternative { get; set; }
    }
}