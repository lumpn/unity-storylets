using System;
using System.Collections.Generic;
using Lumpn.Storylets.Utils;

namespace Lumpn.Storylets.Builders
{
    public sealed class SymbolLookup
    {
        private readonly Dictionary<string, int> symbols = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        public int Register(string symbol)
        {
            if (symbols.TryGetValue(symbol, out int result))
            {
                return result;
            }

            result = symbols.Count;
            symbols.Add(symbol, result);
            return result;
        }

        public int Query(string symbol)
        {
            return symbols.GetOrDefault(symbol, -1);
        }
    }
}
