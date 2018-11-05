using Improbable.Gdk.Subscriptions;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    public class LaunchableBehaviour : MonoBehaviour
    {
        [Require] public EntityId EntityId;
        [Require] public Entity Entity;
    }
}
