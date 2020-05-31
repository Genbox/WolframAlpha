using System.Collections.Generic;
using Genbox.WolframAlpha.Enums;

namespace Genbox.WolframAlpha.Objects
{
    /// <summary>
    /// Wolfram|Alpha makes numerous assumptions when analyzing a query and deciding how to present its results. A
    /// simple example is a word that can refer to multiple things, like "pi", which is a well-known mathematical constant but
    /// is also the name of a movie.
    /// </summary>
    public class Assumption
    {
        public List<AssumptionValue> Values { get; set; }

        public AssumptionType Type { get; set; }

        public string Word { get; set; }

        public string Template { get; set; }
    }
}