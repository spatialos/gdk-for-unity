// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Tests
{
    public partial class ExhaustiveRepeated
    {
        public class ExhaustiveRepeatedViewStorage : IViewStorage, IViewComponentStorage<Snapshot>,
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
                

                if (update.Field1.HasValue)
                {
                    data.Field1 = update.Field1.Value;
                }

                if (update.Field2.HasValue)
                {
                    data.Field2 = update.Field2.Value;
                }

                if (update.Field3.HasValue)
                {
                    data.Field3 = update.Field3.Value;
                }

                if (update.Field4.HasValue)
                {
                    data.Field4 = update.Field4.Value;
                }

                if (update.Field5.HasValue)
                {
                    data.Field5 = update.Field5.Value;
                }

                if (update.Field6.HasValue)
                {
                    data.Field6 = update.Field6.Value;
                }

                if (update.Field7.HasValue)
                {
                    data.Field7 = update.Field7.Value;
                }

                if (update.Field8.HasValue)
                {
                    data.Field8 = update.Field8.Value;
                }

                if (update.Field9.HasValue)
                {
                    data.Field9 = update.Field9.Value;
                }

                if (update.Field10.HasValue)
                {
                    data.Field10 = update.Field10.Value;
                }

                if (update.Field11.HasValue)
                {
                    data.Field11 = update.Field11.Value;
                }

                if (update.Field12.HasValue)
                {
                    data.Field12 = update.Field12.Value;
                }

                if (update.Field13.HasValue)
                {
                    data.Field13 = update.Field13.Value;
                }

                if (update.Field14.HasValue)
                {
                    data.Field14 = update.Field14.Value;
                }

                if (update.Field15.HasValue)
                {
                    data.Field15 = update.Field15.Value;
                }

                if (update.Field16.HasValue)
                {
                    data.Field16 = update.Field16.Value;
                }

                if (update.Field17.HasValue)
                {
                    data.Field17 = update.Field17.Value;
                }

                if (update.Field18.HasValue)
                {
                    data.Field18 = update.Field18.Value;
                }

                componentData[entityId] = data;
            }
        }
    }
}
