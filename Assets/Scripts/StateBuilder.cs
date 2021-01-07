using System.Collections.Generic;
using System.Linq;

public sealed class StateBuilder
{
    private readonly List<VariableBuilder> variableBuilders = new List<VariableBuilder>();

    public VariableBuilder AddVariable(Lookup lookup, string identifier)
    {
        return AddVariable(lookup.Register(identifier));
    }

    public VariableBuilder AddVariable(int identifier)
    {
        var builder = new VariableBuilder(identifier);
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
