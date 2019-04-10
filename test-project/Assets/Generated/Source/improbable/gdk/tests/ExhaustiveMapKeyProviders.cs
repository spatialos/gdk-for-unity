// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Tests
{
    public partial class ExhaustiveMapKey
    {
        internal static class ReferenceTypeProviders
        {
            public static class UpdatesProvider 
            {
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.ExhaustiveMapKey.Update>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.ExhaustiveMapKey.Update>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.ExhaustiveMapKey.Update>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.ExhaustiveMapKey.Update> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.ExhaustiveMapKey.Update> value)
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
            

            public static class Field1Provider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<BlittableBool,string>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<BlittableBool,string>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<BlittableBool,string>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<BlittableBool,string> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field1Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<BlittableBool,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field1Provider does not contain handle {handle}");
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
            

            public static class Field2Provider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<float,string>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<float,string>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<float,string>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<float,string> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field2Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<float,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field2Provider does not contain handle {handle}");
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
            

            public static class Field3Provider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<byte[],string>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<byte[],string>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<byte[],string>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<byte[],string> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field3Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<byte[],string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field3Provider does not contain handle {handle}");
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
            

            public static class Field4Provider 
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
                        throw new ArgumentException($"Field4Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<int,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field4Provider does not contain handle {handle}");
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
            

            public static class Field5Provider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<long,string>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<long,string>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<long,string>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<long,string> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field5Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<long,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field5Provider does not contain handle {handle}");
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
            

            public static class Field6Provider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<double,string>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<double,string>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<double,string>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<double,string> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field6Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<double,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field6Provider does not contain handle {handle}");
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
            

            public static class Field7Provider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,string>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,string>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,string>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,string> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field7Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field7Provider does not contain handle {handle}");
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
            

            public static class Field8Provider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<uint,string>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<uint,string>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<uint,string>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<uint,string> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field8Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<uint,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field8Provider does not contain handle {handle}");
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
            

            public static class Field9Provider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<ulong,string>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<ulong,string>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<ulong,string>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<ulong,string> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field9Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<ulong,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field9Provider does not contain handle {handle}");
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
            

            public static class Field10Provider 
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
                        throw new ArgumentException($"Field10Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<int,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field10Provider does not contain handle {handle}");
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
            

            public static class Field11Provider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<long,string>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<long,string>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<long,string>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<long,string> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field11Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<long,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field11Provider does not contain handle {handle}");
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
            

            public static class Field12Provider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<uint,string>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<uint,string>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<uint,string>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<uint,string> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field12Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<uint,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field12Provider does not contain handle {handle}");
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
            

            public static class Field13Provider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<ulong,string>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<ulong,string>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<ulong,string>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<ulong,string> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field13Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<ulong,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field13Provider does not contain handle {handle}");
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
            

            public static class Field14Provider 
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
                        throw new ArgumentException($"Field14Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<int,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field14Provider does not contain handle {handle}");
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
            

            public static class Field15Provider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<long,string>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<long,string>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<long,string>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<long,string> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field15Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<long,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field15Provider does not contain handle {handle}");
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
            

            public static class Field16Provider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field16Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field16Provider does not contain handle {handle}");
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
            

            public static class Field17Provider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field17Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field17Provider does not contain handle {handle}");
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
            

            public static class Field18Provider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field18Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"Field18Provider does not contain handle {handle}");
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
