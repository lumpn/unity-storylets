using System;

public sealed class State
{
    /// variables sorted by identifier
    private readonly Variable[] variables;

    public State(Variable[] variables)
    {
        this.variables = variables;
    }

    public int GetValue(int identifier)
    {
        var key = new Variable(identifier, 0);
        var idx = Array.BinarySearch<Variable>(variables, key, VariableIdentifierComparer.Default);
        if (idx < 0)
        {
            return 0;
        }

        var variable = variables[idx];
        return variable.value;
    }
}
