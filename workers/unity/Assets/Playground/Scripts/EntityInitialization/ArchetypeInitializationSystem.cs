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
    public class ArchetypeInitializationSystem : SpatialOSSystem
    {
        public struct Data
        {
            public readonly int Length;
            [ReadOnly] public ComponentArray<SpatialOSArchetypeComponent> ArchetypeComponents;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<NewlyAddedSpatialOSEntity> NewlyCreatedEntities;
        }

        [Inject] private Data data;

        private const string LoggerName = "ArchetypeInitializationSystem";
        private const string ArchetypeMappingNotFound = "No corresponding archetype mapping found.";

        private const string UnsupportedArchetype =
            "Worker type isn't supported by the ArchetypeInitializationSystem.";

        private WorkerBase worker;
        private readonly ViewCommandBuffer viewCommandBuffer = new ViewCommandBuffer();

        private readonly MethodInfo addComponentMethod = typeof(EntityCommandBuffer).GetMethods().First(method =>
            method.Name == "AddComponent" && method.GetParameters()[0].ParameterType == typeof(Entity));

        private string workerType;

        private readonly Dictionary<Type, bool> typeToIsComponentData = new Dictionary<Type, bool>();

        private readonly Dictionary<Type, MethodInfo> typeToAddComponentGenericMethodInfo =
            new Dictionary<Type, MethodInfo>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            worker = WorkerRegistry.GetWorkerForWorld(World);

            if (!(worker is UnityClient) && !(worker is UnityGameLogic))
            {
                worker.LogDispatcher.HandleLog(LogType.Error, new LogEvent(UnsupportedArchetype)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField("WorldName", World.Name)
                    .WithField("WorkerType", worker));
            }

            workerType = worker.GetType().Name;
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var archetypeName = data.ArchetypeComponents[i].ArchetypeName;
                var entity = data.Entities[i];

                ComponentType[] componentTypesToAdd;
                if (!ArchetypeConfig.WorkerTypeToArchetypeNameToComponentTypes[workerType]
                    .TryGetValue(archetypeName, out componentTypesToAdd))
                {
                    worker.LogDispatcher.HandleLog(LogType.Error, new LogEvent(ArchetypeMappingNotFound)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("ArchetypeName", archetypeName)
                        .WithField("WorkerType", workerType));
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
                            new object[] { entity, componentInstance });
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
