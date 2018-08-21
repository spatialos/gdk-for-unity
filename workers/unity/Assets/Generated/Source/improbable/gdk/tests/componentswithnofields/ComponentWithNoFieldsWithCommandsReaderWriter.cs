
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Entity = Unity.Entities.Entity;

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
    {
        public partial class Requirables
        {
            [InjectableId(InjectableType.ReaderWriter, 1005)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 1005)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<SpatialOSComponentWithNoFieldsWithCommands, SpatialOSComponentWithNoFieldsWithCommands.Update>
            {
            }

            [InjectableId(InjectableType.ReaderWriter, 1005)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : IWriter<SpatialOSComponentWithNoFieldsWithCommands, SpatialOSComponentWithNoFieldsWithCommands.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<SpatialOSComponentWithNoFieldsWithCommands, SpatialOSComponentWithNoFieldsWithCommands.Update>, Reader, Writer
            {
                public ReaderWriterImpl(Entity entity,EntityManager entityManager,ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                protected override void TriggerFieldCallbacks(SpatialOSComponentWithNoFieldsWithCommands.Update update)
                {
                }
                protected override void ApplyUpdate(SpatialOSComponentWithNoFieldsWithCommands.Update update, ref SpatialOSComponentWithNoFieldsWithCommands data)
                {
                }
            }
        }
    }
}
