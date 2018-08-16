using Generated.Playground;
using Improbable.Gdk.Core.GameObjectRepresentation;
using UnityEngine;
using Color = UnityEngine.Color;

public class FlashOnCollision : MonoBehaviour
{
    [Require] private Collisions.Reader reader;

    private float collideTime;
    private bool flashing = false;

    [SerializeField] private float flashTime = 0.2f;
    [SerializeField] private Color flashColor = Color.red;

    private MeshRenderer renderer;

    private MaterialPropertyBlock basicMaterial;
    private MaterialPropertyBlock flashingMaterial;

    private void OnEnable()
    {
        if (reader != null) // Needed until prefab preprocessing is implemented
        {
            reader.OnPlayerCollided += HandleCollisionEvent;
        }
    }

    private void Awake()
    {
        renderer = gameObject.GetComponent<MeshRenderer>();
        basicMaterial = new MaterialPropertyBlock();
        basicMaterial.SetColor("_Color", Color.white);
        flashingMaterial = new MaterialPropertyBlock();
        flashingMaterial.SetColor("_Color", flashColor);
    }

    private void OnDisable()
    {
        if (reader != null) // Needed until prefab preprocessing is implemented
        {
            reader.OnPlayerCollided -= HandleCollisionEvent;
        }
    }

    private void HandleCollisionEvent(PlayerCollidedEvent e)
    {
        collideTime = Time.time;
        flashing = true;
        renderer.SetPropertyBlock(flashingMaterial);
    }

    private void Update()
    {
        if (flashing && Time.time - collideTime > flashTime)
        {
            renderer.SetPropertyBlock(basicMaterial);
            flashing = false;
        }
    }
}
