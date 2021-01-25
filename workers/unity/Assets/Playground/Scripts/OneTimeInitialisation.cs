using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using UnityEngine;

namespace Playground
{
    public static class OneTimeInitialisation
    {
        private static bool initialized;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            if (initialized)
            {
                return;
            }

            initialized = true;

            // Setup template to use for player on connecting client
            PlayerLifecycleConfig.CreatePlayerEntityTemplate = EntityTemplates.CreatePlayerEntityTemplate;
            PlayerLifecycleConfig.PlayerCreatorEntityId = new EntityId(2);
        }
    }
}
