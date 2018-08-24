using Generated.Playground;
using Improbable.Gdk.Core.GameObjectRepresentation;
using UnityEditor;
using UnityEngine;
using Color = UnityEngine.Color;

public class FlashOnCollision : MonoBehaviour
{
    [Require] private Collisions.Requirables.Reader reader;

    private float collideTime;
    private bool flashing = false;

    [SerializeField] private float flashTime = 0.2f;

    private MeshRenderer renderer;

    private static MaterialPropertyBlock basicMaterial;
    private static MaterialPropertyBlock flashingMaterial;

    [RuntimeInitializeOnLoadMethod]
    public static void SetupColors()
    {
        basicMaterial = new MaterialPropertyBlock();
        basicMaterial.SetColor("_Color", Color.white);

        flashingMaterial = new MaterialPropertyBlock();
        flashingMaterial.SetColor("_Color", Color.red);
    }

    private void OnEnable()
    {
        if (reader != null) // TODO UTY-791: Needed until prefab preprocessing is implemented, remove as part of UTY-791
        {
            reader.OnPlayerCollided += HandleCollisionEvent;
        }
    }

    private void Awake()
    {
        renderer = gameObject.GetComponent<MeshRenderer>();
    }

    private void OnDisable()
    {
        if (reader != null) // TODO UTY-791: Needed until prefab preprocessing is implemented, remove as part of UTY-791
        {
            reader.OnPlayerCollided -= HandleCollisionEvent;
        }
    }

    private void HandleCollisionEvent(Empty empty)
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
