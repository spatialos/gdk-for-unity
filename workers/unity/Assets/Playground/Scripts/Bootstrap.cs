using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
#if UNITY_ANDROID
using Improbable.Gdk.Android;
#endif
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Playground
{
    public class Bootstrap : MonoBehaviour
    {
        public GameObject Level;

        private const int TargetFrameRate = -1; // Turns off VSync

        private static readonly List<Worker> Workers = new List<Worker>();

        private const string ipRegEx =
            @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";

        private GameObject connectionPanel;
        private InputField connectParamInputField;
        private Button connectButton;
        private bool connectionRequested;
        private Text errorField;

        public void Awake()
        {
            // Taken from DefaultWorldInitalization.cs
            SetupInjectionHooks(); // Register hybrid injection hooks
            PlayerLoopManager.RegisterDomainUnload(DomainUnloadShutdown, 10000); // Clean up worlds and player loop

            Application.targetFrameRate = TargetFrameRate;
            Worker.OnConnect += w => Debug.Log($"{w.WorkerId} is connecting");
            Worker.OnDisconnect += w => Debug.Log($"{w.WorkerId} is disconnecting");
            PlayerLifecycleConfig.CreatePlayerEntityTemplate = PlayerTemplate.CreatePlayerEntityTemplate;

            connectionPanel = GameObject.FindGameObjectWithTag("ConnectionPanel");
            connectParamInputField = connectionPanel.transform.Find("ConnectParam").GetComponent<InputField>();
            connectParamInputField.text = PlayerPrefs.GetString("cachedIp");
            connectButton = connectionPanel.transform.Find("ConnectButton").GetComponent<Button>();
            connectButton.onClick.AddListener(Connect);
            errorField = connectionPanel.transform.Find("ConnectionError").GetComponent<Text>();
            if (!Application.isMobilePlatform)
            {
                connectParamInputField.gameObject.SetActive(false);
            }

            if (Application.isEditor)
            {
                var config = new ReceptionistConfig
                {
                    WorkerType = SystemConfig.UnityGameLogic,
                };
                CreateWorker(config, new Vector3(500, 0, 0));
            }
            else if (!Application.isMobilePlatform)
            {
                var commandLineArguments = System.Environment.GetCommandLineArgs();
                Debug.LogFormat("Command line {0}", string.Join(" ", commandLineArguments.ToArray()));
                var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
                var config = ConnectionUtility.CreateConnectionConfigFromCommandLine(commandLineArgs);
                CreateWorker(config, Vector3.zero);
                if (World.AllWorlds.Count <= 0)
                {
                    throw new InvalidConfigurationException(
                        "No worlds have been created, due to invalid worker types being specified. Check the config in" +
                        "Improbable -> Configure editor workers.");
                }

                var worlds = World.AllWorlds.ToArray();
                ScriptBehaviourUpdateOrder.UpdatePlayerLoop(worlds);
                // Systems don't tick if World.Active isn't set
                World.Active = worlds[0];
            }
        }

        private void Connect()
        {
            if (Application.isEditor)
            {
                var config = new ReceptionistConfig
                {
                    WorkerType = SystemConfig.UnityClient,
                };
                CreateWorker(config, Vector3.zero);
            }
#if UNITY_IOS
            else
            {
                ConnectionConfig config;
                config = ReceptionistConfig.CreateConnectionConfigForiOS();
                config.WorkerType = SystemConfig.UnityClient;
                CreateWorker(config, Vector3.zero);
            }
#elif UNITY_ANDROID
            else if (Application.isMobilePlatform)
            {
                ConnectionConfig config;
                if (DeviceInfo.IsAndroidStudioEmulator() && connectParamInputField.text.Equals(string.Empty))
                {
                    config = ReceptionistConfig.CreateConnectionConfigForAndroidEmulator();
                    config.WorkerType = SystemConfig.UnityClient;
                }
                else
                {
                    config = SetConnectionParameters(connectParamInputField.text);
                    config.WorkerType = SystemConfig.UnityClient;
                    PlayerPrefs.SetString("cachedIp", connectParamInputField.text);
                    PlayerPrefs.Save();
                }

                CreateWorker(config, Vector3.zero);
            }
#endif

            if (World.AllWorlds.Count <= 0)
            {
                throw new InvalidConfigurationException(
                    "No worlds have been created, due to invalid worker types being specified. Check the config in" +
                    "Improbable -> Configure editor workers.");
            }

            var worlds = World.AllWorlds.ToArray();
            ScriptBehaviourUpdateOrder.UpdatePlayerLoop(worlds);
            // Systems don't tick if World.Active isn't set
            World.Active = worlds[0];

            connectionPanel.gameObject.SetActive(false);
        }

        private ConnectionConfig SetConnectionParameters(string param)
        {
            if (IsIpAddress(param))
            {
                return ReceptionistConfig.CreateConnectionConfigForPhysicalAndroid(param);
            }
            else
            {
                connectionRequested = false;
                errorField.text = "Entered text is not a valid IP address.";
            }

            return null;
            // TODO: UTY-558 else -> cloud connection
        }

        private static bool IsIpAddress(string param)
        {
            return Regex.Match(param, ipRegEx).Success;
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
            foreach (var worker in Workers)
            {
                worker.Dispose();
            }

            World.DisposeAllWorlds();
            ScriptBehaviourUpdateOrder.UpdatePlayerLoop();
        }

        private void CreateWorker(ConnectionConfig config, Vector3 origin)
        {
            var worker = Worker.Connect(config, new ForwardingDispatcher(), origin);
            Instantiate(Level, origin, Quaternion.identity);
            SystemConfig.AddSystems(worker.World, config.WorkerType);
            Workers.Add(worker);
        }
    }
}
