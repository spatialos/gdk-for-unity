using Generated.Playground;
using Improbable.Gdk.Core.GameObjectRepresentation;
using UnityEngine;
using Color = UnityEngine.Color;

public class FlashOnCollision : MonoBehaviour
{
    [Require] private Generated.Playground.Collisions.Reader reader;

    private float collideTime;
    private bool flashing = false;
    [SerializeField] public float flashTime = 0.2f;
    [SerializeField] public Color flashColor = Color.red;

    void OnEnable()
    {
        if (reader != null) // Needed until prefab preprocessing is implemented
        {
            reader.OnPlayerCollided += HandleCollisionEvent;
        }
    }

    void OnDisable()
    {
        if (reader != null) // Needed until prefab preprocessing is implemented
        {
            reader.OnPlayerCollided -= HandleCollisionEvent;
        }
    }

    void HandleCollisionEvent(PlayerCollidedEvent e)
    {
        collideTime = Time.time;
        flashing = true;
        var renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material.SetColor("_Color", flashColor);
    }

    void Update()
    {
        if (flashing && Time.time - collideTime > flashTime)
        {
            var renderer = gameObject.GetComponent<MeshRenderer>();
            renderer.material.SetColor("_Color", UnityEngine.Color.white);
        }
    }
}
