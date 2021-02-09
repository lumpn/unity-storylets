public sealed class PredicateBuilder
{
    private readonly Lookup lookup;
    public int identifier;
    public int min, max;

    public PredicateBuilder(Lookup lookup, int identifier)
    {
        this.lookup = lookup;
        this.identifier = identifier;
    }

    public PredicateBuilder EqualTo(string value)
    {
        return EqualTo(lookup.Register(value));
    }

    public PredicateBuilder EqualTo(int value)
    {
        min = value;
        max = value;
        return this;
    }

    public PredicateBuilder LessThan(int value)
    {
        min = Constants.minValue;
        max = value - 1;
        return this;
    }

    // [min, max] both inclusive
    public PredicateBuilder Between(int min, int max)
    {
        Assert.NotEqual(min, Constants.noValue);
        Assert.NotEqual(max, Constants.noValue);
        Assert.LessOrEqual(min, max);

        this.min = min;
        this.max = max;
        return this;
    }

    public Predicate Build()
    {
        return new Predicate(identifier, min, max);
    }
}
