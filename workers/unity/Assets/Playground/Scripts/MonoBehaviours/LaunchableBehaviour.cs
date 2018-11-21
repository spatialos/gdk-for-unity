using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
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
