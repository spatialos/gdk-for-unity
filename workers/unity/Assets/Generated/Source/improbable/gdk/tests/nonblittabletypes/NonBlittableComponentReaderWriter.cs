
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Gdk.Core.MonoBehaviours;
using Entity = Unity.Entities.Entity;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        [ComponentId(1002)]
        internal class ReaderWriterCreator : IReaderWriterCreator
        {
            public IReaderWriterInternal CreateReaderWriter(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
                return new ReaderWriterImpl(entity, entityManager, logDispatcher);
            }
        }

        [ReaderInterface]
        [ComponentId(1002)]
        public interface Reader : IReader<SpatialOSNonBlittableComponent, SpatialOSNonBlittableComponent.Update>
        {
            event Action<BlittableBool> BoolFieldUpdated;
            event Action<int> IntFieldUpdated;
            event Action<long> LongFieldUpdated;
            event Action<float> FloatFieldUpdated;
            event Action<double> DoubleFieldUpdated;
            event Action<string> StringFieldUpdated;
            event Action<global::System.Nullable<int>> OptionalFieldUpdated;
            event Action<global::System.Collections.Generic.List<int>> ListFieldUpdated;
            event Action<global::System.Collections.Generic.Dictionary<int, string>> MapFieldUpdated;

            event Action<FirstEventEvent> OnFirstEvent;
            event Action<SecondEventEvent> OnSecondEvent;
        }

        [WriterInterface]
        [ComponentId(1002)]
        public interface Writer : IWriter<SpatialOSNonBlittableComponent, SpatialOSNonBlittableComponent.Update>
        {
        }

        internal class ReaderWriterImpl :
            NonBlittableReaderWriterBase<SpatialOSNonBlittableComponent, SpatialOSNonBlittableComponent.Update>, Reader, Writer
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

            private readonly List<Action<string>> stringFieldDelegates = new List<Action<string>>();

            public event Action<string> StringFieldUpdated
            {
                add => stringFieldDelegates.Add(value);
                remove => stringFieldDelegates.Remove(value);
            }

            private readonly List<Action<global::System.Nullable<int>>> optionalFieldDelegates = new List<Action<global::System.Nullable<int>>>();

            public event Action<global::System.Nullable<int>> OptionalFieldUpdated
            {
                add => optionalFieldDelegates.Add(value);
                remove => optionalFieldDelegates.Remove(value);
            }

            private readonly List<Action<global::System.Collections.Generic.List<int>>> listFieldDelegates = new List<Action<global::System.Collections.Generic.List<int>>>();

            public event Action<global::System.Collections.Generic.List<int>> ListFieldUpdated
            {
                add => listFieldDelegates.Add(value);
                remove => listFieldDelegates.Remove(value);
            }

            private readonly List<Action<global::System.Collections.Generic.Dictionary<int, string>>> mapFieldDelegates = new List<Action<global::System.Collections.Generic.Dictionary<int, string>>>();

            public event Action<global::System.Collections.Generic.Dictionary<int, string>> MapFieldUpdated
            {
                add => mapFieldDelegates.Add(value);
                remove => mapFieldDelegates.Remove(value);
            }

            protected override void TriggerFieldCallbacks(SpatialOSNonBlittableComponent.Update update)
            {
                DispatchWithErrorHandling(update.BoolField, boolFieldDelegates);
                DispatchWithErrorHandling(update.IntField, intFieldDelegates);
                DispatchWithErrorHandling(update.LongField, longFieldDelegates);
                DispatchWithErrorHandling(update.FloatField, floatFieldDelegates);
                DispatchWithErrorHandling(update.DoubleField, doubleFieldDelegates);
                DispatchWithErrorHandling(update.StringField, stringFieldDelegates);
                DispatchWithErrorHandling(update.OptionalField, optionalFieldDelegates);
                DispatchWithErrorHandling(update.ListField, listFieldDelegates);
                DispatchWithErrorHandling(update.MapField, mapFieldDelegates);
            }
            protected override void ApplyUpdate(SpatialOSNonBlittableComponent.Update update, SpatialOSNonBlittableComponent data)
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
                if (update.StringField.HasValue)
                {
                    data.StringField = update.StringField.Value;
                }
                if (update.OptionalField.HasValue)
                {
                    data.OptionalField = update.OptionalField.Value;
                }
                if (update.ListField.HasValue)
                {
                    data.ListField = update.ListField.Value;
                }
                if (update.MapField.HasValue)
                {
                    data.MapField = update.MapField.Value;
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
