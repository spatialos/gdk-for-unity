
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        public partial class Requirable
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
            public interface Reader : IReader<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component, Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update>
            {
                event Action<BlittableBool> BoolFieldUpdated;
                event Action<int> IntFieldUpdated;
                event Action<long> LongFieldUpdated;
                event Action<float> FloatFieldUpdated;
                event Action<double> DoubleFieldUpdated;
                event Action<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload> OnFirstEvent;
                event Action<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload> OnSecondEvent;
            }

            [InjectableId(InjectableType.ReaderWriter, 1001)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : Reader, IWriter<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component, Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update>
            {
                void SendFirstEvent( global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload payload);
                void SendSecondEvent( global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload payload);
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component, Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update>, Reader, Writer
            {
                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                private readonly List<Action<BlittableBool>> boolFieldDelegates = new List<Action<BlittableBool>>();

                public event Action<BlittableBool> BoolFieldUpdated
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        boolFieldDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        boolFieldDelegates.Remove(value);
                    }
                }

                private readonly List<Action<int>> intFieldDelegates = new List<Action<int>>();

                public event Action<int> IntFieldUpdated
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        intFieldDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        intFieldDelegates.Remove(value);
                    }
                }

                private readonly List<Action<long>> longFieldDelegates = new List<Action<long>>();

                public event Action<long> LongFieldUpdated
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        longFieldDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        longFieldDelegates.Remove(value);
                    }
                }

                private readonly List<Action<float>> floatFieldDelegates = new List<Action<float>>();

                public event Action<float> FloatFieldUpdated
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        floatFieldDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        floatFieldDelegates.Remove(value);
                    }
                }

                private readonly List<Action<double>> doubleFieldDelegates = new List<Action<double>>();

                public event Action<double> DoubleFieldUpdated
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        doubleFieldDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        doubleFieldDelegates.Remove(value);
                    }
                }

                protected override void TriggerFieldCallbacks(Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update update)
                {
                    DispatchWithErrorHandling(update.BoolField, boolFieldDelegates);
                    DispatchWithErrorHandling(update.IntField, intFieldDelegates);
                    DispatchWithErrorHandling(update.LongField, longFieldDelegates);
                    DispatchWithErrorHandling(update.FloatField, floatFieldDelegates);
                    DispatchWithErrorHandling(update.DoubleField, doubleFieldDelegates);
                }

                protected override void ApplyUpdate(Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update update, ref Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component data)
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

                private readonly List<Action<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload>> FirstEventDelegates = new List<Action<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload>>();

                public event Action<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload> OnFirstEvent
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        FirstEventDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        FirstEventDelegates.Remove(value);
                    }
                }

                public void OnFirstEventEvent(global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload payload)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(payload, FirstEventDelegates, logDispatcher);
                }

                public void SendFirstEvent(global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload payload)
                {
                    if (!VerifyNotDisposed())
                    {
                        return;
                    }

                    var sender = EntityManager.GetComponentData<EventSender.FirstEvent>(Entity);
                    sender.Events.Add(payload);
                }

                private readonly List<Action<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload>> SecondEventDelegates = new List<Action<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload>>();

                public event Action<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload> OnSecondEvent
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        SecondEventDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        SecondEventDelegates.Remove(value);
                    }
                }

                public void OnSecondEventEvent(global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload payload)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(payload, SecondEventDelegates, logDispatcher);
                }

                public void SendSecondEvent(global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload payload)
                {
                    if (!VerifyNotDisposed())
                    {
                        return;
                    }

                    var sender = EntityManager.GetComponentData<EventSender.SecondEvent>(Entity);
                    sender.Events.Add(payload);
                }
            }
        }
    }
}
