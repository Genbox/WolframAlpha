using System;
using Genbox.WolframAlpha.Enums;

namespace Genbox.WolframAlpha.Requests
{
    public class SimpleQueryRequest
    {
        public SimpleQueryRequest(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("You must supply an input", nameof(input));

            Input = input;
        }

        /// <summary>The query you want to make to Wolfram|Alpha.</summary>
        public string Input { get; }

        /// <summary>
        /// For API types that return full Wolfram|Alpha output, the layout parameter defines how content is presented.
        /// The default setting is divider, which specifies a series of pods with horizontal dividers. The other option, labelbar,
        /// specifies a series of separate content sections with label bar headings
        /// </summary>
        public Layout Layout { get; set; }

        /// <summary>
        /// This parameter allows you to change the overall background color for visual results. For example, if you want
        /// a light grey background, set it to F5F5F5. Colors can be expressed as HTML names (e.g. "white"), hexadecimal RGB values
        /// (e.g. "00AAFF") or comma-separated decimal RGB values (e.g. "0,100,200"). You can also add an alpha channel to RGB
        /// values (e.g. "0,100,200,200") or specify "transparent" or "clear" for a transparent background. The default background
        /// color is white.
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// Use this parameter to select a foreground color—either "black" (default) or "white"—for text elements. The
        /// foreground parameter is useful for making text more readable against certain background colors.
        /// </summary>
        public string ForegroundColor { get; set; }

        /// <summary>
        /// Specify the display size of text elements in points, with a default setting of 14. Oversized text (i.e.
        /// anything too wide to fit inside your "width" setting) will automatically be hyphenated.
        /// </summary>
        public int FontSize { get; set; }

        /// <summary>
        /// This parameter specifies the desired width (in pixels) for output images, with a default setting of "500". In
        /// order to display text and images optimally, the actual output size may vary slightly. Any text too large to fit will be
        /// hyphenated, so it's best to use this in conjunction with the fontsize parameter.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Use this parameter to manually select what system of units to use for measurements and quantities (either
        /// "metric" or "imperial"). By default, Wolfram|Alpha will use your location to determine this setting.
        /// </summary>
        public Unit OutputUnit { get; set; }

        /// <summary>
        /// This parameter specifies the maximum amount of time (in seconds) allowed to process a query, with a default
        /// value of "5". It is primarily used to optimize response times in applications, although it may also affect the number
        /// and type of results returned by the Simple API.
        /// </summary>
        public int Timeout { get; set; }
    }
}