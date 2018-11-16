using System;
using System.Collections.Generic;
using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public interface ISpecialCommandSender
    {
        void RegisterSelf(Dictionary<Type, Func<EntityManager, global::Unity.Entities.Entity, object, EntityId, object, object, uint?, bool, long>> callbacks);
    }
}

