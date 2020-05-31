using System.IO;

namespace Genbox.WolframAlpha.Abstract
{
    public interface IXmlSerializer
    {
        public T Deserialize<T>(Stream s);
    }
}