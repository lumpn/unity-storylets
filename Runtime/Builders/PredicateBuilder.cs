using Lumpn.Storylets.Utils;

namespace Lumpn.Storylets.Builders
{
    public sealed class PredicateBuilder
    {
        private readonly SymbolLookup lookup;
        public int identifier;
        public int min, max;

        public PredicateBuilder(SymbolLookup lookup, int identifier)
        {
            DebugAssert.NotNull(lookup);

            this.lookup = lookup;
            this.identifier = identifier;
        }

        public PredicateBuilder EqualTo(string value)
        {
            return EqualTo(lookup.Register(value));
        }

        public PredicateBuilder EqualTo(int value)
        {
            DebugAssert.NotEqual(min, Constants.NoValue);

            min = value;
            max = value;
            return this;
        }

        public PredicateBuilder LessThan(int value)
        {
            min = Constants.MinValue;
            max = value - 1;
            return this;
        }

        // [min, max] both inclusive
        public PredicateBuilder Between(int min, int max)
        {
            DebugAssert.NotEqual(min, Constants.NoValue);
            DebugAssert.NotEqual(max, Constants.NoValue);
            DebugAssert.LessOrEqual(min, max);

            this.min = min;
            this.max = max;
            return this;
        }

        public Predicate Build()
        {
            return new Predicate(identifier, min, max);
        }
    }
}
