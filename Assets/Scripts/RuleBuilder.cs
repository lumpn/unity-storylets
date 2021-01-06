using System.Collections.Generic;
using System.Linq;

public sealed class RuleBuilder
{
    private readonly Lookup lookup;
    private readonly List<PredicateBuilder> predicateBuilders = new List<PredicateBuilder>();
    public IEffect effect;

    public RuleBuilder(Lookup lookup)
    {
        this.lookup = lookup;
    }

    public PredicateBuilder AddPredicate(string identifier)
    {
        var builder = new PredicateBuilder(lookup, identifier);
        predicateBuilders.Add(builder);
        return builder;
    }

    public RuleBuilder SetEffect(IEffect effect)
    {
        this.effect = effect;
        return this;
    }

    public Rule Build()
    {
        var predicates = predicateBuilders.Select(p => p.Build())
                                          .OrderBy(p => p, PredicateIdentifierComparer.Default)
                                          .ToArray();

        return new Rule(predicates, effect);
    }
}
