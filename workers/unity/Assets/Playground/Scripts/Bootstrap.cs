using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Playground
{
    public class Bootstrap : MonoBehaviour
    {
        public GameObject Level;

        private const int TargetFrameRate = -1; // Turns off VSync

        public const string LoggerName = "Bootstrap";

        private static Dictionary<SpatialOSWorld, ConnectionConfig> WorldToConfig = new Dictionary<SpatialOSWorld, ConnectionConfig>();

        public void Awake()
        {
            InitializeWorkerTypes();
            // Taken from DefaultWorldInitalization.cs
            SetupInjectionHooks(); // Register hybrid injection hooks
            PlayerLoopManager.RegisterDomainUnload(DomainUnloadShutdown, 10000); // Clean up worlds and player loop

            Application.targetFrameRate = TargetFrameRate;
            if (Application.isEditor)
            {
#if UNITY_EDITOR
                var workerConfigurations =
                    AssetDatabase.LoadAssetAtPath<ScriptableWorkerConfiguration>(ScriptableWorkerConfiguration
                        .AssetPath);
                foreach (var workerConfig in workerConfigurations.WorkerConfigurations)
                {
                    if (!workerConfig.IsEnabled)
                    {
                        continue;
                    }
                    var config = new ReceptionistConfig();
                    config.UseExternalIp = workerConfigurations.UseExternalIp;
                    config.WorkerType = workerConfig.Type;
                    config.WorkerId = $"{config.WorkerType}-{Guid.NewGuid()}";
                    WorldToConfig.Add(new SpatialOSWorld(config.WorkerId, workerConfig.Origin), config);
                } 
#endif
            }
            else
            {
                var commandLineArguments = System.Environment.GetCommandLineArgs();
                Debug.LogFormat("Command line {0}", string.Join(" ", commandLineArguments.ToArray()));
                var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);


                var connectionConfig = ConnectionUtility.CreateConnectionConfigFromCommandLine(commandLineArgs);
                if (string.Empty.Equals(connectionConfig.WorkerType))
                {
                    // because the launcher does not pass in the worker type...
                    connectionConfig.WorkerType = nameof(UnityClient);
                }
                WorldToConfig.Add(new SpatialOSWorld(connectionConfig.WorkerId, Vector3.zero), connectionConfig);
            }

            if (World.AllWorlds.Count <= 0)
            {
                throw new InvalidConfigurationException(
                    "No worlds have been created, due to invalid Worker types being specified. Check the config in" +
                    "Improbable -> Configure editor workers.");
            }

            var worlds = World.AllWorlds.ToArray();
            ScriptBehaviourUpdateOrder.UpdatePlayerLoop(worlds);
            // Systems don't tick if World.Active isn't set
            World.Active = worlds[0];
        }

        public void Start()
        {
            foreach (KeyValuePair<SpatialOSWorld, ConnectionConfig> worldConfigPair in WorldToConfig)
            {
                LoadLevel(Vector3.zero);

                try
                {
                    worldConfigPair.Key.Connect(worldConfigPair.Value, new ForwardingDispatcher());

                }
                catch (ConnectionFailedException exception)
                {
                    new LoggingDispatcher().HandleLog(LogType.Error, new LogEvent(exception.Message)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("Reason", exception.Reason));
                }
            }
            ScriptBehaviourUpdateOrder.UpdatePlayerLoop(World.AllWorlds.ToArray());
        }

        public static void InitializeWorkerTypes()
        {
            WorkerRegistry.RegisterWorkerType<UnityClient>();
            WorkerRegistry.RegisterWorkerType<UnityGameLogic>();
        }

        public static void SetupInjectionHooks()
        {
            // Reflection to get internal hook classes. Doesn't seem to be a proper way to do this.
            var gameObjectArrayInjectionHookType =
                typeof(Unity.Entities.GameObjectEntity).Assembly.GetType("Unity.Entities.GameObjectArrayInjectionHook");
            var transformAccessArrayInjectionHookType =
                typeof(Unity.Entities.GameObjectEntity).Assembly.GetType(
                    "Unity.Entities.TransformAccessArrayInjectionHook");
            var componentArrayInjectionHookType =
                typeof(Unity.Entities.GameObjectEntity).Assembly.GetType("Unity.Entities.ComponentArrayInjectionHook");

            InjectionHookSupport.RegisterHook(
                (InjectionHook) Activator.CreateInstance(gameObjectArrayInjectionHookType));
            InjectionHookSupport.RegisterHook(
                (InjectionHook) Activator.CreateInstance(transformAccessArrayInjectionHookType));
            InjectionHookSupport.RegisterHook(
                (InjectionHook) Activator.CreateInstance(componentArrayInjectionHookType));
        }

        public static void DomainUnloadShutdown()
        {
            // because otherwise the world won't get cleaned up and unity stops working:
            foreach (var world in WorldToConfig.Keys) {
                world.Dispose();
            }

            World.DisposeAllWorlds();
            ScriptBehaviourUpdateOrder.UpdatePlayerLoop();
        }

        private void LoadLevel(Vector3 origin)
        {
            Instantiate(Level, origin, Quaternion.identity);
        }
    }
}
