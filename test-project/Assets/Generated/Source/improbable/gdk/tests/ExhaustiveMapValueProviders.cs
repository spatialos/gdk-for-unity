// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Tests
{
    public partial class ExhaustiveMapValue
    {
        internal static class ReferenceTypeProviders
        {
#if !DISABLE_REACTIVE_COMPONENTS
            public static class UpdatesProvider 
            {
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.ExhaustiveMapValue.Update>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.ExhaustiveMapValue.Update>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.ExhaustiveMapValue.Update>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.ExhaustiveMapValue.Update> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.ExhaustiveMapValue.Update> value)
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
            
#endif

            public static class Field1Provider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,bool>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,bool>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,bool>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,bool> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field1Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,bool> value)
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
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,float>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,float>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,float>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,float> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field2Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,float> value)
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
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,byte[]>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,byte[]>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,byte[]>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,byte[]> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field3Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,byte[]> value)
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
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,int>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,int>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,int>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,int> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field4Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,int> value)
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
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,long>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,long>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,long>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,long> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field5Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,long> value)
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
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,double>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,double>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,double>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,double> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field6Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,double> value)
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
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,uint>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,uint>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,uint>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,uint> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field8Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,uint> value)
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
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,ulong>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,ulong>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,ulong>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,ulong> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field9Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,ulong> value)
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
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,int>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,int>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,int>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,int> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field10Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,int> value)
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
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,long>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,long>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,long>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,long> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field11Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,long> value)
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
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,uint>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,uint>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,uint>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,uint> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field12Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,uint> value)
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
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,ulong>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,ulong>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,ulong>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,ulong> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field13Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,ulong> value)
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
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,int>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,int>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,int>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,int> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field14Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,int> value)
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
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,long>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,long>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,long>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,long> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field15Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,long> value)
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
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntityId>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntityId>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntityId>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntityId> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field16Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Core.EntityId> value)
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
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeType>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeType>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeType>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeType> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field17Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeType> value)
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
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeEnum>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeEnum>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeEnum>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeEnum> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"Field18Provider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeEnum> value)
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
