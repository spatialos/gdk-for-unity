
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
        }

        [WriterInterface]
        [ComponentId(1002)]
        public interface Writer
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

            public void OnFirstEventEvent(FirstEventEvent payload)
            {
                throw new System.NotImplementedException();
            }

            public void OnSecondEventEvent(SecondEventEvent payload)
            {
                throw new System.NotImplementedException();
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
