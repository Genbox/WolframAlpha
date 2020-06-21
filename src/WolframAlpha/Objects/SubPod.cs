using System.Collections.Generic;
using Genbox.WolframAlpha.Requests;
using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha.Objects
{
    public class SubPod
    {
        [SerializeInfo(Name = "img")]
        public Image Image { get; set; }

        public string Plaintext { get; set; }

        public string Title { get; set; }

        public List<string> MicroSources { get; set; }
        public List<string> DataSources { get; set; }
        public List<Info> Infos { get; set; }
        public List<State> States { get; set; }

        /// <summary>
        /// MathML elements enclose the Presentation MathML representation of a single subpod. They only appear if the
        /// requested result formats include mathml. &lt;mathml&gt; has no attributes.
        /// </summary>
        public string MathMl { get; set; }

        /// <summary>
        /// Wolfram Language input that can be executed within a Wolfram Language environment to provide the result given
        /// in a single subpod. Supplied when <see cref="FullResultRequest.Formats" /> is set to minput.
        /// </summary>
        public string MInput { get; set; }

        /// <summary>Wolfram Language output</summary>
        public string MOutput { get; set; }
    }
}