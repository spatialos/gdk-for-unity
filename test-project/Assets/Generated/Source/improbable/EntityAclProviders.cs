// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable
{
    public partial class EntityAcl
    {
        internal static class ReferenceTypeProviders
        {
            public static class UpdatesProvider 
            {
                private static readonly Dictionary<uint, List<Generated.Improbable.EntityAcl.Update>> Storage = new Dictionary<uint, List<Generated.Improbable.EntityAcl.Update>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<Generated.Improbable.EntityAcl.Update>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<Generated.Improbable.EntityAcl.Update> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<Generated.Improbable.EntityAcl.Update> value)
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
            

            public static class ReadAclProvider 
            {
                private static readonly Dictionary<uint, global::Generated.Improbable.WorkerRequirementSet> Storage = new Dictionary<uint, global::Generated.Improbable.WorkerRequirementSet>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::Generated.Improbable.WorkerRequirementSet));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::Generated.Improbable.WorkerRequirementSet Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"ReadAclProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::Generated.Improbable.WorkerRequirementSet value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"ReadAclProvider does not contain handle {handle}");
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
            

            public static class ComponentWriteAclProvider 
            {
                private static readonly Dictionary<uint, global::System.Collections.Generic.Dictionary<uint,global::Generated.Improbable.WorkerRequirementSet>> Storage = new Dictionary<uint, global::System.Collections.Generic.Dictionary<uint,global::Generated.Improbable.WorkerRequirementSet>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(global::System.Collections.Generic.Dictionary<uint,global::Generated.Improbable.WorkerRequirementSet>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static global::System.Collections.Generic.Dictionary<uint,global::Generated.Improbable.WorkerRequirementSet> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"ComponentWriteAclProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, global::System.Collections.Generic.Dictionary<uint,global::Generated.Improbable.WorkerRequirementSet> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"ComponentWriteAclProvider does not contain handle {handle}");
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
