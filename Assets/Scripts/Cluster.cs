public sealed class Cluster : IRuleset
{
    private readonly int identifier, threshold;
    private readonly IRuleset below, above;

    public Cluster(int identifier, int threshold, IRuleset below, IRuleset above)
    {
        this.identifier = identifier;
        this.threshold = threshold;
        this.below = below;
        this.above = above;
    }

    public Rule Query(State state)
    {
        var value = state.GetValue(identifier);
        var ruleset = (value < threshold) ? below : above;
        return ruleset.Query(state);
    }
}
