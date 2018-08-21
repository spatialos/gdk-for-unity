
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Entity = Unity.Entities.Entity;

namespace Generated.Improbable.Gdk.Tests
{
    public partial class NestedComponent
    {
        public partial class Requirables
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
            public interface Reader : IReader<SpatialOSNestedComponent, SpatialOSNestedComponent.Update>
            {
                event Action<global::Generated.Improbable.Gdk.Tests.TypeName> NestedTypeUpdated;
            }

            [InjectableId(InjectableType.ReaderWriter, 20152)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : IWriter<SpatialOSNestedComponent, SpatialOSNestedComponent.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<SpatialOSNestedComponent, SpatialOSNestedComponent.Update>, Reader, Writer
            {
                public ReaderWriterImpl(Entity entity,EntityManager entityManager,ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                private readonly List<Action<global::Generated.Improbable.Gdk.Tests.TypeName>> nestedTypeDelegates = new List<Action<global::Generated.Improbable.Gdk.Tests.TypeName>>();

                public event Action<global::Generated.Improbable.Gdk.Tests.TypeName> NestedTypeUpdated
                {
                    add => nestedTypeDelegates.Add(value);
                    remove => nestedTypeDelegates.Remove(value);
                }

                protected override void TriggerFieldCallbacks(SpatialOSNestedComponent.Update update)
                {
                    DispatchWithErrorHandling(update.NestedType, nestedTypeDelegates);
                }
                protected override void ApplyUpdate(SpatialOSNestedComponent.Update update, ref SpatialOSNestedComponent data)
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
