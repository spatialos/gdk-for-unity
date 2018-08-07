
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

namespace Generated.Improbable.Gdk.Tests
{
    public partial class ExhaustiveSingular
    {
        [ComponentId(197715)]
        internal class ReaderWriterCreator : IReaderWriterCreator
        {
            public IReaderWriterInternal CreateReaderWriter(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
                return new ReaderWriterImpl(entity, entityManager, logDispatcher);
            }
        }

        [ReaderInterface]
        [ComponentId(197715)]
        public interface Reader : IReader<SpatialOSExhaustiveSingular, SpatialOSExhaustiveSingular.Update>
        {
            event Action<BlittableBool> Field1Updated;
            event Action<float> Field2Updated;
            event Action<int> Field4Updated;
            event Action<long> Field5Updated;
            event Action<double> Field6Updated;
            event Action<string> Field7Updated;
            event Action<uint> Field8Updated;
            event Action<ulong> Field9Updated;
            event Action<int> Field10Updated;
            event Action<long> Field11Updated;
            event Action<uint> Field12Updated;
            event Action<ulong> Field13Updated;
            event Action<int> Field14Updated;
            event Action<long> Field15Updated;
            event Action<long> Field16Updated;
            event Action<global::Generated.Improbable.Gdk.Tests.SomeType> Field17Updated;
        }

        [WriterInterface]
        [ComponentId(197715)]
        public interface Writer
        {
        }

        internal class ReaderWriterImpl :
            NonBlittableReaderBase<SpatialOSExhaustiveSingular, SpatialOSExhaustiveSingular.Update>, Reader, Writer
        {
            public ReaderWriterImpl(Entity entity,EntityManager entityManager,ILogDispatcher logDispatcher)
                : base(entity, entityManager, logDispatcher)
            {
            }

            private readonly List<Action<BlittableBool>> field1Delegates = new List<Action<BlittableBool>>();

            public event Action<BlittableBool> Field1Updated
            {
                add => field1Delegates.Add(value);
                remove => field1Delegates.Remove(value);
            }

            private readonly List<Action<float>> field2Delegates = new List<Action<float>>();

            public event Action<float> Field2Updated
            {
                add => field2Delegates.Add(value);
                remove => field2Delegates.Remove(value);
            }

            private readonly List<Action<int>> field4Delegates = new List<Action<int>>();

            public event Action<int> Field4Updated
            {
                add => field4Delegates.Add(value);
                remove => field4Delegates.Remove(value);
            }

            private readonly List<Action<long>> field5Delegates = new List<Action<long>>();

            public event Action<long> Field5Updated
            {
                add => field5Delegates.Add(value);
                remove => field5Delegates.Remove(value);
            }

            private readonly List<Action<double>> field6Delegates = new List<Action<double>>();

            public event Action<double> Field6Updated
            {
                add => field6Delegates.Add(value);
                remove => field6Delegates.Remove(value);
            }

            private readonly List<Action<string>> field7Delegates = new List<Action<string>>();

            public event Action<string> Field7Updated
            {
                add => field7Delegates.Add(value);
                remove => field7Delegates.Remove(value);
            }

            private readonly List<Action<uint>> field8Delegates = new List<Action<uint>>();

            public event Action<uint> Field8Updated
            {
                add => field8Delegates.Add(value);
                remove => field8Delegates.Remove(value);
            }

            private readonly List<Action<ulong>> field9Delegates = new List<Action<ulong>>();

            public event Action<ulong> Field9Updated
            {
                add => field9Delegates.Add(value);
                remove => field9Delegates.Remove(value);
            }

            private readonly List<Action<int>> field10Delegates = new List<Action<int>>();

            public event Action<int> Field10Updated
            {
                add => field10Delegates.Add(value);
                remove => field10Delegates.Remove(value);
            }

            private readonly List<Action<long>> field11Delegates = new List<Action<long>>();

            public event Action<long> Field11Updated
            {
                add => field11Delegates.Add(value);
                remove => field11Delegates.Remove(value);
            }

            private readonly List<Action<uint>> field12Delegates = new List<Action<uint>>();

            public event Action<uint> Field12Updated
            {
                add => field12Delegates.Add(value);
                remove => field12Delegates.Remove(value);
            }

            private readonly List<Action<ulong>> field13Delegates = new List<Action<ulong>>();

            public event Action<ulong> Field13Updated
            {
                add => field13Delegates.Add(value);
                remove => field13Delegates.Remove(value);
            }

            private readonly List<Action<int>> field14Delegates = new List<Action<int>>();

            public event Action<int> Field14Updated
            {
                add => field14Delegates.Add(value);
                remove => field14Delegates.Remove(value);
            }

            private readonly List<Action<long>> field15Delegates = new List<Action<long>>();

            public event Action<long> Field15Updated
            {
                add => field15Delegates.Add(value);
                remove => field15Delegates.Remove(value);
            }

            private readonly List<Action<long>> field16Delegates = new List<Action<long>>();

            public event Action<long> Field16Updated
            {
                add => field16Delegates.Add(value);
                remove => field16Delegates.Remove(value);
            }

            private readonly List<Action<global::Generated.Improbable.Gdk.Tests.SomeType>> field17Delegates = new List<Action<global::Generated.Improbable.Gdk.Tests.SomeType>>();

            public event Action<global::Generated.Improbable.Gdk.Tests.SomeType> Field17Updated
            {
                add => field17Delegates.Add(value);
                remove => field17Delegates.Remove(value);
            }

            protected override void TriggerFieldCallbacks(SpatialOSExhaustiveSingular.Update update)
            {
                DispatchWithErrorHandling(update.Field1, field1Delegates);
                DispatchWithErrorHandling(update.Field2, field2Delegates);
                DispatchWithErrorHandling(update.Field4, field4Delegates);
                DispatchWithErrorHandling(update.Field5, field5Delegates);
                DispatchWithErrorHandling(update.Field6, field6Delegates);
                DispatchWithErrorHandling(update.Field7, field7Delegates);
                DispatchWithErrorHandling(update.Field8, field8Delegates);
                DispatchWithErrorHandling(update.Field9, field9Delegates);
                DispatchWithErrorHandling(update.Field10, field10Delegates);
                DispatchWithErrorHandling(update.Field11, field11Delegates);
                DispatchWithErrorHandling(update.Field12, field12Delegates);
                DispatchWithErrorHandling(update.Field13, field13Delegates);
                DispatchWithErrorHandling(update.Field14, field14Delegates);
                DispatchWithErrorHandling(update.Field15, field15Delegates);
                DispatchWithErrorHandling(update.Field16, field16Delegates);
                DispatchWithErrorHandling(update.Field17, field17Delegates);
            }
        }
    }
}
