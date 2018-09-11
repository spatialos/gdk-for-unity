using System;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public static class WorldsInitializationHelper
    {
        public static void SetupInjectionHooks()
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

        public static void DomainUnloadShutdown()
        {
            World.DisposeAllWorlds();
            ScriptBehaviourUpdateOrder.UpdatePlayerLoop();
        }
    }
}
