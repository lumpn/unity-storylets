using System.Collections.Generic;
using System.Linq;

public sealed class RuleBuilder
{
    public readonly List<PredicateBuilder> predicateBuilders = new List<PredicateBuilder>();
    public IEffect effect;

    public Rule Build(Lookup lookup)
    {
        var predicates = predicateBuilders.Select(p => p.Build(lookup))
                                          .OrderBy(p => p, PredicateIdentifierComparer.Default)
                                          .ToArray();

        return new Rule(predicates, effect);
    }
}
