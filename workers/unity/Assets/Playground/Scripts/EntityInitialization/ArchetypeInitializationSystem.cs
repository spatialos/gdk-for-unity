using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    /// <summary>
    ///     Adds a list of components to newly spawned entities according to an archetype definition.
    /// </summary>
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.EntityInitialisationGroup))]
    public class ArchetypeInitializationSystem : ComponentSystem
    {
        public struct Data
        {
            public readonly int Length;
            [ReadOnly] public ComponentArray<SpatialOSArchetypeComponent> ArchetypeComponents;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<NewlyAddedSpatialOSEntity> NewlyCreatedEntities;
        }

        [Inject] private Data data;

        public struct WorkerData
        {
            public readonly int Length;
            [ReadOnly] public SharedComponentDataArray<WorkerConfig> WorkerConfigs;
        }

        [Inject] private WorkerData workerData;

        private const string LoggerName = "ArchetypeInitializationSystem";
        private const string ArchetypeMappingNotFound = "No corresponding archetype mapping found.";

        private const string UnsupportedArchetype =
            "Worker type isn't supported by the ArchetypeInitializationSystem.";

        private ViewCommandBuffer viewCommandBuffer;

        private readonly MethodInfo addComponentMethod = typeof(EntityCommandBuffer).GetMethods().First(method =>
            method.Name == "AddComponent" && method.GetParameters()[0].ParameterType == typeof(Entity));


        private readonly Dictionary<Type, bool> typeToIsComponentData = new Dictionary<Type, bool>();

        private readonly Dictionary<Type, MethodInfo> typeToAddComponentGenericMethodInfo =
            new Dictionary<Type, MethodInfo>();

        protected override void OnUpdate()
        {
            if (workerData.Length == 0)
            {
                new LoggingDispatcher().HandleLog(LogType.Error, new LogEvent("This system should not have been run without a worker entity"));
            }

            var worker = workerData.WorkerConfigs[0].Worker;
            if (!(SystemConfig.UnityGameLogic.Equals(worker.WorkerType)) && !(SystemConfig.UnityClient.Equals(worker.WorkerType)))
            {
                worker.LogDispatcher.HandleLog(LogType.Error, new LogEvent(UnsupportedArchetype)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField("WorldName", World.Name)
                    .WithField("WorkerType", worker));
            }

            if (viewCommandBuffer == null)
            {
                viewCommandBuffer = new ViewCommandBuffer(worker.LogDispatcher);
            }
            for (var i = 0; i < data.Length; i++)
            {
                var archetypeName = data.ArchetypeComponents[i].ArchetypeName;
                var entity = data.Entities[i];

                if (!ArchetypeConfig.WorkerTypeToArchetypeNameToComponentTypes[worker.WorkerType]
                    .TryGetValue(archetypeName, out var componentTypesToAdd))
                {
                    worker.LogDispatcher.HandleLog(LogType.Error, new LogEvent(ArchetypeMappingNotFound)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("ArchetypeName", archetypeName)
                        .WithField("WorkerType", worker.WorkerType));
                    continue;
                }

                foreach (var componentType in componentTypesToAdd)
                {
                    var type = componentType.GetManagedType();
                    var componentInstance = Activator.CreateInstance(type);

                    bool isComponentData;
                    if (!typeToIsComponentData.TryGetValue(type, out isComponentData))
                    {
                        isComponentData = type.GetInterfaces().Contains(typeof(IComponentData));
                        typeToIsComponentData[type] = isComponentData;
                    }

                    if (isComponentData)
                    {
                        MethodInfo addComponentMethodGeneric;
                        if (!typeToAddComponentGenericMethodInfo.TryGetValue(type, out addComponentMethodGeneric))
                        {
                            addComponentMethodGeneric = addComponentMethod.MakeGenericMethod(type);
                            typeToAddComponentGenericMethodInfo[type] = addComponentMethodGeneric;
                        }

                        addComponentMethodGeneric.Invoke(PostUpdateCommands,
                            new [] { entity, componentInstance });
                    }
                    else
                    {
                        viewCommandBuffer.AddComponent(entity, componentType, componentInstance);
                    }
                }
            }

            viewCommandBuffer.FlushBuffer(EntityManager);
        }
    }
}
