using System;
using Improbable.Gdk.PlayerLifecycle;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    public static class OneTimeInitialisation
    {
        private static bool initialized = false;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            if (initialized)
            {
                return;
            }

            initialized = true;
            SetupInjectionHooks();
            PlayerLoopManager.RegisterDomainUnload(DomainUnloadShutdown, 1000);

            // Setup template to use for player on connecting client
            PlayerLifecycleConfig.CreatePlayerEntityTemplate = PlayerTemplate.CreatePlayerEntityTemplate;
        }

        private static void SetupInjectionHooks()
        {
            var hybridAssembly = typeof(GameObjectEntity).Assembly;

            // Reflection to get internal hook classes. Doesn't seem to be a proper way to do this.
            var gameObjectArrayInjectionHookType =
                hybridAssembly.GetType("Unity.Entities.GameObjectArrayInjectionHook");
            var transformAccessArrayInjectionHookType =
                hybridAssembly.GetType(
                    "Unity.Entities.TransformAccessArrayInjectionHook");
            var componentArrayInjectionHookType =
                hybridAssembly.GetType("Unity.Entities.ComponentArrayInjectionHook");

            InjectionHookSupport.RegisterHook(
                (InjectionHook) Activator.CreateInstance(gameObjectArrayInjectionHookType));
            InjectionHookSupport.RegisterHook(
                (InjectionHook) Activator.CreateInstance(transformAccessArrayInjectionHookType));
            InjectionHookSupport.RegisterHook(
                (InjectionHook) Activator.CreateInstance(componentArrayInjectionHookType));
        }

        private static void DomainUnloadShutdown()
        {
            World.DisposeAllWorlds();
            ScriptBehaviourUpdateOrder.UpdatePlayerLoop();
        }
    }
}
