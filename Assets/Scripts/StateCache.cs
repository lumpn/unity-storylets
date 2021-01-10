public sealed class StateCache : IState
{
    private struct Entry
    {
        public int call;
        public int id;
        public int value;
    }

    private readonly Entry[] entries;
    private readonly IState state;
    private int call = 0;

    public StateCache(int capacity, IState state)
    {
        this.entries = new Entry[capacity];
        this.state = state;
    }

    public int GetValue(int identifier)
    {
        call++;
        var idx = identifier % entries.Length;
        var entry = entries[idx];

        if (entry.call == call && entry.id == identifier)
        {
            // cache hit
            return entry.value;
        }

        // cache miss
        var value = state.GetValue(identifier);
        entry.call = call;
        entry.id = identifier;
        entry.value = value;
        return value;
    }
}
