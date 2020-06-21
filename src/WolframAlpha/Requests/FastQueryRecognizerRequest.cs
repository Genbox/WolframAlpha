using System;
using Genbox.WolframAlpha.Enums;

namespace Genbox.WolframAlpha.Requests
{
    public class FastQueryRecognizerRequest
    {
        public FastQueryRecognizerRequest(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("You must supply an input", nameof(input));

            Input = input;
            Mode = QueryRecognizerMode.Default;
        }

        /// <summary>The query you want to make to Wolfram|Alpha.</summary>
        public string Input { get; }

        /// <summary>
        /// The Fast Query Recognizer is available in two different modes, each of which is configured to accept certain types of inputs. "Default" mode is designed to recognize any query for which Wolfram|Alpha returns a relevant result, with the goal of placing as many answers as possible into results. Some specific types of queries (e.g. phone numbers, IP addresses, product codes) are explicitly filtered, and only inputs with unambiguous linguistics are accepted (e.g. "boston employment" is not accepted, but "boston employment rate" is). This tuning up for ambiguity is an ongoing improvement. he "Voice" mode of the Fast Query Recognizer is optimized for spoken input.  It is generally less restrictive than the "Default" mode, meaning that more variation of the input will be accepted.
        /// </summary>
        public QueryRecognizerMode Mode { get; set; }
    }
}