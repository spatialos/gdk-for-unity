using Improbable.Worker;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class GameObjectReference : Component
    {
        public GameObject GameObject;

        public long spatialId;

        public void OnAddComponent(uint componentId)
        {
            Debug.Log(spatialId + " recevied OnAddComponent: " + componentId);
        }

        public void OnRemoveComponent(uint componentId)
        {
            Debug.Log(spatialId + " recevied OnRemoveComponent: " + componentId);
        }

        public void OnAuthorityChange(uint componentId, Authority authority)
        {
            Debug.Log(spatialId + " recevied OnAuthorityChange: " + componentId + ", " + authority);
        }

        public void OnComponentUpdate(uint componentId, object update)
        {
            Debug.Log(spatialId + " recevied OnComponentUpdate: " + componentId);
        }

        public void OnEvent(uint componentId, int eventIndex, object evt)
        {
            Debug.Log(spatialId + " recevied OnEvent: " + componentId);
        }

        public void OnCommandRequest(uint componentId, int commandIndex, object commandRequest)
        {
            Debug.Log(spatialId + " recevied OnCommandRequest: " + componentId);
        }
    }
}
