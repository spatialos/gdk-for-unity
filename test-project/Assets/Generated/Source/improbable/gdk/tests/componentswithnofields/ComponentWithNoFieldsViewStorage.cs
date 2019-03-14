// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFields
    {
        public class ComponentWithNoFieldsViewStorage : IViewStorage, IViewComponentStorage<Snapshot>,
            IViewComponentUpdater<Update>
        {
            private readonly Dictionary<long, Authority> authorityStates = new Dictionary<long, Authority>();
            private readonly Dictionary<long, Snapshot> componentData = new Dictionary<long, Snapshot>();

            public Type GetSnapshotType()
            {
                return typeof(Snapshot);
            }

            public Type GetUpdateType()
            {
                return typeof(Update);
            }

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public bool HasComponent(long entityId)
            {
                return componentData.ContainsKey(entityId);
            }

            public Snapshot GetComponent(long entityId)
            {
                if (!componentData.TryGetValue(entityId, out var component))
                {
                    throw new ArgumentException($"Entity with Entity ID {entityId} does not have component {typeof(Snapshot)} in the view.");
                }

                return component;
            }

            public Authority GetAuthority(long entityId)
            {
                if (!authorityStates.TryGetValue(entityId, out var authority))
                {
                    throw new ArgumentException($"Entity with Entity ID {entityId} does not have component {typeof(Snapshot)} in the view.");
                }

                return authority;
            }

            public void ApplyDiff(ViewDiff viewDiff)
            {
                var storage = viewDiff.GetComponentDiffStorage(ComponentId);

                foreach (var entity in storage.GetComponentsAdded())
                {
                    authorityStates[entity.Id] = Authority.NotAuthoritative;
                    componentData[entity.Id] = new Snapshot();
                }

                foreach (var entity in storage.GetComponentsRemoved())
                {
                    authorityStates.Remove(entity.Id);
                    componentData.Remove(entity.Id);
                }

                var updates = ((IDiffUpdateStorage<Update>) storage).GetUpdates();
                for (var i = 0; i < updates.Count; i++)
                {
                    ref readonly var update = ref updates[i];
                    ApplyUpdate(update.EntityId.Id, in update.Update);
                }

                var authorityChanges = ((IDiffAuthorityStorage) storage).GetAuthorityChanges();
                for (var i = 0; i < authorityChanges.Count; i++)
                {
                    var authorityChange = authorityChanges[i];
                    authorityStates[authorityChange.EntityId.Id] = authorityChange.Authority;
                }
            }

            public void ApplyUpdate(long entityId, in Update update)
            {
                if (!componentData.TryGetValue(entityId, out var data)) 
                {
                    return;
                }
                

                componentData[entityId] = data;
            }
        }
    }
}
