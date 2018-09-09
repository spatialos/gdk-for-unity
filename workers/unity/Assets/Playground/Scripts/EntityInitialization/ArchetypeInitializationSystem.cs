using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Gdk.TransformSynchronization;
using Unity.Collections;
using Unity.Entities;

namespace Playground
{
    /// <summary>
    ///     Optionally adds a list of components to newly spawned entities according to an archetype definition.
    /// </summary>
    [UpdateInGroup(typeof(EntityInitialization))]
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

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            worker = World.GetExistingManager<WorkerSystem>();
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var archetype = data.ArchetypeComponents[i].ArchetypeName;
                var entity = data.Entities[i];

                switch (worker.WorkerType)
                {
                    // case WorkerUtils.UnityClient when archetype == ArchetypeConfig.CharacterArchetype:
                    // case WorkerUtils.UnityClient when archetype == ArchetypeConfig.CubeArchetype:
                    // case WorkerUtils.UnityClient when archetype == ArchetypeConfig.SpinnerArchetype:
                    // case WorkerUtils.UnityGameLogic when archetype == ArchetypeConfig.CharacterArchetype:
                    // case WorkerUtils.UnityGameLogic when archetype == ArchetypeConfig.CubeArchetype:
                    // case WorkerUtils.UnityGameLogic when archetype == ArchetypeConfig.SpinnerArchetype:
                    //     PostUpdateCommands.AddBuffer<BufferedTransform>(entity);
                    //     break;
                    default:
                        break;
                }
            }
        }
    }
}
