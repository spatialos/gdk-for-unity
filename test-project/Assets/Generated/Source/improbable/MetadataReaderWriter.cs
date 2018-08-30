
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
    public partial class Metadata
    {
        public partial class Requirables
        {
            [InjectableId(InjectableType.ReaderWriter, 53)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 53)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Generated.Improbable.Metadata.Component, Generated.Improbable.Metadata.Update>
            {
                event Action<string> EntityTypeUpdated;
            }

            [InjectableId(InjectableType.ReaderWriter, 53)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : IWriter<Generated.Improbable.Metadata.Component, Generated.Improbable.Metadata.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Generated.Improbable.Metadata.Component, Generated.Improbable.Metadata.Update>, Reader, Writer
            {
                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                private readonly List<Action<string>> entityTypeDelegates = new List<Action<string>>();

                public event Action<string> EntityTypeUpdated
                {
                    add => entityTypeDelegates.Add(value);
                    remove => entityTypeDelegates.Remove(value);
                }

                protected override void TriggerFieldCallbacks(Generated.Improbable.Metadata.Update update)
                {
                    DispatchWithErrorHandling(update.EntityType, entityTypeDelegates);
                }
                protected override void ApplyUpdate(Generated.Improbable.Metadata.Update update, ref Generated.Improbable.Metadata.Component data)
                {
                    if (update.EntityType.HasValue)
                    {
                        data.EntityType = update.EntityType.Value;
                    }
                }
            }
        }
    }
}
