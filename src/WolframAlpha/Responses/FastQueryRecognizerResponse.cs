using System;
using Genbox.WolframAlpha.Objects;

namespace Genbox.WolframAlpha.Responses
{
    public class FastQueryRecognizerResponse
    {
        public Version Version { get; set; }
        public bool SpellingCorrection { get; set; }
        public int BuildNumber { get; set; }
        public Query Query { get; set; }
        public Error Error { get; set; }
    }
}