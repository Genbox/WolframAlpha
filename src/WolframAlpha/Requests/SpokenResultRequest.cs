using System;
using Genbox.WolframAlpha.Enums;

namespace Genbox.WolframAlpha.Requests
{
    public class SpokenResultRequest
    {
        public SpokenResultRequest(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("You must supply an input", nameof(input));

            Input = input;
        }

        /// <summary>The query you want to make to Wolfram|Alpha.</summary>
        public string Input { get; }

        /// <summary>
        /// Use this parameter to manually select what system of units to use for measurements and quantities (either
        /// "metric" or "imperial"). By default, Wolfram|Alpha will use your location to determine this setting.
        /// </summary>
        public Unit OutputUnit { get; set; }

        /// <summary>
        /// This parameter specifies the maximum amount of time (in seconds) allowed to process a query, with a default
        /// value of "5". Although it is primarily used to optimize response times in applications, the timeout parameter may
        /// occasionally affect what value is returned by the Short Answers API.
        /// </summary>
        public int Timeout { get; set; }
    }
}