using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Timing
{
    [UpdateInGroup(typeof(SpatialOSReceiveGroup))]
    public class AddComponentAtLocalTimeSystem : ComponentSystem
    {
        public struct Data
        {
            public int Length;
            [ReadOnly] public ComponentDataArray<ActiveTimer> Timer;
            [ReadOnly] public EntityArray Entities;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; ++i)
            {
                // Iterate over completed timesr and add the relevant components
                var queue = data.Timer[i].Queue;
                while (queue.Count() > 0 && queue.Peak().TimerEndTime < Time.time)
                {
                    queue.Pop().AddComponentAction(PostUpdateCommands);
                }

                // If the buffer is empty, remove the timer tag
                if (queue.Count() == 0)
                {
                    PostUpdateCommands.AddComponent(data.Entities[i], new CheckForEmptyTimer());
                }
            }
        }
    }
}
