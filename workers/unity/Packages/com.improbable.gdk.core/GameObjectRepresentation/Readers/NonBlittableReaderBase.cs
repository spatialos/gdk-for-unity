using System;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public abstract class NonBlittableReaderBase<TSpatialComponentData, TComponentUpdate>
        : ReaderBase<TSpatialComponentData, TComponentUpdate>
        where TSpatialComponentData : Component, ISpatialComponentData
        where TComponentUpdate : ISpatialComponentUpdate
    {
        protected NonBlittableReaderBase(Entity entity, EntityManager entityManager) : base(entity, entityManager)
        {
        }

        public override TSpatialComponentData Data
        {
            get
            {
                try
                {
                    return EntityManager.GetComponentObject<TSpatialComponentData>(Entity);
                }
                catch (Exception e)
                {
                    throw new Exception("TODO throw same exception as BlittableReaderBase", e);
                }
            }
        }
    }
}
