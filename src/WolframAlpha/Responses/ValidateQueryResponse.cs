using System;
using System.Collections.Generic;
using Genbox.WolframAlpha.Objects;
using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha.Responses
{
    public class ValidateQueryResponse
    {
        [SerializeInfo(Name = "success")]
        public bool IsSuccess { get; set; }

        [SerializeInfo(Name = "error")]
        public bool IsError { get; set; }

        public double Timing { get; set; }
        public double ParseTiming { get; set; }
        public Version Version { get; set; }
        public List<Assumption> Assumptions { get; set; }
        public List<Warning> Warnings { get; set; }
    }
}