namespace Lumpn.Storylets.Utils
{
    public sealed class StateCache : IState
    {
        private struct CachedVariable
        {
            public readonly int call;
            public readonly int identifier;
            public readonly int value;

            public CachedVariable(int call, int identifier, int value)
            {
                this.call = call;
                this.identifier = identifier;
                this.value = value;
            }
        }

        private readonly CachedVariable[] entries;
        private readonly IState state;
        private int call = 1;

        public StateCache(IState state, int capacity)
        {
            this.entries = new CachedVariable[capacity];
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

            if (entry.call == call && entry.identifier == identifier)
            {
                // cache hit
                return entry.value;
            }

            // cache miss
            var value = state.GetValue(identifier);
            entries[idx] = new CachedVariable(call, identifier, value);

            return value;
        }
    }
}
