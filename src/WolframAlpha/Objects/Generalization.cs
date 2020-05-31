using System;
using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha.Objects
{
    public class Generalization
    {
        public string Topic { get; set; }

        [SerializeInfo(Name = "desc")]
        public string Description { get; set; }

        public Uri Url { get; set; }
    }
}