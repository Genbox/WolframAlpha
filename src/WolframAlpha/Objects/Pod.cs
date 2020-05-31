using System;
using System.Collections.Generic;
using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha.Objects
{
    public class Pod
    {
        public List<SubPod> SubPods { get; set; }

        public List<Expressiontype> ExpressionTypes { get; set; }

        public string Title { get; set; }

        public string Scanner { get; set; }

        public string Id { get; set; }

        public int Position { get; set; }

        [SerializeInfo(Name = "error")]
        public bool IsError { get; set; }

        [SerializeInfo(Name = "primary")]
        public bool IsPrimary { get; set; }

        public List<State> States { get; set; }
        public List<Definition> Definitions { get; set; }
        public List<Info> Infos { get; set; }

        /// <summary>Used when format includes HTML</summary>
        public string Markup { get; set; }

        [SerializeInfo(Name = "async")]
        public Uri AsyncUrl { get; set; }

        public List<Sound> Sounds { get; set; }
    }
}