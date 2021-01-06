public sealed class PredicateBuilder
{
    public string identifier;
    public int min, max;

    public Predicate Build(Lookup lookup)
    {
        var id = lookup.Register(identifier);
        return new Predicate(id, min, max);
    }
}
