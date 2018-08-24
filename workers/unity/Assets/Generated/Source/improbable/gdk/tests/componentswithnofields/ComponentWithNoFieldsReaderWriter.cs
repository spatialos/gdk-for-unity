
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
    public partial class ComponentWithNoFields
    {
        public partial class Requirables
        {
            [InjectableId(InjectableType.ReaderWriter, 1003)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 1003)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<SpatialOSComponentWithNoFields, SpatialOSComponentWithNoFields.Update>
            {
            }

            [InjectableId(InjectableType.ReaderWriter, 1003)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : IWriter<SpatialOSComponentWithNoFields, SpatialOSComponentWithNoFields.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<SpatialOSComponentWithNoFields, SpatialOSComponentWithNoFields.Update>, Reader, Writer
            {
                public ReaderWriterImpl(Entity entity,EntityManager entityManager,ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                protected override void TriggerFieldCallbacks(SpatialOSComponentWithNoFields.Update update)
                {
                }
                protected override void ApplyUpdate(SpatialOSComponentWithNoFields.Update update, ref SpatialOSComponentWithNoFields data)
                {
                }
            }
        }
    }
}
