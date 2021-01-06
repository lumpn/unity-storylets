using System.Collections.Generic;
using System.Linq;

public sealed class StateBuilder
{
    private readonly Lookup lookup;
    private readonly List<VariableBuilder> variableBuilders = new List<VariableBuilder>();

    public StateBuilder(Lookup lookup)
    {
        this.lookup = lookup;
    }

    public VariableBuilder AddVariable(string identifier)
    {
        var builder = new VariableBuilder(lookup, identifier);
        variableBuilders.Add(builder);
        return builder;
    }

    public State Build()
    {
        var variables = variableBuilders.Select(p => p.Build())
                                        .OrderBy(p => p, VariableIdentifierComparer.Default)
                                        .ToArray();

        return new State(variables);
    }
}
