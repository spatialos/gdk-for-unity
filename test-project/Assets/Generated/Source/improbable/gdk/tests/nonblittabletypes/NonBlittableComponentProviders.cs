// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        internal static class ReferenceTypeProviders
        {
            public static class UpdatesProvider 
            {
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update> value)
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
            

            public static class StringFieldProvider 
            {
                private static readonly Dictionary<uint, string> Storage = new Dictionary<uint, string>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(string));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static string Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"StringFieldProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, string value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"StringFieldProvider does not contain handle {handle}");
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
            

            public static class OptionalFieldProvider 
            {
                private static readonly Dictionary<uint, int?> Storage = new Dictionary<uint, int?>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(int?));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static int? Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"OptionalFieldProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, int? value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"OptionalFieldProvider does not contain handle {handle}");
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
            

            public static class ListFieldProvider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.List<int>> Storage = new Dictionary<uint, global::System.Collections.Generic.List<int>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.List<int>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.List<int> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"ListFieldProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.List<int> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"ListFieldProvider does not contain handle {handle}");
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
            

            public static class MapFieldProvider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<int,string>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<int,string>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<int,string>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<int,string> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"MapFieldProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<int,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"MapFieldProvider does not contain handle {handle}");
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
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"FirstEventProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload> value)
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
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"SecondEventProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload> value)
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
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Request>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Request>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Request>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Request> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"FirstCommandSenderProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Request> value)
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
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedRequest>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedRequest>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedRequest>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedRequest> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"FirstCommandRequestsProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedRequest> value)
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
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Response>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Response>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Response>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Response> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"FirstCommandResponderProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Response> value)
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
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedResponse>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedResponse>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedResponse>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedResponse> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"FirstCommandResponsesProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedResponse> value)
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
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Request>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Request>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Request>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Request> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"SecondCommandSenderProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Request> value)
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
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedRequest>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedRequest>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedRequest>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedRequest> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"SecondCommandRequestsProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedRequest> value)
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
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Response>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Response>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Response>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Response> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"SecondCommandResponderProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Response> value)
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
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedResponse>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedResponse>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedResponse>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedResponse> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"SecondCommandResponsesProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedResponse> value)
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
