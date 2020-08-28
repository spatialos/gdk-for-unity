using System.Collections.Generic;

namespace Improbable.Gdk.Core.SceneAuthoring
{
    public interface IConvertGameObjectToSpatialOsEntity
    {
        List<ConvertedEntity> Convert();
    }

    public readonly struct ConvertedEntity
    {
        public readonly EntityId? EntityId;
        public readonly EntityTemplate Template;

        public ConvertedEntity(EntityId entityId, EntityTemplate template)
        {
            EntityId = entityId;
            Template = template;
        }

        public ConvertedEntity(EntityTemplate template)
        {
            EntityId = null;
            Template = template;
        }
    }
}
