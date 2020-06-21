using System.Collections.Generic;
using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha.Objects
{
    public class Variable
    {
        public string Name { get; set; }

        [SerializeInfo(Name = "desc")]
        public string Description { get; set; }

        public int Current { get; set; }

        public int Count { get; set; }

        public List<VariableValue> Values { get; set; }
    }
}