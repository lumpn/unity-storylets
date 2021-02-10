using UnityEngine;

namespace Lumpn.Storylets
{
    public sealed class LogAction : IAction
    {
        private readonly string message;

        public LogAction(string message)
        {
            this.message = message;
        }

        public void Execute()
        {
            Debug.Log(message);
        }
    }
}
