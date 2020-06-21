using System.Collections.Generic;
using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha.Objects
{
    public class Formula
    {
        public string Name { get; set; }

        [SerializeInfo(Name = "desc")]
        public string Description { get; set; }

        public List<Variable> Variables { get; set; }
    }
}