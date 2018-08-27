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
    [DisableAutoCreation]
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

        private const string UnsupportedArchetype =
            "Worker type isn't supported by the ArchetypeInitializationSystem.";

        private Worker worker;

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
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var archetypeName = data.ArchetypeComponents[i].ArchetypeName;
                var entity = data.Entities[i];
                
                ArchetypeConfig.AddComponentDataForArchetype(worker, archetypeName, PostUpdateCommands, entity);
            }            
        }
    }
}
