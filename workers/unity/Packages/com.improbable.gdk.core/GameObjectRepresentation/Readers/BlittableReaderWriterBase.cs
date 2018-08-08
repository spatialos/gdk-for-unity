using System;
using System.IO;
using Improbable.Gdk.Core.MonoBehaviours;
using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    internal abstract class BlittableReaderWriterBase<TSpatialComponentData, TComponentUpdate>
        : ReaderWriterBase<TSpatialComponentData, TComponentUpdate>
        where TSpatialComponentData : struct, ISpatialComponentData, IComponentData
        where TComponentUpdate : ISpatialComponentUpdate
    {
        protected BlittableReaderWriterBase(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher) : base(entity, entityManager, logDispatcher)
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

        public override void Send(TComponentUpdate update)
        {
            try
            {
                var data = EntityManager.GetComponentData<TSpatialComponentData>(Entity);
                UpdateData(ref data, update);
                EntityManager.SetComponentData(Entity, data);
            }
            catch (Exception e)
            {
                throw new WriterDataUpdateFailedException(e);
            }
        }

        protected abstract void UpdateData(ref TSpatialComponentData data, TComponentUpdate update);
    }
}
