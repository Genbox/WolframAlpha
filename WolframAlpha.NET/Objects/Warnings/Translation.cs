using RestSharp.Deserializers;

namespace WolframAlphaNET.Objects.Warnings
{
    public class Translation
    {
        public string Text { get; set; }
        public string Phrase { get; set; }

        [DeserializeAs(Name = "trans")]
        public string TranslatedText { get; set; }

        [DeserializeAs(Name = "lang")]
        public string Language { get; set; }
    }
}