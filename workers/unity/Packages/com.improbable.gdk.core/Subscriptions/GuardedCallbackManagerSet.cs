using System.Collections.Generic;

namespace Improbable.Gdk.Subscriptions
{
    internal class GuardedCallbackManagerSet<TIndex, TManager> where TManager : ICallbackManager
    {
        private readonly Dictionary<TIndex, TManager> indexToCallbackManager =
            new Dictionary<TIndex, TManager>();

        private readonly List<TManager> callbackManagers = new List<TManager>();

        private readonly List<TIndex> indexesToAdd = new List<TIndex>();

        public bool TryGetManager(TIndex index, out TManager manager)
        {
            return indexToCallbackManager.TryGetValue(index, out manager);
        }

        public void AddCallbackManager(TIndex index, TManager manager)
        {
            indexToCallbackManager.Add(index, manager);
            indexesToAdd.Add(index);
        }

        public List<TManager> GetManagers()
        {
            foreach (var indexToAdd in indexesToAdd)
            {
                callbackManagers.Add(indexToCallbackManager[indexToAdd]);
            }

            indexesToAdd.Clear();

            return callbackManagers;
        }

        public void InvokeCallbacks()
        {
            foreach (var manager in GetManagers())
            {
                manager.InvokeCallbacks();
            }
        }
    }
}
