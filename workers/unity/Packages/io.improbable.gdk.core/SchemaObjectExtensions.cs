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
    }
}
