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
        private struct Data
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<ArchetypeComponent.Component> ArchetypeComponents;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<NewlyAddedSpatialOSEntity> NewlyCreatedEntities;
        }

        [Inject] private Data data;

        private const string LoggerName = "ArchetypeInitializationSystem";
        private const string ArchetypeMappingNotFound = "No corresponding archetype mapping found.";

        private const string UnsupportedArchetype =
            "Worker type isn't supported by the ArchetypeInitializationSystem.";

        private ViewCommandBuffer viewCommandBuffer;

        private readonly MethodInfo addComponentMethod = typeof(EntityCommandBuffer).GetMethods().First(method =>
            method.Name == "AddComponent" && method.GetParameters()[0].ParameterType == typeof(Entity));

        private Worker worker;

        private readonly Dictionary<Type, bool> typeToIsComponentData = new Dictionary<Type, bool>();

        private readonly Dictionary<Type, MethodInfo> typeToAddComponentGenericMethodInfo =
            new Dictionary<Type, MethodInfo>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            worker = Worker.GetWorkerFromWorld(World);
            if (!SystemConfig.UnityClient.Equals(worker.WorkerType) &&
                !SystemConfig.UnityGameLogic.Equals(worker.WorkerType))
            {
                worker.LogDispatcher.HandleLog(LogType.Error, new LogEvent(UnsupportedArchetype)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField("WorldName", World.Name)
                    .WithField("WorkerType", worker));
            }

            viewCommandBuffer = new ViewCommandBuffer(EntityManager, worker.LogDispatcher);
        }

        protected override void OnUpdate()
        {
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

                    if (!typeToIsComponentData.TryGetValue(type, out var isComponentData))
                    {
                        isComponentData = type.GetInterfaces().Contains(typeof(IComponentData));
                        typeToIsComponentData[type] = isComponentData;
                    }

                    if (isComponentData)
                    {
                        if (!typeToAddComponentGenericMethodInfo.TryGetValue(type, out var addComponentMethodGeneric))
                        {
                            addComponentMethodGeneric = addComponentMethod.MakeGenericMethod(type);
                            typeToAddComponentGenericMethodInfo[type] = addComponentMethodGeneric;
                        }

                        addComponentMethodGeneric.Invoke(PostUpdateCommands,
                            new[] { entity, componentInstance });
                    }
                    else
                    {
                        viewCommandBuffer.AddComponent(entity, componentType, componentInstance);
                    }
                }
            }

            viewCommandBuffer.FlushBuffer();
        }
    }
}
