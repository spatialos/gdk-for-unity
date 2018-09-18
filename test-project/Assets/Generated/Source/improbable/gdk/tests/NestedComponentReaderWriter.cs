
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Entity = Unity.Entities.Entity;

namespace Generated.Improbable.Gdk.Tests
{
    public partial class NestedComponent
    {
        public partial class Requirable
        {
            [InjectableId(InjectableType.ReaderWriter, 20152)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 20152)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Generated.Improbable.Gdk.Tests.NestedComponent.Component, Generated.Improbable.Gdk.Tests.NestedComponent.Update>
            {
                event Action<global::Generated.Improbable.Gdk.Tests.TypeName> NestedTypeUpdated;
            }

            [InjectableId(InjectableType.ReaderWriter, 20152)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : Reader, IWriter<Generated.Improbable.Gdk.Tests.NestedComponent.Component, Generated.Improbable.Gdk.Tests.NestedComponent.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Generated.Improbable.Gdk.Tests.NestedComponent.Component, Generated.Improbable.Gdk.Tests.NestedComponent.Update>, Reader, Writer
            {
                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                private readonly List<Action<global::Generated.Improbable.Gdk.Tests.TypeName>> nestedTypeDelegates = new List<Action<global::Generated.Improbable.Gdk.Tests.TypeName>>();

                public event Action<global::Generated.Improbable.Gdk.Tests.TypeName> NestedTypeUpdated
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        nestedTypeDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        nestedTypeDelegates.Remove(value);
                    }
                }

                protected override void TriggerFieldCallbacks(Generated.Improbable.Gdk.Tests.NestedComponent.Update update)
                {
                    DispatchWithErrorHandling(update.NestedType, nestedTypeDelegates);
                }

                protected override void ApplyUpdate(Generated.Improbable.Gdk.Tests.NestedComponent.Update update, ref Generated.Improbable.Gdk.Tests.NestedComponent.Component data)
                {
                    if (update.NestedType.HasValue)
                    {
                        data.NestedType = update.NestedType.Value;
                    }
                }
            }
        }
    }
}
