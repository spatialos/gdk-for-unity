using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public static class SchemaObjectExtensions
    {
        public static void AddEntityId(this SchemaObject obj, uint fieldId, EntityId entityId)
        {
            obj.AddEntityId(fieldId, entityId.Id);
        }

        public static EntityId GetEntityIdStruct(this SchemaObject obj, uint fieldId)
        {
            return new EntityId(obj.GetEntityId(fieldId));
        }

        public static EntityId IndexEntityIdStruct(this SchemaObject obj, uint fieldId, uint index)
        {
            return new EntityId(obj.IndexEntityId(fieldId, index));
        }

        public static void AddEntity(this SchemaObject obj, uint fieldId, EntitySnapshot snapshot)
        {
            var entityObject = obj.AddObject(fieldId);
            snapshot.SerializeToSchemaObject(entityObject);
        }

        public static EntitySnapshot GetEntity(this SchemaObject obj, uint fieldId)
        {
            var entityObj = obj.GetObject(fieldId);
            return new EntitySnapshot(entityObj);
        }

        public static EntitySnapshot IndexEntity(this SchemaObject obj, uint fieldId, uint index)
        {
            var entityObj = obj.IndexObject(fieldId, index);
            return new EntitySnapshot(entityObj);
        }

        public static uint GetEntityCount(this SchemaObject obj, uint fieldId)
        {
            return obj.GetObjectCount(fieldId);
        }
    }
}
