using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Gdk.TransformSynchronization;
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
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<ArchetypeComponent.Component> ArchetypeComponents;
            [ReadOnly] public ComponentDataArray<NewlyAddedSpatialOSEntity> NewlyCreatedEntities;
        }

        [Inject] private Data data;

        private WorkerSystem worker;
        private const string LoggerName = "ArchetypeInitializationSystem";
        private const string ArchetypeMappingNotFound = "No corresponding archetype mapping found.";
        private const string UnsupportedArchetype = "Worker type isn't supported by the ArchetypeInitializationSystem.";

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            worker = World.GetExistingManager<WorkerSystem>();
            if (!WorkerUtils.UnityClient.Equals(worker.WorkerType) &&
                !WorkerUtils.UnityGameLogic.Equals(worker.WorkerType))
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
                var archetype = data.ArchetypeComponents[i].ArchetypeName;
                var entity = data.Entities[i];

                switch (worker.WorkerType)
                {
                    case WorkerUtils.UnityClient when archetype == ArchetypeConfig.CharacterArchetype:
                    case WorkerUtils.UnityClient when archetype == ArchetypeConfig.CubeArchetype:
                    case WorkerUtils.UnityClient when archetype == ArchetypeConfig.SpinnerArchetype:
                    case WorkerUtils.UnityGameLogic when archetype == ArchetypeConfig.CharacterArchetype:
                    case WorkerUtils.UnityGameLogic when archetype == ArchetypeConfig.CubeArchetype:
                    case WorkerUtils.UnityGameLogic when archetype == ArchetypeConfig.SpinnerArchetype:
                        PostUpdateCommands.AddBuffer<BufferedTransform>(entity);
                        break;
                    default:
                        worker.LogDispatcher.HandleLog(LogType.Error, new LogEvent(ArchetypeMappingNotFound)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField("ArchetypeName", archetype)
                            .WithField("WorkerType", worker.WorkerType));
                        break;
                }
            }
        }
    }
}
