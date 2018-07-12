using Unity.Entities;
using UnityEngine.Experimental.PlayerLoop;

namespace Improbable.Gdk.TransformSynchronization
{
    [UpdateBefore(typeof(FixedUpdate))]
    public class TransformSynchronizationGroup
    {
    }
}
