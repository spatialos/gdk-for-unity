using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class ProcessColorChangeSystem : ComponentSystem
    {
        public struct Data
        {
            public int Length;
            public ComponentArray<EventsReceived<ChangeColorEvent>> EventUpdate;
            public ComponentArray<MeshRenderer> Renderers;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var component = data.EventUpdate[i];
                var renderer = data.Renderers[i];
                foreach (var colorEvent in component.Buffer)
                {
                    var color = UnityEngine.Color.white;
                    switch (colorEvent.Payload.Color)
                    {
                        case Generated.Playground.Color.BLUE:
                            color = UnityEngine.Color.blue;
                            break;
                        case Generated.Playground.Color.GREEN:
                            color = UnityEngine.Color.green;
                            break;
                        case Generated.Playground.Color.YELLOW:
                            color = UnityEngine.Color.yellow;
                            break;
                        case Generated.Playground.Color.RED:
                            color = UnityEngine.Color.red;
                            break;
                    }

                    renderer.material.SetColor("_Color", color);
                }
            }
        }
    }
}
