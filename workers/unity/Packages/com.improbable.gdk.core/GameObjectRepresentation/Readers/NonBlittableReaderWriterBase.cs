using System;
using Improbable.Gdk.Core.MonoBehaviours;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    internal abstract class NonBlittableReaderWriterBase<TSpatialComponentData, TComponentUpdate>
        : ReaderWriterBase<TSpatialComponentData, TComponentUpdate>
        where TSpatialComponentData : Component, ISpatialComponentData
        where TComponentUpdate : ISpatialComponentUpdate
    {
        protected NonBlittableReaderWriterBase(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher) : base(entity, entityManager, logDispatcher)
        {
        }

        public sealed override TSpatialComponentData Data
        {
            get
            {
                try
                {
                    return EntityManager.GetComponentObject<TSpatialComponentData>(Entity);
                }
                catch (Exception e)
                {
                    throw new ReaderDataGetFailedException(e, Entity.Index);
                }
            }
        }

        public override void Send(TComponentUpdate update)
        {
            try
            {
                var data = EntityManager.GetComponentObject<TSpatialComponentData>(Entity);
                ApplyUpdate(update, data);
            }
            catch (Exception e)
            {
                throw new WriterDataUpdateFailedException(e, Entity.Index);
            }
        }

        protected abstract void ApplyUpdate(TComponentUpdate update, TSpatialComponentData data);
    }
}
