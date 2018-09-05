
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Entity = Unity.Entities.Entity;

namespace Generated.Improbable
{
    public partial class Persistence
    {
        public partial class Requirables
        {
            [InjectableId(InjectableType.ReaderWriter, 55)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 55)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Generated.Improbable.Persistence.Component, Generated.Improbable.Persistence.Update>
            {
            }

            [InjectableId(InjectableType.ReaderWriter, 55)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : IWriter<Generated.Improbable.Persistence.Component, Generated.Improbable.Persistence.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Generated.Improbable.Persistence.Component, Generated.Improbable.Persistence.Update>, Reader, Writer
            {
                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                protected override void TriggerFieldCallbacks(Generated.Improbable.Persistence.Update update)
                {
                }

                protected override void ApplyUpdate(Generated.Improbable.Persistence.Update update, ref Generated.Improbable.Persistence.Component data)
                {
                }
            }
        }
    }
}
