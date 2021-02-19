namespace Lumpn.Storylets.Utils
{
    public sealed class StateCache : IState
    {
        private struct Entry
        {
            public int call;
            public int variableIdentifier;
            public int variableValue;
        }

        private readonly Entry[] entries;
        private readonly IState state;
        private int call = 1;

        public StateCache(IState state, int capacity)
        {
            this.entries = new Entry[capacity];
            this.state = state;
        }

        public void Invalidate()
        {
            call++;
        }

        public int GetValue(int identifier)
        {
            var idx = identifier % entries.Length;
            var entry = entries[idx];

            if (entry.call == call && entry.variableIdentifier == identifier)
            {
                // cache hit
                return entry.variableValue;
            }

            // cache miss
            var value = state.GetValue(identifier);
            entry.call = call;
            entry.variableIdentifier = identifier;
            entry.variableValue = value;
            return value;
        }
    }
}
