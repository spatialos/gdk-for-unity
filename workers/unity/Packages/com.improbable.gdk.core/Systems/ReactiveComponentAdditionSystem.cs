using System.Diagnostics;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateAfter(typeof(SpatialOSReceiveSystem))]
    public class ReactiveComponentAdditionSystem : ComponentSystem
    {
        private MutableView view;

        ViewCommandBuffer viewCommandBuffer = new ViewCommandBuffer();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            view = WorkerRegistry.GetWorkerForWorld(World).View;

            GenerateComponentGroups();
        }

        private void GenerateComponentGroups()
        {
            foreach (var componentTranslator in view.TranslationUnits.Values)
            {
                var newEntityComponentGroup = GetComponentGroup(componentTranslator.NewEntityComponentTypes);
                componentTranslator.NewEntityComponentGroup = newEntityComponentGroup;
            }
        }

        protected override void OnUpdate()
        {
            foreach (var translationUnit in view.TranslationUnits.Values)
            {
                translationUnit.PostReceive(ref viewCommandBuffer);
            }

            viewCommandBuffer.FlushBuffer(view);
        }
    }
}
