// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

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

namespace Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        internal class ReactiveComponentReplicator : IReactiveComponentReplicationHandler
        {
            public uint ComponentId => 1001;

            public EntityArchetypeQuery EventQuery => new EntityArchetypeQuery
            {
                All = new[]
                {
                    ComponentType.Create<EventSender.FirstEvent>(),
                    ComponentType.Create<EventSender.SecondEvent>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>(),
            };

            public EntityArchetypeQuery[] CommandQueries => new EntityArchetypeQuery[]
            {
                new EntityArchetypeQuery()
                {
                    All = new[]
                    {
                        ComponentType.Create<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandSenders.FirstCommand>(),
                        ComponentType.Create<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponders.FirstCommand>(),
                    },
                    Any = Array.Empty<ComponentType>(),
                    None = Array.Empty<ComponentType>(),
                },
                new EntityArchetypeQuery()
                {
                    All = new[]
                    {
                        ComponentType.Create<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandSenders.SecondCommand>(),
                        ComponentType.Create<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponders.SecondCommand>(),
                    },
                    Any = Array.Empty<ComponentType>(),
                    None = Array.Empty<ComponentType>(),
                },
            };

            public void SendEvents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, ComponentUpdateSystem componentUpdateSystem)
            {
                Profiler.BeginSample("BlittableComponent");

                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var eventFirstEventType = system.GetArchetypeChunkComponentType<EventSender.FirstEvent>(true);
                var eventSecondEventType = system.GetArchetypeChunkComponentType<EventSender.SecondEvent>(true);
                foreach (var chunk in chunkArray)
                {
                    var entityIdArray = chunk.GetNativeArray(spatialOSEntityType);
                    var eventFirstEventArray = chunk.GetNativeArray(eventFirstEventType);
                    var eventSecondEventArray = chunk.GetNativeArray(eventSecondEventType);
                    for (var i = 0; i < entityIdArray.Length; i++)
                    {
                        foreach (var e in eventFirstEventArray[i].Events)
                        {
                            componentUpdateSystem.SendEvent(new FirstEvent.Event(e), entityIdArray[i].EntityId);
                        }

                        eventFirstEventArray[i].Events.Clear();
                        foreach (var e in eventSecondEventArray[i].Events)
                        {
                            componentUpdateSystem.SendEvent(new SecondEvent.Event(e), entityIdArray[i].EntityId);
                        }

                        eventSecondEventArray[i].Events.Clear();
                    }
                }

                Profiler.EndSample();
            }

            public void SendCommands(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, CommandSystem commandSystem)
            {
                Profiler.BeginSample("BlittableComponent");
                var entityType = system.GetArchetypeChunkEntityType();
                var senderTypeFirstCommand = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandSenders.FirstCommand>(true);
                var responderTypeFirstCommand = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponders.FirstCommand>(true);
                var senderTypeSecondCommand = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandSenders.SecondCommand>(true);
                var responderTypeSecondCommand = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponders.SecondCommand>(true);

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);
                    if (chunk.Has(senderTypeFirstCommand))
                    {
                        var senders = chunk.GetNativeArray(senderTypeFirstCommand);
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

                        var responders = chunk.GetNativeArray(responderTypeFirstCommand);
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

                    if (chunk.Has(senderTypeSecondCommand))
                    {
                        var senders = chunk.GetNativeArray(senderTypeSecondCommand);
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

                        var responders = chunk.GetNativeArray(responderTypeSecondCommand);
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
            public override EntityArchetypeQuery CleanupArchetypeQuery => new EntityArchetypeQuery
            {
                All = Array.Empty<ComponentType>(),
                Any = new ComponentType[]
                {
                    ComponentType.Create<ComponentAdded<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(),
                    ComponentType.Create<ComponentRemoved<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(),
                    ComponentType.Create<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReceivedUpdates>(),
                    ComponentType.Create<AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(),
                    ComponentType.Create<ReceivedEvents.FirstEvent>(),
                    ComponentType.Create<ReceivedEvents.SecondEvent>(),
                    ComponentType.Create<CommandRequests.FirstCommand>(),
                    ComponentType.Create<CommandResponses.FirstCommand>(),
                    ComponentType.Create<CommandRequests.SecondCommand>(),
                    ComponentType.Create<CommandResponses.SecondCommand>(),
                },
                None = Array.Empty<ComponentType>(),
            };

            public override void CleanComponents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>();
                var firstEventEventType = system.GetArchetypeChunkComponentType<ReceivedEvents.FirstEvent>();
                var secondEventEventType = system.GetArchetypeChunkComponentType<ReceivedEvents.SecondEvent>();

                var firstCommandRequestType = system.GetArchetypeChunkComponentType<CommandRequests.FirstCommand>();
                var firstCommandResponseType = system.GetArchetypeChunkComponentType<CommandResponses.FirstCommand>();

                var secondCommandRequestType = system.GetArchetypeChunkComponentType<CommandRequests.SecondCommand>();
                var secondCommandResponseType = system.GetArchetypeChunkComponentType<CommandResponses.SecondCommand>();

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);

                    // Updates
                    if (chunk.Has(receivedUpdateType))
                    {
                        var updateArray = chunk.GetNativeArray(receivedUpdateType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
                        }
                    }

                    // FirstEvent Event
                    if (chunk.Has(firstEventEventType))
                    {
                        var firstEventEventArray = chunk.GetNativeArray(firstEventEventType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ReceivedEvents.FirstEvent>(entities[i]);
                            ReferenceTypeProviders.FirstEventProvider.Free(firstEventEventArray[i].handle);
                        }
                    }

                    // SecondEvent Event
                    if (chunk.Has(secondEventEventType))
                    {
                        var secondEventEventArray = chunk.GetNativeArray(secondEventEventType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ReceivedEvents.SecondEvent>(entities[i]);
                            ReferenceTypeProviders.SecondEventProvider.Free(secondEventEventArray[i].handle);
                        }
                    }

                    // FirstCommand Command
                    if (chunk.Has(firstCommandRequestType))
                    {
                        var firstCommandRequestArray = chunk.GetNativeArray(firstCommandRequestType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandRequests.FirstCommand>(entities[i]);
                            ReferenceTypeProviders.FirstCommandRequestsProvider.Free(firstCommandRequestArray[i].CommandListHandle);
                        }
                    }

                    if (chunk.Has(firstCommandResponseType))
                    {
                        var firstCommandResponseArray = chunk.GetNativeArray(firstCommandResponseType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandResponses.FirstCommand>(entities[i]);
                            ReferenceTypeProviders.FirstCommandResponsesProvider.Free(firstCommandResponseArray[i].CommandListHandle);
                        }
                    }
                    // SecondCommand Command
                    if (chunk.Has(secondCommandRequestType))
                    {
                        var secondCommandRequestArray = chunk.GetNativeArray(secondCommandRequestType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandRequests.SecondCommand>(entities[i]);
                            ReferenceTypeProviders.SecondCommandRequestsProvider.Free(secondCommandRequestArray[i].CommandListHandle);
                        }
                    }

                    if (chunk.Has(secondCommandResponseType))
                    {
                        var secondCommandResponseArray = chunk.GetNativeArray(secondCommandResponseType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandResponses.SecondCommand>(entities[i]);
                            ReferenceTypeProviders.SecondCommandResponsesProvider.Free(secondCommandResponseArray[i].CommandListHandle);
                        }
                    }
                }
            }
        }

        internal class AcknowledgeAuthorityLossHandler : AbstractAcknowledgeAuthorityLossHandler
       {
            public override EntityArchetypeQuery Query => new EntityArchetypeQuery
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<AuthorityLossImminent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>()
            };

            public override void AcknowledgeAuthorityLoss(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                ComponentUpdateSystem updateSystem)
            {
                var authorityLossType = system.GetArchetypeChunkComponentType<AuthorityLossImminent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>();
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
                                1001);
                        }
                    }
                }
            }
        }
    }
}
