using System;
using Improbable.Worker;
using Improbable.Worker.Core;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.CodegenAdapters
{
    public abstract class ComponentDispatcherHandler
    {
        public abstract uint ComponentId { get; }
        public abstract void OnAddComponent(AddComponentOp op);
        public abstract void OnRemoveComponent(RemoveComponentOp op);
        public abstract void OnComponentUpdate(ComponentUpdateOp op);
        public abstract void OnAuthorityChange(AuthorityChangeOp op);

        public abstract void OnCommandRequest(CommandRequestOp op);
        public abstract void OnCommandResponse(CommandResponseOp op);

        public abstract void AddCommandComponents(Entity entity);

        public abstract void Dispose();

        protected ILogDispatcher LogDispatcher;
        protected World World;
        protected WorkerSystem Worker;

        protected const string ReceivedDuplicateComponentAdded =
            "Received ComponentAdded, but already received one for this entity.";

        protected const string ReceivedDuplicateComponentRemoved =
            "Received ComponentRemoved, but already received one for this entity.";

        protected const string EntityNotFound = "No entity found for entity specified in op.";
        protected const string InvalidAuthorityChange = "Invalid authority state change received.";

        protected ComponentDispatcherHandler(WorkerSystem worker, World world)
        {
            LogDispatcher = worker.LogDispatcher;
            World = world;
            Worker = worker;
        }

        /// <summary>
        ///     A helper method that wraps TryGetEntity on the Worker. This throws if the entity does not exist.
        ///     Calling this method on receiving an op and not having the Entity Id specified in the op is
        ///     strictly incorrect.
        /// </summary>
        /// <param name="entityId">The Spatial EntityId to try and get.</param>
        /// <returns>The Unity ECS entity that corresponds to the Spatial Entity ID.</returns>
        /// <exception cref="InvalidSpatialEntityStateException"></exception>
        protected Entity TryGetEntityFromEntityId(EntityId entityId)
        {
            if (!Worker.TryGetEntity(entityId, out var entity))
            {
                throw new InvalidSpatialEntityStateException(
                    $"Could not find Unity Entity for Spatial Entity ID {entityId.Id}.");
            }

            return entity;
        }

        /// <summary>
        ///     An exception for when the ComponentDispatcherHandler receive a CommandRequest or CommandResponse
        ///     with an index that it does not know.
        /// </summary>
        protected class UnknownCommandIndexException : Exception
        {
            /// <param name="commandIndex">The unknown command index.</param>
            /// <param name="componentName">The name of the component that the command was received on.</param>
            public UnknownCommandIndexException(uint commandIndex, string componentName)
                : base($"Unknown command index: {commandIndex} received for {componentName}. " +
                    "This can be caused by adding commands to a component and not rebuilding workers.")
            {
            }
        }
    }
}
