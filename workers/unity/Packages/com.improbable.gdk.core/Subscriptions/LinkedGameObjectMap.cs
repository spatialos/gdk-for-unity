using System.Collections.Generic;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Subscriptions
{
    /// <summary>
    ///     Represents the mapping between SpatiaLOS entity IDs and linked GameObjects.
    /// </summary>
    public class LinkedGameObjectMap
    {
        private EntityGameObjectLinker linker;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LinkedGameObjectMap"/> class backed with the data from
        ///     the specified <see cref="EntityGameObjectLinker"/>.
        /// </summary>
        /// <param name="linker">The linker which contains the backing data for this map.</param>
        public LinkedGameObjectMap(EntityGameObjectLinker linker)
        {
            this.linker = linker;
        }

        /// <summary>
        ///     Gets the GameObjects that are linked to a given SpatialOS entity ID.
        /// </summary>
        /// <param name="entityId">The entity ID to get GameObjects for.</param>
        /// <returns>A readonly list of the linked GameObjects or null if there are none linked.</returns>
        public IReadOnlyList<GameObject> GetLinkedGameObjects(EntityId entityId)
        {
            return linker.EntityIdToGameObjects.TryGetValue(entityId, out var goList) ? goList.AsReadOnly() : null;
        }

        /// <summary>
        ///     Tries to get the GameObjects that are linked to a given SpatialOS entity ID.
        /// </summary>
        /// <param name="entityId">The entity ID to get GameObjects for.</param>
        /// <param name="linkedGameObjects">
        ///     When this method returns, contains the GameObjects linked to the specified <see cref="EntityId"/>,
        ///     if any are linked; otherwise, null. This parameter is passed uninitialized.
        /// </param>
        /// <returns>True, if there are any GameObjects linked to the <see cref="EntityId"/>; otherwise false</returns>
        public bool TryGetLinkedGameObjects(EntityId entityId, out IReadOnlyList<GameObject> linkedGameObjects)
        {
            linkedGameObjects = GetLinkedGameObjects(entityId);
            return linkedGameObjects != null;
        }
    }


}
