
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
    public partial class Position
    {
        public partial class Requirables
        {
            [InjectableId(InjectableType.ReaderWriter, 54)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 54)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Generated.Improbable.Position.Component, Generated.Improbable.Position.Update>
            {
                event Action<global::Generated.Improbable.Coordinates> CoordsUpdated;
            }

            [InjectableId(InjectableType.ReaderWriter, 54)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : IWriter<Generated.Improbable.Position.Component, Generated.Improbable.Position.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Generated.Improbable.Position.Component, Generated.Improbable.Position.Update>, Reader, Writer
            {
                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                private readonly List<Action<global::Generated.Improbable.Coordinates>> coordsDelegates = new List<Action<global::Generated.Improbable.Coordinates>>();

                public event Action<global::Generated.Improbable.Coordinates> CoordsUpdated
                {
                    add => coordsDelegates.Add(value);
                    remove => coordsDelegates.Remove(value);
                }

                protected override void TriggerFieldCallbacks(Generated.Improbable.Position.Update update)
                {
                    DispatchWithErrorHandling(update.Coords, coordsDelegates);
                }
                protected override void ApplyUpdate(Generated.Improbable.Position.Update update, ref Generated.Improbable.Position.Component data)
                {
                    if (update.Coords.HasValue)
                    {
                        data.Coords = update.Coords.Value;
                    }
                }
            }
        }
    }
}
