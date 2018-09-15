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
        protected const string CommandIndexNotFound = "Command index not found.";
        protected const string InvalidAuthorityChange = "Invalid authority state change received.";

        protected ComponentDispatcherHandler(WorkerSystem worker, World world)
        {
            LogDispatcher = worker.LogDispatcher;
            World = world;
            Worker = worker;
        }

        /// <summary>
        ///     A helper method that wraps TryGetEntity on the Worker. This throws if the entity does not exist.
        ///     The reason is throws is that this method is called on receiving ops and not having that Entity Id is
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
    }
}
