// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public class NonBlittableComponentViewStorage : IViewStorage, IViewComponentStorage<Snapshot>,
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
                

                if (update.BoolField.HasValue)
                {
                    data.BoolField = update.BoolField.Value;
                }

                if (update.IntField.HasValue)
                {
                    data.IntField = update.IntField.Value;
                }

                if (update.LongField.HasValue)
                {
                    data.LongField = update.LongField.Value;
                }

                if (update.FloatField.HasValue)
                {
                    data.FloatField = update.FloatField.Value;
                }

                if (update.DoubleField.HasValue)
                {
                    data.DoubleField = update.DoubleField.Value;
                }

                if (update.StringField.HasValue)
                {
                    data.StringField = update.StringField.Value;
                }

                if (update.OptionalField.HasValue)
                {
                    data.OptionalField = update.OptionalField.Value;
                }

                if (update.ListField.HasValue)
                {
                    data.ListField = update.ListField.Value;
                }

                if (update.MapField.HasValue)
                {
                    data.MapField = update.MapField.Value;
                }

                componentData[entityId] = data;
            }
        }
    }
}
