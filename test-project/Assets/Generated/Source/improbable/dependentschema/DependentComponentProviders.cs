// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;

namespace Improbable.DependentSchema
{
    public partial class DependentComponent
    {
        internal static class ReferenceTypeProviders
        {
            public static class AProvider 
            {
                private static readonly Dictionary<uint, global::Improbable.TestSchema.ExhaustiveRepeatedData> Storage = new Dictionary<uint, global::Improbable.TestSchema.ExhaustiveRepeatedData>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, global::Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::Improbable.TestSchema.ExhaustiveRepeatedData));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::Improbable.TestSchema.ExhaustiveRepeatedData Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"AProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::Improbable.TestSchema.ExhaustiveRepeatedData value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"AProvider does not contain handle {handle}");
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
            

            public static class CProvider 
            {
                private static readonly Dictionary<uint, global::Improbable.TestSchema.SomeEnum?> Storage = new Dictionary<uint, global::Improbable.TestSchema.SomeEnum?>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, global::Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::Improbable.TestSchema.SomeEnum?));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::Improbable.TestSchema.SomeEnum? Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"CProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::Improbable.TestSchema.SomeEnum? value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"CProvider does not contain handle {handle}");
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
            

            public static class DProvider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.List<global::Improbable.TestSchema.SomeType>> Storage = new Dictionary<uint, global::System.Collections.Generic.List<global::Improbable.TestSchema.SomeType>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, global::Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.List<global::Improbable.TestSchema.SomeType>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.List<global::Improbable.TestSchema.SomeType> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"DProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.List<global::Improbable.TestSchema.SomeType> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"DProvider does not contain handle {handle}");
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
            

            public static class EProvider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, global::Improbable.TestSchema.SomeType>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, global::Improbable.TestSchema.SomeType>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, global::Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, global::Improbable.TestSchema.SomeType>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, global::Improbable.TestSchema.SomeType> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"EProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, global::Improbable.TestSchema.SomeType> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"EProvider does not contain handle {handle}");
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
