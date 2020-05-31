using System;
using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha.Objects
{
    [SerializeInfo(Name = "img")]
    public class Image
    {
        public Uri Src { get; set; }

        public string Alt { get; set; }

        public string Title { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Type { get; set; }

        public string Themes { get; set; }

        [SerializeInfo(Name = "colorinvertable")]
        public bool ColorInvertible { get; set; }
    }
}