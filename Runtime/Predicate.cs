using Lumpn.Storylets.Utils;

namespace Lumpn.Storylets
{
    public struct Predicate
    {
        public readonly int identifier;
        public readonly int min, max;

        // [min, max] both inclusive
        public Predicate(int identifier, int min, int max)
        {
            DebugAssert.LessOrEqual(min, max);

            this.identifier = identifier;
            this.min = min;
            this.max = max;
        }

        public bool Matches(int value)
        {
            return (value >= min)
                && (value <= max);
        }

        public bool Matches(IState state)
        {
            var value = state.GetValue(identifier);
            return Matches(value);
        }
    }
}
