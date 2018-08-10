
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Gdk.Core.MonoBehaviours;
using Entity = Unity.Entities.Entity;

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
    {
        [ComponentId(1005)]
        internal class ReaderWriterCreator : IReaderWriterCreator
        {
            public IReaderWriterInternal CreateReaderWriter(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
                return new ReaderWriterImpl(entity, entityManager, logDispatcher);
            }
        }

        [ReaderInterface]
        [ComponentId(1005)]
        public interface Reader : IReader<SpatialOSComponentWithNoFieldsWithCommands>
        {
        }

        [WriterInterface]
        [ComponentId(1005)]
        public interface Writer : IWriter<SpatialOSComponentWithNoFieldsWithCommands>
        {
        }

        internal class ReaderWriterImpl : ReaderWriterBase<SpatialOSComponentWithNoFieldsWithCommands>, Reader, Writer
        {
            public ReaderWriterImpl(Entity entity,EntityManager entityManager,ILogDispatcher logDispatcher)
                : base(entity, entityManager, logDispatcher)
            {
            }

            public void OnCmdCommandRequest(Cmd.Request request)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
