using System;
using System.IO;
using Improbable.Gdk.Core.MonoBehaviours;
using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public abstract class BlittableReaderBase<TSpatialComponentData, TComponentUpdate>
        : ReaderBase<TSpatialComponentData, TComponentUpdate>
        where TSpatialComponentData : struct, ISpatialComponentData, IComponentData
        where TComponentUpdate : ISpatialComponentUpdate
    {
        protected BlittableReaderBase(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher) : base(entity, entityManager, logDispatcher)
        {
        }

        public sealed override TSpatialComponentData Data
        {
            get
            {
                try
                {
                    return EntityManager.GetComponentData<TSpatialComponentData>(Entity);
                }
                catch (Exception e)
                {
                    throw new ReaderDataGetFailedException(e);
                }
            }
        }
    }
}
