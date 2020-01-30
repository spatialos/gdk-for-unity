// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;

namespace Improbable.TestSchema
{
    public partial class RecursiveComponent
    {
        internal static class ReferenceTypeProviders
        {
            public static class AProvider
            {
                private static readonly Dictionary<uint, global::Improbable.TestSchema.TypeA> Storage = new Dictionary<uint, global::Improbable.TestSchema.TypeA>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, global::Unity.Entities.World>();

                private static uint nextHandle = 0;

                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();

                    Storage.Add(handle, default(global::Improbable.TestSchema.TypeA));
                    WorldMapping.Add(handle, world);

                    return handle;
                }

                public static global::Improbable.TestSchema.TypeA Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"AProvider does not contain handle {handle}");
                    }

                    return value;
                }

                public static void Set(uint handle, global::Improbable.TestSchema.TypeA value)
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

            public static class BProvider
            {
                private static readonly Dictionary<uint, global::Improbable.TestSchema.TypeB> Storage = new Dictionary<uint, global::Improbable.TestSchema.TypeB>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, global::Unity.Entities.World>();

                private static uint nextHandle = 0;

                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();

                    Storage.Add(handle, default(global::Improbable.TestSchema.TypeB));
                    WorldMapping.Add(handle, world);

                    return handle;
                }

                public static global::Improbable.TestSchema.TypeB Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"BProvider does not contain handle {handle}");
                    }

                    return value;
                }

                public static void Set(uint handle, global::Improbable.TestSchema.TypeB value)
                {
                    if (!Storage.ContainsKey(handle))
                    {
                        throw new ArgumentException($"BProvider does not contain handle {handle}");
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
                private static readonly Dictionary<uint, global::Improbable.TestSchema.TypeC> Storage = new Dictionary<uint, global::Improbable.TestSchema.TypeC>();
                private static readonly Dictionary<uint, global::Unity.Entities.World> WorldMapping = new Dictionary<uint, global::Unity.Entities.World>();

                private static uint nextHandle = 0;

                public static uint Allocate(global::Unity.Entities.World world)
                {
                    var handle = GetNextHandle();

                    Storage.Add(handle, default(global::Improbable.TestSchema.TypeC));
                    WorldMapping.Add(handle, world);

                    return handle;
                }

                public static global::Improbable.TestSchema.TypeC Get(uint handle)
                {
                    if (!Storage.TryGetValue(handle, out var value))
                    {
                        throw new ArgumentException($"CProvider does not contain handle {handle}");
                    }

                    return value;
                }

                public static void Set(uint handle, global::Improbable.TestSchema.TypeC value)
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
        }
    }
}
