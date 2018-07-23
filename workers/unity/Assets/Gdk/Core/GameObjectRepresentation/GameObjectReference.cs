using Improbable.Worker;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Invokes dispatcher callbacks on GameObjects that represent entities.
    /// </summary>
    public class GameObjectReference : Component
    {
        public GameObject GameObject;

        public void OnAddComponent(uint componentId)
        {
        }

        public void OnRemoveComponent(uint componentId)
        {
        }

        public void OnAuthorityChange(uint componentId, Authority authority)
        {
        }

        public void OnComponentUpdate(uint componentId, object update)
        {
        }

        public void OnEvent(uint componentId, int eventIndex, object evt)
        {
        }

        public void OnCommandRequest(uint componentId, int commandIndex, object commandRequest)
        {
        }
    }
}
