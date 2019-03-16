using System.Collections.Generic;

namespace WolframAlphaNET.Objects
{
    public class ValidateQueryResult
    {
        public bool Success { get; set; }
        public bool Error { get; set; }
        public float Timing { get; set; }
        public float ParseTiming { get; set; }
        public string Version { get; set; }

        public List<Assumption> Assumptions { get; set; }
    }
}