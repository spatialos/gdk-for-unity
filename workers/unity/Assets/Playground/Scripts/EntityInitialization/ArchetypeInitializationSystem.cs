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

        private MutableView view;
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

            var worker = WorkerRegistry.GetWorkerForWorld(World);
            view = worker.View;

            if (!(worker is UnityClient) && !(worker is UnityGameLogic))
            {
                view.LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                        "Worker type isn't supported by the ArchetypeInitializationSystem.")
                    .WithField("WorldName", World.Name)
                    .WithField("WorkerType", worker));
            }

            workerType = worker.GetWorkerType;
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
                    view.LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "No corresponding archetype mapping found.")
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

            viewCommandBuffer.FlushBuffer(view);
        }
    }
}
