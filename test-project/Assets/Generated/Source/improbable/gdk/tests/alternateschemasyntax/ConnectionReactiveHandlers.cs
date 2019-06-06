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

namespace Improbable.Gdk.Tests.AlternateSchemaSyntax
{
    public partial class Connection
    {
        internal class ReactiveComponentReplicator : IReactiveComponentReplicationHandler
        {
            public uint ComponentId => 1105;

            public EntityQueryDesc EventQuery => new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<EventSender.MyEvent>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
            };

            public EntityQueryDesc[] CommandQueries => null;

            public void SendEvents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, ComponentUpdateSystem componentUpdateSystem)
            {
                Profiler.BeginSample("Connection");

                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var eventMyEventType = system.GetArchetypeChunkComponentType<EventSender.MyEvent>(true);
                foreach (var chunk in chunkArray)
                {
                    var entityIdArray = chunk.GetNativeArray(spatialOSEntityType);
                    var eventMyEventArray = chunk.GetNativeArray(eventMyEventType);
                    for (var i = 0; i < entityIdArray.Length; i++)
                    {
                        foreach (var e in eventMyEventArray[i].Events)
                        {
                            componentUpdateSystem.SendEvent(new MyEvent.Event(e), entityIdArray[i].EntityId);
                        }

                        eventMyEventArray[i].Events.Clear();
                    }
                }

                Profiler.EndSample();
            }

            public void SendCommands(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, CommandSystem commandSystem)
            {
            }
        }

        internal class ComponentCleanup : ComponentCleanupHandler
        {
            public override EntityQueryDesc CleanupArchetypeQuery => new EntityQueryDesc
            {
                Any = new ComponentType[]
                {
                    ComponentType.ReadWrite<ComponentAdded<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(),
                    ComponentType.ReadWrite<ComponentRemoved<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(),
                    ComponentType.ReadWrite<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates>(),
                    ComponentType.ReadWrite<AuthorityChanges<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(),
                    ComponentType.ReadWrite<ReceivedEvents.MyEvent>(),
                },
            };

            public override void CleanComponents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>();
                var myEventEventType = system.GetArchetypeChunkComponentType<ReceivedEvents.MyEvent>();

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);

                    // Updates
                    if (chunk.Has(receivedUpdateType))
                    {
                        var updateArray = chunk.GetNativeArray(receivedUpdateType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
                        }
                    }

                    // MyEvent Event
                    if (chunk.Has(myEventEventType))
                    {
                        var myEventEventArray = chunk.GetNativeArray(myEventEventType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ReceivedEvents.MyEvent>(entities[i]);
                            ReferenceTypeProviders.MyEventProvider.Free(myEventEventArray[i].handle);
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
                    ComponentType.ReadOnly<AuthorityLossImminent<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
            };

            public override void AcknowledgeAuthorityLoss(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
                ComponentUpdateSystem updateSystem)
            {
                var authorityLossType = system.GetArchetypeChunkComponentType<AuthorityLossImminent<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>();
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
                                1105);
                        }
                    }
                }
            }
        }
    }
}
#endif
