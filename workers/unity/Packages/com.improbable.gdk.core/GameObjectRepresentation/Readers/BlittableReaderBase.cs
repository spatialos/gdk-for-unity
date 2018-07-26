using System;
using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public abstract class BlittableReaderBase<TSpatialComponentData, TComponentUpdate>
        : ReaderBase<TSpatialComponentData, TComponentUpdate>
        where TSpatialComponentData : struct, ISpatialComponentData, IComponentData
        where TComponentUpdate : ISpatialComponentUpdate
    {
        protected BlittableReaderBase(Entity entity, EntityManager entityManager) : base(entity, entityManager)
        {
        }

        public override TSpatialComponentData Data
        {
            get
            {
                try
                {
                    return EntityManager.GetComponentData<TSpatialComponentData>(Entity);
                }
                catch (Exception e)
                {
                    throw new Exception("TODO write exception message here", e);
                }
            }
        }
    }
}
