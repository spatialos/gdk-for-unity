
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFields
    {
        public partial class Requirable
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
            public interface Reader : IReader<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Component, Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Update>
            {
            }

            [InjectableId(InjectableType.ReaderWriter, 1003)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : Reader, IWriter<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Component, Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Component, Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Update>, Reader, Writer
            {
                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                protected override void TriggerFieldCallbacks(Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Update update)
                {
                }

                protected override void ApplyUpdate(Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Update update, ref Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFields.Component data)
                {
                }
            }
        }
    }
}
