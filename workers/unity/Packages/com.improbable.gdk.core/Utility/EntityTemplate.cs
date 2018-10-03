using System;
using Improbable.Worker.Core;

namespace Improbable.Gdk.Core
{
    public class EntityTemplate
    {
        internal readonly Entity template; //Internal for tests
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
    }
}
