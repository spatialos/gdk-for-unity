using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.Commands;

namespace Improbable.Gdk.Core
{
    public static class ComponentDatabase
    {
        internal static Dictionary<uint, IComponentMetaclass> Metaclasses { get; }

        private static Dictionary<Type, uint> ComponentsToIds { get; }
        private static Dictionary<Type, uint> ComponentUpdatesToIds { get; }
        private static Dictionary<Type, uint> ComponentEventsToIds { get; }

        private static Dictionary<Type, uint> SnapshotsToIds { get; }

        private static Dictionary<Type, ICommandMetaclass> ReceivedRequestsToCommandMetaclass { get; }
        private static Dictionary<Type, ICommandMetaclass> ReceivedResponsesToCommandMetaclass { get; }

#if UNITY_INCLUDE_TESTS
        private static Dictionary<Type, ICommandMetaclass> RequestsToCommandMetaclass { get; }
#endif
        
        static ComponentDatabase()
        {
            Metaclasses = TypeCache.ComponentMetaClassTypes.Value
                .Select(type => (IComponentMetaclass) Activator.CreateInstance(type))
                .ToDictionary(metaclass => metaclass.ComponentId, metaclass => metaclass);

            ComponentsToIds = Metaclasses.ToDictionary(pair => pair.Value.Data, pair => pair.Key);
            ComponentUpdatesToIds = Metaclasses.ToDictionary(pair => pair.Value.Update, pair => pair.Key);
            ComponentEventsToIds = Metaclasses
                .SelectMany(pair => pair.Value.Events, (pair, type) => (type, pair.Key))
                .ToDictionary(pair => pair.type, pair => pair.Key);

            SnapshotsToIds = Metaclasses.ToDictionary(pair => pair.Value.Snapshot, pair => pair.Key);

            ReceivedRequestsToCommandMetaclass = Metaclasses
                .SelectMany(pair => pair.Value.Commands, (_, cmd) => (cmd.ReceivedRequest, cmd))
                .ToDictionary(pair => pair.ReceivedRequest, pair => pair.cmd);
            ReceivedResponsesToCommandMetaclass = Metaclasses
                .SelectMany(pair => pair.Value.Commands, (_, cmd) => (cmd.ReceivedResponse, cmd))
                .ToDictionary(pair => pair.ReceivedResponse, pair => pair.cmd);

#if UNITY_INCLUDE_TESTS
            RequestsToCommandMetaclass = Metaclasses
                .SelectMany(pair => pair.Value.Commands, (_, cmd) => (cmd.Request, cmd))
                .ToDictionary(pair => pair.Request, pair => pair.cmd);
#endif
        }

        public static IComponentMetaclass GetMetaclass(uint componentId)
        {
            if (!Metaclasses.TryGetValue(componentId, out var metaclass))
            {
                throw new ArgumentException($"Can not find Metaclass for SpatialOS component ID {componentId}.");
            }

            return metaclass;
        }

        private static IComponentMetaclass GetMetaclass<T>() where T : ISpatialComponentData
        {
            if (!ComponentsToIds.TryGetValue(typeof(T), out var id))
            {
                throw new ArgumentException($"Can not find Metaclass for unregistered SpatialOS component {nameof(T)}.");
            }

            return Metaclasses[id];
        }

        private static uint GetComponentId<T>() where T : ISpatialComponentData
        {
            if (!ComponentsToIds.TryGetValue(typeof(T), out var id))
            {
                throw new ArgumentException($"Can not find ID for unregistered SpatialOS component {nameof(T)}.");
            }

            return id;
        }
        
        private static uint GetComponentIdFromUpdate<T>() where T : ISpatialComponentUpdate
        {
            if (!ComponentUpdatesToIds.TryGetValue(typeof(T), out var id))
            {
                throw new ArgumentException($"Can not find ID for unregistered SpatialOS component update {nameof(T)}.");
            }

            return id;
        }

        private static uint GetComponentIdFromEvent<T>() where T : IEvent
        {
            if (!ComponentEventsToIds.TryGetValue(typeof(T), out var id))
            {
                throw new ArgumentException($"Can not find ID for unregistered SpatialOS component event {nameof(T)}.");
            }

            return id;
        }

        public static uint GetComponentId(Type type)
        {
            if (!ComponentsToIds.TryGetValue(type, out var id))
            {
                throw new ArgumentException($"Can not find ID for unregistered SpatialOS component {type.Name}.");
            }

            return id;
        }

        public static uint GetSnapshotComponentId<T>() where T : ISpatialComponentSnapshot
        {
            if (!SnapshotsToIds.TryGetValue(typeof(T), out var id))
            {
                throw new ArgumentException($"Can not find ID for unregistered SpatialOS component snapshot {nameof(T)}.");
            }

            return id;
        }

#if UNITY_INCLUDE_TESTS
        public static ICommandMetaclass GetCommandMetaclassFromRequest<T>() where T : ICommandRequest
        {
            if (!RequestsToCommandMetaclass.TryGetValue(typeof(T), out var metaclass))
            {
                throw new ArgumentException($"Can not find ID for unregistered SpatialOS command request {nameof(T)}.");
            }

            return metaclass;
        }
#endif

        public static ICommandMetaclass GetCommandMetaclassFromReceivedResponse<T>() where T : IReceivedCommandResponse
        {
            if (!ReceivedResponsesToCommandMetaclass.TryGetValue(typeof(T), out var metaclass))
            {
                throw new ArgumentException($"Can not find ID for unregistered SpatialOS received command response {nameof(T)}.");
            }

            return metaclass;
        }

        public static ICommandMetaclass GetCommandMetaclassFromReceivedRequest<T>() where T : IReceivedCommandRequest
        {
            if (!ReceivedRequestsToCommandMetaclass.TryGetValue(typeof(T), out var metaclass))
            {
                throw new ArgumentException($"Can not find ID for unregistered SpatialOS received command request {nameof(T)}.");
            }

            return metaclass;
        }

        public static class ComponentType<T> where T : ISpatialComponentData
        {
            public static readonly uint ComponentId;
            public static readonly IComponentMetaclass Metaclass;
            
            static ComponentType()
            {
                ComponentId = GetComponentId<T>();
                Metaclass = GetMetaclass<T>();
            }
        }
        
        public static class ComponentUpdateType<T> where T : ISpatialComponentUpdate
        {
            public static readonly uint ComponentId;
            
            static ComponentUpdateType()
            {
                ComponentId = GetComponentIdFromUpdate<T>();
            }
        }
        
        public static class ComponentEventType<T> where T : IEvent
        {
            public static readonly uint ComponentId;
            
            static ComponentEventType()
            {
                ComponentId = GetComponentIdFromEvent<T>();
            }
        }

        public static class ComponentCommandRequestType<T> where T : IReceivedCommandRequest
        {
            public static readonly uint ComponentId;
            public static readonly uint CommandIndex;
            
            static ComponentCommandRequestType()
            {
                var meta = GetCommandMetaclassFromReceivedRequest<T>();
                ComponentId = meta.ComponentId;
                CommandIndex = meta.CommandIndex;
            }
        }

        public static class ComponentCommandResponseType<T> where T : IReceivedCommandResponse
        {
            public static readonly uint ComponentId;
            public static readonly uint CommandIndex;
            
            static ComponentCommandResponseType()
            {
                var meta = GetCommandMetaclassFromReceivedResponse<T>();
                ComponentId = meta.ComponentId;
                CommandIndex = meta.CommandIndex;
            }
        }
    }
}
