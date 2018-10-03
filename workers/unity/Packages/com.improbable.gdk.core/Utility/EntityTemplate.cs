using System;
using Improbable.Worker.Core;

namespace Improbable.Gdk.Core
{
    public class EntityTemplate
    {
        internal readonly Entity template;
        private bool hasBuild;

        internal EntityTemplate(Entity entity)
        {
            template = entity;
        }

        public Entity GetEntity()
        {
            if (hasBuild)
            {
                throw new InvalidOperationException("Cannot create an entity using the same EntityTemplate twice.");
            }

            hasBuild = true;
            return template;
        }
    }
}
