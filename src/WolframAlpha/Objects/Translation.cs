using Genbox.WolframAlpha.Serialization;

namespace Genbox.WolframAlpha.Objects
{
    public class Translation
    {
        public string Phrase { get; set; }

        [SerializeInfo(Name = "trans")]
        public string TranslatedText { get; set; }

        [SerializeInfo(Name = "lang")]
        public string Language { get; set; }

        public string Text { get; set; }
    }
}