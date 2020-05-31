using System.Collections.Generic;
using Genbox.WolframAlpha.Enums;

namespace Genbox.WolframAlpha.Objects
{
    public class Reinterpret
    {
        public string Text { get; set; }
        public string New { get; set; }
        public double Score { get; set; }
        public Level Level { get; set; }
        public List<Alternative> Alternatives { get; set; }
    }
}