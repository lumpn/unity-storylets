using Lumpn.Storylets.Utils;

namespace Lumpn.Storylets
{
    /// cluster of two ruleset separated by a given threshold
    public sealed class Cluster : IRuleset
    {
        public readonly int identifier, threshold;
        public readonly IRuleset below, above;

        public Cluster(int identifier, int threshold, IRuleset below, IRuleset above)
        {
            Assert.NotNull(below);
            Assert.NotNull(above);

            this.identifier = identifier;
            this.threshold = threshold;
            this.below = below;
            this.above = above;
        }

        public Rule Query(IState state)
        {
            var value = state.GetValue(identifier);
            var ruleset = (value < threshold) ? below : above;
            return ruleset.Query(state);
        }
    }
}
