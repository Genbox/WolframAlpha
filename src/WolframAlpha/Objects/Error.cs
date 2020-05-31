using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha.Objects
{
    public class Error
    {
        /// <summary>The error code, an integer.</summary>
        public int Code { get; set; }

        /// <summary>A short message describing the error.</summary>
        [SerializeInfo(Name = "msg")]
        public string Message { get; set; }
    }
}