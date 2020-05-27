using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using Unity.Profiling;

namespace Improbable.Gdk.Debug.WorkerInspector
{
    internal class EntityListData
    {
        private static ProfilerMarker refreshDataMarker = new ProfilerMarker("EntityList.RefreshData");
        private static ProfilerMarker applySearchMarker = new ProfilerMarker("EntityList.ApplySearch");

        public readonly List<EntityData> Data = new List<EntityData>();

        private readonly List<EntityData> fullData = new List<EntityData>();
        private World world;
        private EntityQuery query;
        private EntitySearchParameters searchParameters;

        public void ApplySearch(EntitySearchParameters searchParameters)
        {
            this.searchParameters = searchParameters;

            using (applySearchMarker.Auto())
            {
                Data.Clear();

                foreach (var datum in fullData)
                {
                    if (datum.Matches(searchParameters))
                    {
                        Data.Add(datum);
                    }
                }
            }
        }

        public void SetNewWorld(World newWorld)
        {
            fullData.Clear();
            Data.Clear();
            query?.Dispose();

            if (newWorld == null)
            {
                world = null;
                query = null;
                return;
            }

            // Need to refresh the query.
            world = newWorld;
            query = world.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<SpatialEntityId>());
        }

        public void RefreshData()
        {
            if (world == null)
            {
                return;
            }

            using (refreshDataMarker.Auto())
            {
                fullData.Clear();
                var spatialOSComponentType = world.EntityManager.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var metadataComponentType =
                    world.EntityManager.GetArchetypeChunkComponentType<Metadata.Component>(true);
                var ecsEntityType = world.EntityManager.GetArchetypeChunkEntityType();

                using (var chunks = query.CreateArchetypeChunkArray(Allocator.TempJob))
                {
                    foreach (var chunk in chunks)
                    {
                        NativeArray<Metadata.Component>? metadataArray = null;

                        if (chunk.Has(metadataComponentType))
                        {
                            metadataArray = chunk.GetNativeArray(metadataComponentType);
                        }

                        var entityIdArray = chunk.GetNativeArray(spatialOSComponentType);
                        var entities = chunk.GetNativeArray(ecsEntityType);

                        for (var i = 0; i < entities.Length; i++)
                        {
                            var data = new EntityData(entityIdArray[i].EntityId, metadataArray?[i].EntityType);
                            fullData.Add(data);
                        }
                    }
                }

                fullData.Sort();
            }

            ApplySearch(searchParameters);
        }
    }

    internal readonly struct EntityData : IComparable<EntityData>, IComparable, IEquatable<EntityData>
    {
        public readonly EntityId EntityId;
        public readonly string Metadata;

        public EntityData(EntityId entityId, string metadata)
        {
            EntityId = entityId;
            Metadata = metadata;
        }

        public bool Matches(EntitySearchParameters searchParameters)
        {
            if (searchParameters.EntityId.HasValue)
            {
                return searchParameters.EntityId.Value == EntityId;
            }

            if (!string.IsNullOrEmpty(searchParameters.SearchFragment))
            {
                return Metadata.ToLower().Contains(searchParameters.SearchFragment);
            }

            return true;
        }

        public override string ToString()
        {
            return Metadata != null ? $"{Metadata} ({EntityId})" : EntityId.ToString();
        }

        public int CompareTo(EntityData other)
        {
            return EntityId.CompareTo(other.EntityId);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return 1;
            }

            return obj is EntityData other
                ? CompareTo(other)
                : throw new ArgumentException($"Object must be of type {nameof(EntityData)}");
        }

        public bool Equals(EntityData other)
        {
            return EntityId.Equals(other.EntityId) && Metadata == other.Metadata;
        }

        public override bool Equals(object obj)
        {
            return obj is EntityData other && Equals(other);
        }

        public static bool operator ==(EntityData left, EntityData right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EntityData left, EntityData right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EntityId.GetHashCode() * 397) ^ (Metadata != null ? Metadata.GetHashCode() : 0);
            }
        }
    }
}
