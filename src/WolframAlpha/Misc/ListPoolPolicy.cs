using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Genbox.WolframAlpha.Misc
{
    public class ListPoolPolicy<T> : PooledObjectPolicy<List<T>>
    {
        public int InitialCapacity { get; set; } = 8;

        public override List<T> Create()
        {
            return new List<T>(InitialCapacity);
        }

        public override bool Return(List<T> obj)
        {
            obj.Clear();
            return true;
        }
    }
}