using System.Collections.Generic;

namespace Lumpn.Storylets.Utils
{
    public static class DictionaryExtensions
    {
        public static V GetOrFallback<K, V>(this IDictionary<K, V> dictionary, K key, V fallback)
        {
            if (dictionary.TryGetValue(key, out V result))
            {
                return result;
            }
            return fallback;
        }
    }
}
