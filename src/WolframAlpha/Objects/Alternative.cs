using Genbox.WolframAlpha.Enums;

namespace Genbox.WolframAlpha.Objects
{
    public class Alternative
    {
        public double Score { get; set; }
        public Level Level { get; set; }
        public string Value { get; set; }
    }
}