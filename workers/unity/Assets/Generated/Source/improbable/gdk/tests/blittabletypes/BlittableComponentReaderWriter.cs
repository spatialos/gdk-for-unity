
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
        [InjectableId(InjectableType.ReaderWriter, 1001)]
        internal class ReaderWriterCreator : IInjectableCreator
        {
            public IInjectable CreateReaderWriter(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
                return new ReaderWriterImpl(entity, entityManager, logDispatcher);
            }
        }

        [InjectableId(InjectableType.ReaderWriter, 1001)]
        [InjectionCondition(InjectionCondition.RequireComponentToRead)]
        public interface Reader : IReader<SpatialOSBlittableComponent, SpatialOSBlittableComponent.Update>
        {
            event Action<BlittableBool> BoolFieldUpdated;
            event Action<int> IntFieldUpdated;
            event Action<long> LongFieldUpdated;
            event Action<float> FloatFieldUpdated;
            event Action<double> DoubleFieldUpdated;
            event Action<FirstEventEvent> OnFirstEvent;
            event Action<SecondEventEvent> OnSecondEvent;
        }

        [InjectableId(InjectableType.ReaderWriter, 1001)]
        [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
        public interface Writer : IWriter<SpatialOSBlittableComponent, SpatialOSBlittableComponent.Update>
        {
            void SendFirstEvent( global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload payload);
            void SendSecondEvent( global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload payload);
        }

        internal class ReaderWriterImpl :
            BlittableReaderWriterBase<SpatialOSBlittableComponent, SpatialOSBlittableComponent.Update>, Reader, Writer
        {
            public ReaderWriterImpl(Entity entity,EntityManager entityManager,ILogDispatcher logDispatcher)
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

            protected override void TriggerFieldCallbacks(SpatialOSBlittableComponent.Update update)
            {
                DispatchWithErrorHandling(update.BoolField, boolFieldDelegates);
                DispatchWithErrorHandling(update.IntField, intFieldDelegates);
                DispatchWithErrorHandling(update.LongField, longFieldDelegates);
                DispatchWithErrorHandling(update.FloatField, floatFieldDelegates);
                DispatchWithErrorHandling(update.DoubleField, doubleFieldDelegates);
            }
            protected override void ApplyUpdate(SpatialOSBlittableComponent.Update update, ref SpatialOSBlittableComponent data)
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

            private readonly List<Action<FirstEventEvent>> firstEventDelegates = new System.Collections.Generic.List<System.Action<FirstEventEvent>>();

            public event Action<FirstEventEvent> OnFirstEvent
            {
                add => firstEventDelegates.Add(value);
                remove => firstEventDelegates.Remove(value);
            }

            public void OnFirstEventEvent(FirstEventEvent payload)
            {
                DispatchEventWithErrorHandling(payload, firstEventDelegates);
            }

            public void SendFirstEvent( global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload payload)
            {
                var sender = EntityManager.GetComponentData<EventSender<SpatialOSBlittableComponent>>(Entity);
                sender.SendFirstEventEvent(payload);
            }

            private readonly List<Action<SecondEventEvent>> secondEventDelegates = new System.Collections.Generic.List<System.Action<SecondEventEvent>>();

            public event Action<SecondEventEvent> OnSecondEvent
            {
                add => secondEventDelegates.Add(value);
                remove => secondEventDelegates.Remove(value);
            }

            public void OnSecondEventEvent(SecondEventEvent payload)
            {
                DispatchEventWithErrorHandling(payload, secondEventDelegates);
            }

            public void SendSecondEvent( global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload payload)
            {
                var sender = EntityManager.GetComponentData<EventSender<SpatialOSBlittableComponent>>(Entity);
                sender.SendSecondEventEvent(payload);
            }

            public void OnFirstCommandCommandRequest(FirstCommand.Request request)
            {
                throw new System.NotImplementedException();
            }

            public void OnSecondCommandCommandRequest(SecondCommand.Request request)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
