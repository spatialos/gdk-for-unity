using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    /// <summary>
    /// A toolkit for copying components based on ComponentType.
    /// </summary>
    public unsafe class EntityDataCopyKit
    {
        private EntityManager entityManager;

        private delegate void EmSetSharedComponentDataRaw(Entity entity, int typeIndex, object componentData);

        private delegate void EmSetComponentDataRaw(Entity entity, int typeIndex, void* data, int size);

        private delegate void* EmGetComponentDataRawRO(Entity entity, int typeIndex);

        private delegate void* EmGetComponentDataRawRW(Entity entity, int typeIndex);

        private delegate object EmGetSharedComponentData(Entity entity, int typeIndex);

        private delegate void* EmGetBufferRawRW(Entity entity, int typeIndex);

        private delegate void* EmGetBufferRawRO(Entity entity, int typeIndex);

        private delegate int EmGetBufferLength(Entity entity, int typeIndex);

        private EmSetSharedComponentDataRaw emSetSharedComponentDataRaw;
        private EmSetComponentDataRaw emSetComponentDataRaw;
        private EmGetComponentDataRawRO emGetComponentDataRawRO;
        private EmGetComponentDataRawRW emGetComponentDataRawRW;
        private EmGetSharedComponentData emGetSharedComponentData;
        private EmGetBufferRawRW emGetBufferRawRW;
        private EmGetBufferRawRO emGetBufferRawRO;
        private EmGetBufferLength emGetBufferLength;

        public EntityDataCopyKit(EntityManager entityManager)
        {
            this.entityManager = entityManager;

            var emType = typeof(EntityManager);

            var methodInfo = GetMethod("SetSharedComponentDataBoxedDefaultMustBeNull", 3);
            emSetSharedComponentDataRaw = methodInfo.CreateDelegate(typeof(EmSetSharedComponentDataRaw), this.entityManager) as EmSetSharedComponentDataRaw;

            methodInfo = emType.GetMethod("SetComponentDataRaw", BindingFlags.Instance | BindingFlags.NonPublic);
            emSetComponentDataRaw = methodInfo.CreateDelegate(typeof(EmSetComponentDataRaw), this.entityManager) as EmSetComponentDataRaw;

            methodInfo = emType.GetMethod("GetComponentDataRawRO", BindingFlags.Instance | BindingFlags.NonPublic);
            emGetComponentDataRawRO = methodInfo.CreateDelegate(typeof(EmGetComponentDataRawRO), this.entityManager) as EmGetComponentDataRawRO;

            methodInfo = emType.GetMethod("GetComponentDataRawRW", BindingFlags.Instance | BindingFlags.NonPublic);
            emGetComponentDataRawRW = methodInfo.CreateDelegate(typeof(EmGetComponentDataRawRW), this.entityManager) as EmGetComponentDataRawRW;

            methodInfo = GetMethod("GetSharedComponentData", 2);
            emGetSharedComponentData = methodInfo.CreateDelegate(typeof(EmGetSharedComponentData), this.entityManager) as EmGetSharedComponentData;

            methodInfo = emType.GetMethod("GetBufferRawRW", BindingFlags.Instance | BindingFlags.NonPublic);
            emGetBufferRawRW = methodInfo.CreateDelegate(typeof(EmGetBufferRawRW), this.entityManager) as EmGetBufferRawRW;

            methodInfo = emType.GetMethod("GetBufferRawRO", BindingFlags.Instance | BindingFlags.NonPublic);
            emGetBufferRawRO = methodInfo.CreateDelegate(typeof(EmGetBufferRawRO), this.entityManager) as EmGetBufferRawRO;

            methodInfo = emType.GetMethod("GetBufferLength", BindingFlags.Instance | BindingFlags.NonPublic);
            emGetBufferLength = methodInfo.CreateDelegate(typeof(EmGetBufferLength), this.entityManager) as EmGetBufferLength;
        }

        /// <summary>
        /// Copies the data stored in the componentType from the src entity to the dst entity.>
        /// </summary>
        /// <param name="src">The source entity</param>
        /// <param name="dst">The destination entity</param>
        /// <param name="componentType">The type of data to be copied</param>
        public void CopyData(Entity src, Entity dst, ComponentType componentType)
        {
            //Check to ensure dst has componentType
            if (!entityManager.HasComponent(dst, componentType))
            {
                entityManager.AddComponent(dst, componentType);
            }

            if (componentType.IsSharedComponent)
            {
                CopyScd(src, dst, componentType);
            }
            else if (componentType.IsBuffer)
            {
                CopyBuffer(src, dst, componentType);
            }
            else
            {
                CopyIcd(src, dst, componentType);
            }
        }

        private void CopyIcd(Entity src, Entity dst, ComponentType componentType)
        {
            if (componentType.IsZeroSized)
            {
                return;
            }

            var typeInfo = TypeManager.GetTypeInfo(componentType.TypeIndex);
            var size = typeInfo.SizeInChunk;
            var data = emGetComponentDataRawRO(src, componentType.TypeIndex);
            emSetComponentDataRaw(dst, componentType.TypeIndex, data, size);
        }

        private void CopyScd(Entity src, Entity dst, ComponentType componentType)
        {
            var data = emGetSharedComponentData(src, componentType.TypeIndex);
            emSetSharedComponentDataRaw(dst, componentType.TypeIndex, data);
        }

        private void CopyBuffer(Entity src, Entity dst, ComponentType componentType)
        {
            var length = emGetBufferLength(src, componentType.TypeIndex);
            var typeInfo = TypeManager.GetTypeInfo(componentType.TypeIndex);
            var elementSize = typeInfo.ElementSize;
            var alignment = typeInfo.AlignmentInBytes;

            var dstHeader = (FakeBufferHeader*) emGetComponentDataRawRW(dst, componentType.TypeIndex);
            FakeBufferHeader.EnsureCapacity(dstHeader, length, elementSize, alignment, FakeBufferHeader.TrashMode.RetainOldData, false, 0);
            dstHeader->Length = length;

            var dstBufferPtr = emGetBufferRawRW(dst, componentType.TypeIndex);
            var srcBufferPtr = emGetBufferRawRO(src, componentType.TypeIndex);
            UnsafeUtility.MemCpy(dstBufferPtr, srcBufferPtr, elementSize * length);
        }

        private static MethodInfo GetMethod(string methodName, int numOfArgs)
        {
            var methods = typeof(EntityManager).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                if (method.Name == methodName && method.GetParameters().Length == numOfArgs)
                {
                    return method;
                }
            }

            return null;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct FakeBufferHeader
    {
        private const int MinimumCapacity = 8;

        [FieldOffset(0)] private byte* Pointer;
        [FieldOffset(8)] public int Length;
        [FieldOffset(12)] private int Capacity;

        private static byte* GetElementPointer(FakeBufferHeader* header)
        {
            if (header->Pointer != null)
            {
                return header->Pointer;
            }

            return (byte*) (header + 1);
        }

        public enum TrashMode
        {
            TrashOldData,
            RetainOldData
        }

        public static void EnsureCapacity(FakeBufferHeader* header, int count, int typeSize, int alignment, TrashMode trashMode, bool useMemoryInitPattern, byte memoryInitPattern)
        {
            if (count <= header->Capacity)
            {
                return;
            }

            var adjustedCount = Math.Max(MinimumCapacity, Math.Max(2 * header->Capacity, count)); // stop pathological performance of ++Capacity allocating every time, tiny Capacities
            SetCapacity(header, adjustedCount, typeSize, alignment, trashMode, useMemoryInitPattern, memoryInitPattern, 0);
        }


        private static void SetCapacity(FakeBufferHeader* header, int count, int typeSize, int alignment, TrashMode trashMode, bool useMemoryInitPattern, byte memoryInitPattern, int internalCapacity)
        {
            var newCapacity = count;
            if (newCapacity == header->Capacity)
            {
                return;
            }

            var newSizeInBytes = (long) newCapacity * typeSize;

            var oldData = GetElementPointer(header);
            var newData = (newCapacity <= internalCapacity) ? (byte*) (header + 1) : (byte*) UnsafeUtility.Malloc(newSizeInBytes, alignment, Allocator.Persistent);

            if (oldData != newData) // if at least one of them isn't the internal pointer...
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if (useMemoryInitPattern)
                {
                    if (trashMode == TrashMode.RetainOldData)
                    {
                        var oldSizeInBytes = (header->Capacity * typeSize);
                        var bytesToInitialize = newSizeInBytes - oldSizeInBytes;
                        if (bytesToInitialize > 0)
                        {
                            UnsafeUtility.MemSet(newData + oldSizeInBytes, memoryInitPattern, bytesToInitialize);
                        }
                    }
                    else
                    {
                        UnsafeUtility.MemSet(newData, memoryInitPattern, newSizeInBytes);
                    }
                }
#endif
                if (trashMode == TrashMode.RetainOldData)
                {
                    var bytesToCopy = Math.Min((long) header->Capacity, count) * typeSize;
                    UnsafeUtility.MemCpy(newData, oldData, bytesToCopy);
                }

                // Note we're freeing the old buffer only if it was not using the internal capacity. Don't change this to 'oldData', because that would be a bug.
                if (header->Pointer != null)
                {
                    UnsafeUtility.Free(header->Pointer, Allocator.Persistent);
                }
            }

            header->Pointer = (newData == (byte*) (header + 1)) ? null : newData;
            header->Capacity = newCapacity;
        }
    }
}
