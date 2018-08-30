using Improbable.Gdk.Core.GameObjectRepresentation;
using Playground;
using UnityEngine;

public class GameObjectCreationSetup : MonoBehaviour
{
    private void Awake()
    {
        GameObjectSystemHelper.EntityGameObjectCreator = new EntityGameObjectCreator();
    }
}
