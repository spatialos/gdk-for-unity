// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        internal static class ReferenceTypeProviders
        {
            public static class UpdatesProvider 
            {
                private static readonly Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update>> Storage = new Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }
            
                    Storage[handle] = value;
                }
            
                public static void Free(uint handle)
                {
                    Storage.Remove(handle);
                    WorldMapping.Remove(handle);
                }
            
                public static void CleanDataInWorld(global::Unity.Entities.World world)
                {
                    var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();
            
                    foreach (var handle in handles)
                    {
                        Free(handle);
                    }
                }
            
                private static uint GetNextHandle() 
                {
                    nextHandle++;
                    
                    while (Storage.ContainsKey(nextHandle))
                    {
                        nextHandle++;
                    }
            
                    return nextHandle;
                }
            }
            

            public static class FirstEventProvider 
            {
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"FirstEventProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"FirstEventProvider does not contain handle {handle}");
                    }
            
                    Storage[handle] = value;
                }
            
                public static void Free(uint handle)
                {
                    Storage.Remove(handle);
                    WorldMapping.Remove(handle);
                }
            
                public static void CleanDataInWorld(global::Unity.Entities.World world)
                {
                    var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();
            
                    foreach (var handle in handles)
                    {
                        Free(handle);
                    }
                }
            
                private static uint GetNextHandle() 
                {
                    nextHandle++;
                    
                    while (Storage.ContainsKey(nextHandle))
                    {
                        nextHandle++;
                    }
            
                    return nextHandle;
                }
            }
            
            public static class SecondEventProvider 
            {
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"SecondEventProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"SecondEventProvider does not contain handle {handle}");
                    }
            
                    Storage[handle] = value;
                }
            
                public static void Free(uint handle)
                {
                    Storage.Remove(handle);
                    WorldMapping.Remove(handle);
                }
            
                public static void CleanDataInWorld(global::Unity.Entities.World world)
                {
                    var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();
            
                    foreach (var handle in handles)
                    {
                        Free(handle);
                    }
                }
            
                private static uint GetNextHandle() 
                {
                    nextHandle++;
                    
                    while (Storage.ContainsKey(nextHandle))
                    {
                        nextHandle++;
                    }
            
                    return nextHandle;
                }
            }
            
            public static class FirstCommandSenderProvider 
            {
                private static readonly Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Request>> Storage = new Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Request>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Request>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Request> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"FirstCommandSenderProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Request> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"FirstCommandSenderProvider does not contain handle {handle}");
                    }
            
                    Storage[handle] = value;
                }
            
                public static void Free(uint handle)
                {
                    Storage.Remove(handle);
                    WorldMapping.Remove(handle);
                }
            
                public static void CleanDataInWorld(global::Unity.Entities.World world)
                {
                    var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();
            
                    foreach (var handle in handles)
                    {
                        Free(handle);
                    }
                }
            
                private static uint GetNextHandle() 
                {
                    nextHandle++;
                    
                    while (Storage.ContainsKey(nextHandle))
                    {
                        nextHandle++;
                    }
            
                    return nextHandle;
                }
            }
            
            public static class FirstCommandRequestsProvider 
            {
                private static readonly Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedRequest>> Storage = new Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedRequest>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedRequest>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedRequest> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"FirstCommandRequestsProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedRequest> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"FirstCommandRequestsProvider does not contain handle {handle}");
                    }
            
                    Storage[handle] = value;
                }
            
                public static void Free(uint handle)
                {
                    Storage.Remove(handle);
                    WorldMapping.Remove(handle);
                }
            
                public static void CleanDataInWorld(global::Unity.Entities.World world)
                {
                    var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();
            
                    foreach (var handle in handles)
                    {
                        Free(handle);
                    }
                }
            
                private static uint GetNextHandle() 
                {
                    nextHandle++;
                    
                    while (Storage.ContainsKey(nextHandle))
                    {
                        nextHandle++;
                    }
            
                    return nextHandle;
                }
            }
            
            public static class FirstCommandResponderProvider 
            {
                private static readonly Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Response>> Storage = new Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Response>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Response>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Response> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"FirstCommandResponderProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Response> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"FirstCommandResponderProvider does not contain handle {handle}");
                    }
            
                    Storage[handle] = value;
                }
            
                public static void Free(uint handle)
                {
                    Storage.Remove(handle);
                    WorldMapping.Remove(handle);
                }
            
                public static void CleanDataInWorld(global::Unity.Entities.World world)
                {
                    var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();
            
                    foreach (var handle in handles)
                    {
                        Free(handle);
                    }
                }
            
                private static uint GetNextHandle() 
                {
                    nextHandle++;
                    
                    while (Storage.ContainsKey(nextHandle))
                    {
                        nextHandle++;
                    }
            
                    return nextHandle;
                }
            }
            
            public static class FirstCommandResponsesProvider 
            {
                private static readonly Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedResponse>> Storage = new Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedResponse>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedResponse>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedResponse> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"FirstCommandResponsesProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedResponse> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"FirstCommandResponsesProvider does not contain handle {handle}");
                    }
            
                    Storage[handle] = value;
                }
            
                public static void Free(uint handle)
                {
                    Storage.Remove(handle);
                    WorldMapping.Remove(handle);
                }
            
                public static void CleanDataInWorld(global::Unity.Entities.World world)
                {
                    var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();
            
                    foreach (var handle in handles)
                    {
                        Free(handle);
                    }
                }
            
                private static uint GetNextHandle() 
                {
                    nextHandle++;
                    
                    while (Storage.ContainsKey(nextHandle))
                    {
                        nextHandle++;
                    }
            
                    return nextHandle;
                }
            }
            

            public static class SecondCommandSenderProvider 
            {
                private static readonly Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Request>> Storage = new Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Request>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Request>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Request> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"SecondCommandSenderProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Request> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"SecondCommandSenderProvider does not contain handle {handle}");
                    }
            
                    Storage[handle] = value;
                }
            
                public static void Free(uint handle)
                {
                    Storage.Remove(handle);
                    WorldMapping.Remove(handle);
                }
            
                public static void CleanDataInWorld(global::Unity.Entities.World world)
                {
                    var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();
            
                    foreach (var handle in handles)
                    {
                        Free(handle);
                    }
                }
            
                private static uint GetNextHandle() 
                {
                    nextHandle++;
                    
                    while (Storage.ContainsKey(nextHandle))
                    {
                        nextHandle++;
                    }
            
                    return nextHandle;
                }
            }
            
            public static class SecondCommandRequestsProvider 
            {
                private static readonly Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedRequest>> Storage = new Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedRequest>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedRequest>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedRequest> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"SecondCommandRequestsProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedRequest> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"SecondCommandRequestsProvider does not contain handle {handle}");
                    }
            
                    Storage[handle] = value;
                }
            
                public static void Free(uint handle)
                {
                    Storage.Remove(handle);
                    WorldMapping.Remove(handle);
                }
            
                public static void CleanDataInWorld(global::Unity.Entities.World world)
                {
                    var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();
            
                    foreach (var handle in handles)
                    {
                        Free(handle);
                    }
                }
            
                private static uint GetNextHandle() 
                {
                    nextHandle++;
                    
                    while (Storage.ContainsKey(nextHandle))
                    {
                        nextHandle++;
                    }
            
                    return nextHandle;
                }
            }
            
            public static class SecondCommandResponderProvider 
            {
                private static readonly Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Response>> Storage = new Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Response>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Response>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Response> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"SecondCommandResponderProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Response> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"SecondCommandResponderProvider does not contain handle {handle}");
                    }
            
                    Storage[handle] = value;
                }
            
                public static void Free(uint handle)
                {
                    Storage.Remove(handle);
                    WorldMapping.Remove(handle);
                }
            
                public static void CleanDataInWorld(global::Unity.Entities.World world)
                {
                    var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();
            
                    foreach (var handle in handles)
                    {
                        Free(handle);
                    }
                }
            
                private static uint GetNextHandle() 
                {
                    nextHandle++;
                    
                    while (Storage.ContainsKey(nextHandle))
                    {
                        nextHandle++;
                    }
            
                    return nextHandle;
                }
            }
            
            public static class SecondCommandResponsesProvider 
            {
                private static readonly Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedResponse>> Storage = new Dictionary<uint, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedResponse>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedResponse>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedResponse> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"SecondCommandResponsesProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedResponse> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"SecondCommandResponsesProvider does not contain handle {handle}");
                    }
            
                    Storage[handle] = value;
                }
            
                public static void Free(uint handle)
                {
                    Storage.Remove(handle);
                    WorldMapping.Remove(handle);
                }
            
                public static void CleanDataInWorld(global::Unity.Entities.World world)
                {
                    var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();
            
                    foreach (var handle in handles)
                    {
                        Free(handle);
                    }
                }
            
                private static uint GetNextHandle() 
                {
                    nextHandle++;
                    
                    while (Storage.ContainsKey(nextHandle))
                    {
                        nextHandle++;
                    }
            
                    return nextHandle;
                }
            }
            

        }
    }
}
