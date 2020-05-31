using System;

namespace Genbox.WolframAlpha.Serialization
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = false)]
    internal sealed class SerializeInfoAttribute : Attribute
    {
        public string Name { get; set; }

        public bool IsAttribute { get; set; }
    }
}