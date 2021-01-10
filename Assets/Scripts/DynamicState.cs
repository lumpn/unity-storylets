using System.Collections.Generic;

public sealed class DynamicState : IState
{
    private readonly Dictionary<int, Variable> variables = new Dictionary<int, Variable>();

    public Variable AddVariable(int identifier, int value)
    {
        var variable = new Variable(identifier, value);
        variables.Add(identifier, variable);
        return variable;
    }

    public int GetValue(int identifier)
    {
        if (variables.TryGetValue(identifier, out Variable variable))
        {
            return variable.value;
        }

        return Constants.noValue;
    }
}
