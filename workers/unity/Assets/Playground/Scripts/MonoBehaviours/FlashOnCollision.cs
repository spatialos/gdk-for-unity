using Improbable.Gdk.Core.GameObjectRepresentation;
using UnityEngine;

public class FlashOnCollision : MonoBehaviour
{
    [Require] private Generated.Playground.Collisions.Reader reader;

    private float collideTime;
    private bool flashing = false;

    void OnEnable()
    {
        if (reader != null)
        {
            reader.OnPlayerCollided += e =>
            {
                collideTime = Time.time;
                flashing = true;
                var renderer = gameObject.GetComponent<MeshRenderer>();
                renderer.material.SetColor("_Color", UnityEngine.Color.red);
            };
        }
    }

    void Update()
    {
        if (flashing && Time.time - collideTime > 0.2f)
        {
            var renderer = gameObject.GetComponent<MeshRenderer>();
            renderer.material.SetColor("_Color", UnityEngine.Color.white);
        }
    }
}
