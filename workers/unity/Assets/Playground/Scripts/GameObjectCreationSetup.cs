using Improbable.Gdk.Core.GameObjectRepresentation;
using Playground;
using UnityEngine;

public class GameObjectCreationSetup : MonoBehaviour
{
    private void Awake()
    {
        EntityGameObjectCreationConfig.EntityGameObjectCreator = new EntityGameObjectCreator();
    }
}
