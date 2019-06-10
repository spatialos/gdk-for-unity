// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Tests.AlternateSchemaSyntax
{
    public partial class Connection
    {
        internal static class ReferenceTypeProviders
        {
#if !DISABLE_REACTIVE_COMPONENTS
            public static class UpdatesProvider 
            {
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"UpdatesProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update> value)
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

            public static class MyEventProvider 
            {
                private static readonly Dictionary<uint, List<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType>> Storage = new Dictionary<uint, List<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType>>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, Unity.Entities.World>();
            
                private static uint nextHandle = 0;
            
                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();
            
                    Storage.Add(handle, default(List<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType>));
                    WorldMapping.Add(handle, world);
            
                    return handle;
                }
            
                public static List<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType> Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"MyEventProvider does not contain handle {handle}");
                    }
            
                    return value;
                }
            
                public static void Set(uint handle, List<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType> value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"MyEventProvider does not contain handle {handle}");
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
