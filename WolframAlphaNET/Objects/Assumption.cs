using System.Collections.Generic;

namespace WolframAlphaNET.Objects
{
    public class Assumption
    {
        public string Type { get; set; }
        public string Word { get; set; }
        public string Template { get; set; }
        public List<Value> Values { get; set; }
    }
}