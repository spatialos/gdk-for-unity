
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Entity = Unity.Entities.Entity;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        public partial class Requirables
        {
            [InjectableId(InjectableType.ReaderWriter, 1001)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 1001)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component, Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update>
            {
                event Action<BlittableBool> BoolFieldUpdated;
                event Action<int> IntFieldUpdated;
                event Action<long> LongFieldUpdated;
                event Action<float> FloatFieldUpdated;
                event Action<double> DoubleFieldUpdated;
                event Action<global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload> OnFirstEvent;
                event Action<global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload> OnSecondEvent;
            }

            [InjectableId(InjectableType.ReaderWriter, 1001)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : IWriter<Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component, Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update>
            {
                void SendFirstEvent( global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload payload);
                void SendSecondEvent( global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload payload);
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component, Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update>, Reader, Writer
            {
                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                private readonly List<Action<BlittableBool>> boolFieldDelegates = new List<Action<BlittableBool>>();

                public event Action<BlittableBool> BoolFieldUpdated
                {
                    add => boolFieldDelegates.Add(value);
                    remove => boolFieldDelegates.Remove(value);
                }

                private readonly List<Action<int>> intFieldDelegates = new List<Action<int>>();

                public event Action<int> IntFieldUpdated
                {
                    add => intFieldDelegates.Add(value);
                    remove => intFieldDelegates.Remove(value);
                }

                private readonly List<Action<long>> longFieldDelegates = new List<Action<long>>();

                public event Action<long> LongFieldUpdated
                {
                    add => longFieldDelegates.Add(value);
                    remove => longFieldDelegates.Remove(value);
                }

                private readonly List<Action<float>> floatFieldDelegates = new List<Action<float>>();

                public event Action<float> FloatFieldUpdated
                {
                    add => floatFieldDelegates.Add(value);
                    remove => floatFieldDelegates.Remove(value);
                }

                private readonly List<Action<double>> doubleFieldDelegates = new List<Action<double>>();

                public event Action<double> DoubleFieldUpdated
                {
                    add => doubleFieldDelegates.Add(value);
                    remove => doubleFieldDelegates.Remove(value);
                }

                protected override void TriggerFieldCallbacks(Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update update)
                {
                    DispatchWithErrorHandling(update.BoolField, boolFieldDelegates);
                    DispatchWithErrorHandling(update.IntField, intFieldDelegates);
                    DispatchWithErrorHandling(update.LongField, longFieldDelegates);
                    DispatchWithErrorHandling(update.FloatField, floatFieldDelegates);
                    DispatchWithErrorHandling(update.DoubleField, doubleFieldDelegates);
                }
                protected override void ApplyUpdate(Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update update, ref Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component data)
                {
                    if (update.BoolField.HasValue)
                    {
                        data.BoolField = update.BoolField.Value;
                    }
                    if (update.IntField.HasValue)
                    {
                        data.IntField = update.IntField.Value;
                    }
                    if (update.LongField.HasValue)
                    {
                        data.LongField = update.LongField.Value;
                    }
                    if (update.FloatField.HasValue)
                    {
                        data.FloatField = update.FloatField.Value;
                    }
                    if (update.DoubleField.HasValue)
                    {
                        data.DoubleField = update.DoubleField.Value;
                    }
                }

                private readonly List<Action<global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload>> FirstEventDelegates = new List<Action<global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload>>();

                public event Action<global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload> OnFirstEvent
                {
                    add => FirstEventDelegates.Add(value);
                    remove => FirstEventDelegates.Remove(value);
                }

                public void OnFirstEventEvent(global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload payload)
                {
                    DispatchEventWithErrorHandling(payload, FirstEventDelegates);
                }

                public void SendFirstEvent(global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload payload)
                {
                    var sender = EntityManager.GetComponentData<EventSender.FirstEvent>(Entity);
                    sender.Events.Add(payload);
                }

                private readonly List<Action<global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload>> SecondEventDelegates = new List<Action<global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload>>();

                public event Action<global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload> OnSecondEvent
                {
                    add => SecondEventDelegates.Add(value);
                    remove => SecondEventDelegates.Remove(value);
                }

                public void OnSecondEventEvent(global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload payload)
                {
                    DispatchEventWithErrorHandling(payload, SecondEventDelegates);
                }

                public void SendSecondEvent(global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload payload)
                {
                    var sender = EntityManager.GetComponentData<EventSender.SecondEvent>(Entity);
                    sender.Events.Add(payload);
                }
            }
        }
    }
}
