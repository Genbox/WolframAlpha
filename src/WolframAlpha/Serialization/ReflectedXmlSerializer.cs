using System.IO;
using Genbox.WolframAlpha.Abstract;

namespace Genbox.WolframAlpha.Serialization
{
    public class ReflectedXmlSerializer : IXmlSerializer
    {
        private readonly SimpleXmlSerializer _serializer;

        public ReflectedXmlSerializer()
        {
            _serializer = new SimpleXmlSerializer();
        }

        public T Deserialize<T>(Stream s)
        {
            return _serializer.Deserialize<T>(s);
        }
    }
}