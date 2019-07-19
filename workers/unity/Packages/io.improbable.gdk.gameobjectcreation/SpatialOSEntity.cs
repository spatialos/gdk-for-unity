using Improbable.Gdk.Core;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.GameObjectCreation
{
    /// <summary>
    ///     Used to easily retrieve information about a SpatialOS Entity instance from a Unity ECS Entity instance.
    /// </summary>
    public struct SpatialOSEntity
    {
        public readonly EntityId SpatialOSEntityId;
        private readonly EntityManager entityManager;
        private readonly Entity entity;

        internal SpatialOSEntity(Entity entity, EntityManager entityManager)
        {
            this.entity = entity;
            this.entityManager = entityManager;
            SpatialOSEntityId = entityManager.GetComponentData<SpatialEntityId>(entity).EntityId;
        }

        /// <summary>
        ///     Checks if this entity has a component of type <see cref="T"/>
        /// </summary>
        /// <typeparam name="T">The SpatialOS component.</typeparam>
        /// <returns>True, if the entity has the component; false otherwise.</returns>
        public bool HasComponent<T>() where T : struct, ISpatialComponentData, IComponentData
        {
            return entityManager.HasComponent<T>(entity);
        }

        /// <summary>
        ///     Gets a component of type <see cref="T"/> attached to this entity.
        /// </summary>
        /// <typeparam name="T">The SpatialOS component.</typeparam>
        /// <returns>The component <see cref="T"/> attached to this entity.</returns>
        /// <exception cref="System.ArgumentException">Thrown if the component <see cref="T"/> is not attached to this entity.</exception>
        public T GetComponent<T>() where T : struct, ISpatialComponentData, IComponentData
        {
            return entityManager.GetComponentData<T>(entity);
        }

        /// <summary>
        ///     Attempts to get a component of type <see cref="T"/> attached to this entity.
        /// </summary>
        /// <param name="component">
        ///     When this method returns, this will be the component attached to this entity if it exists;
        ///     default constructed otherwise.
        /// </param>
        /// <typeparam name="T">The SpatialOS component.</typeparam>
        /// <returns>True, if the entity has the component; false otherwise.</returns>
        public bool TryGetComponent<T>(out T component) where T : struct, ISpatialComponentData, IComponentData
        {
            var has = HasComponent<T>();
            component = has ? GetComponent<T>() : default(T);
            return has;
        }
    }
}
