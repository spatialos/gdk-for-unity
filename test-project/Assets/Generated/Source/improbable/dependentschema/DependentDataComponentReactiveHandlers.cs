// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if !DISABLE_REACTIVE_COMPONENTS
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Collections;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Core;
using Improbable.Gdk.ReactiveComponents;

namespace Improbable.DependentSchema
{
    public partial class DependentDataComponent
    {
        internal class ReactiveComponentReplicator : IReactiveComponentReplicationHandler
        {
            public uint ComponentId => 198801;

            public EntityQueryDesc EventQuery => new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<EventSender.FooEvent>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
            };

            public EntityQueryDesc[] CommandQueries => new EntityQueryDesc[]
            {
                new EntityQueryDesc()
                {
                    All = new[]
                    {
                        ComponentType.ReadWrite<global::Improbable.DependentSchema.DependentDataComponent.CommandSenders.BarCommand>(),
                        ComponentType.ReadWrite<global::Improbable.DependentSchema.DependentDataComponent.CommandResponders.BarCommand>(),
                    },
                },
            };

            public void SendEvents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, ComponentUpdateSystem componentUpdateSystem)
            {
                Profiler.BeginSample("DependentDataComponent");

                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var eventFooEventType = system.GetArchetypeChunkComponentType<EventSender.FooEvent>(true);
                foreach (var chunk in chunkArray)
                {
                    var entityIdArray = chunk.GetNativeArray(spatialOSEntityType);
                    var eventFooEventArray = chunk.GetNativeArray(eventFooEventType);
                    for (var i = 0; i < entityIdArray.Length; i++)
                    {
                        foreach (var e in eventFooEventArray[i].Events)
                        {
                            componentUpdateSystem.SendEvent(new FooEvent.Event(e), entityIdArray[i].EntityId);
                        }

                        eventFooEventArray[i].Events.Clear();
                    }
                }

                Profiler.EndSample();
            }

            public void SendCommands(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, CommandSystem commandSystem)
            {
                Profiler.BeginSample("DependentDataComponent");
                var entityType = system.GetArchetypeChunkEntityType();
                var senderTypeBarCommand = system.GetArchetypeChunkComponentType<global::Improbable.DependentSchema.DependentDataComponent.CommandSenders.BarCommand>(true);
                var responderTypeBarCommand = system.GetArchetypeChunkComponentType<global::Improbable.DependentSchema.DependentDataComponent.CommandResponders.BarCommand>(true);

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);
                    if (chunk.Has(senderTypeBarCommand))
                    {
                        var senders = chunk.GetNativeArray(senderTypeBarCommand);
                        for (var i = 0; i < senders.Length; i++)
                        {
                            var requests = senders[i].RequestsToSend;
                            if (requests.Count > 0)
                            {
                                foreach (var request in requests)
                                {
                                    commandSystem.SendCommand(request, entities[i]);
                                }

                                requests.Clear();
                            }
                        }

                        var responders = chunk.GetNativeArray(responderTypeBarCommand);
                        for (var i = 0; i < responders.Length; i++)
                        {
                            var responses = responders[i].ResponsesToSend;
                            if (responses.Count > 0)
                            {
                                foreach (var response in responses)
                                {
                                    commandSystem.SendResponse(response);
                                }

                                responses.Clear();
                            }
                        }
                    }

                }

                Profiler.EndSample();
            }
        }

        internal class ComponentCleanup : ComponentCleanupHandler
        {
            public override EntityQueryDesc CleanupArchetypeQuery => new EntityQueryDesc
            {
                Any = new ComponentType[]
                {
                    ComponentType.ReadWrite<ComponentAdded<global::Improbable.DependentSchema.DependentDataComponent.Component>>(),
                    ComponentType.ReadWrite<ComponentRemoved<global::Improbable.DependentSchema.DependentDataComponent.Component>>(),
                    ComponentType.ReadWrite<global::Improbable.DependentSchema.DependentDataComponent.ReceivedUpdates>(),
                    ComponentType.ReadWrite<AuthorityChanges<global::Improbable.DependentSchema.DependentDataComponent.Component>>(),
                    ComponentType.ReadWrite<ReceivedEvents.FooEvent>(),
                    ComponentType.ReadWrite<CommandRequests.BarCommand>(),
                    ComponentType.ReadWrite<CommandResponses.BarCommand>(),
                },
            };

            public override void CleanComponents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<global::Improbable.DependentSchema.DependentDataComponent.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<global::Improbable.DependentSchema.DependentDataComponent.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<global::Improbable.DependentSchema.DependentDataComponent.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<global::Improbable.DependentSchema.DependentDataComponent.Component>>();
                var fooEventEventType = system.GetArchetypeChunkComponentType<ReceivedEvents.FooEvent>();

                var barCommandRequestType = system.GetArchetypeChunkComponentType<CommandRequests.BarCommand>();
                var barCommandResponseType = system.GetArchetypeChunkComponentType<CommandResponses.BarCommand>();

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);

                    // Updates
                    if (chunk.Has(receivedUpdateType))
                    {
                        var updateArray = chunk.GetNativeArray(receivedUpdateType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<global::Improbable.DependentSchema.DependentDataComponent.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            global::Improbable.DependentSchema.DependentDataComponent.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<global::Improbable.DependentSchema.DependentDataComponent.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<global::Improbable.DependentSchema.DependentDataComponent.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<global::Improbable.DependentSchema.DependentDataComponent.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
                        }
                    }

                    // FooEvent Event
                    if (chunk.Has(fooEventEventType))
                    {
                        var fooEventEventArray = chunk.GetNativeArray(fooEventEventType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ReceivedEvents.FooEvent>(entities[i]);
                            ReferenceTypeProviders.FooEventProvider.Free(fooEventEventArray[i].handle);
                        }
                    }

                    // BarCommand Command
                    if (chunk.Has(barCommandRequestType))
                    {
                        var barCommandRequestArray = chunk.GetNativeArray(barCommandRequestType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandRequests.BarCommand>(entities[i]);
                            ReferenceTypeProviders.BarCommandRequestsProvider.Free(barCommandRequestArray[i].CommandListHandle);
                        }
                    }

                    if (chunk.Has(barCommandResponseType))
                    {
                        var barCommandResponseArray = chunk.GetNativeArray(barCommandResponseType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandResponses.BarCommand>(entities[i]);
                            ReferenceTypeProviders.BarCommandResponsesProvider.Free(barCommandResponseArray[i].CommandListHandle);
                        }
                    }
                }
            }
        }

        internal class AcknowledgeAuthorityLossHandler : AbstractAcknowledgeAuthorityLossHandler
       {
            public override EntityQueryDesc Query => new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<AuthorityLossImminent<global::Improbable.DependentSchema.DependentDataComponent.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
            };

            public override void AcknowledgeAuthorityLoss(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                ComponentUpdateSystem updateSystem)
            {
                var authorityLossType = system.GetArchetypeChunkComponentType<AuthorityLossImminent<global::Improbable.DependentSchema.DependentDataComponent.Component>>();
                var spatialEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>();

                foreach (var chunk in chunkArray)
                {
                    var authorityArray = chunk.GetNativeArray(authorityLossType);
                    var spatialEntityIdArray = chunk.GetNativeArray(spatialEntityType);

                    for (int i = 0; i < authorityArray.Length; ++i)
                    {
                        if (authorityArray[i].AcknowledgeAuthorityLoss)
                        {
                            updateSystem.AcknowledgeAuthorityLoss(spatialEntityIdArray[i].EntityId,
                                198801);
                        }
                    }
                }
            }
        }
    }
}
#endif
