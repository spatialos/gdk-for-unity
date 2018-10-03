using System;
using Improbable.Worker.Core;

namespace Improbable.Gdk.Core
{
    public class EntityTemplate : IDisposable
    {
        private readonly Entity template;
        private bool hasBeenUsed;

        internal EntityTemplate(Entity entity)
        {
            template = entity;
        }

        internal Entity GetEntity()
        {
            if (hasBeenUsed)
            {
                throw new InvalidOperationException("Cannot create an entity using the same EntityTemplate twice.");
            }

            hasBeenUsed = true;
            return template;
        }

        public void Dispose()
        {
            if (hasBeenUsed)
            {
                return;
            }

            hasBeenUsed = true;

            foreach (var id in template.GetComponentIds())
            {
                var componentData = template.Get(id);
                componentData?.SchemaData?.Dispose();
            }
        }
    }
}
