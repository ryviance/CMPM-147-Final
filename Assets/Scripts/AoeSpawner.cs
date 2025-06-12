using UnityEngine;

public class AoeSpawner : MonoBehaviour
{
    [Tooltip("Freeze‐AOE prefab")]
    public AoeZone freezeAoePrefab;
    [Tooltip("Burn‐AOE prefab")]
    public AoeZone burnAoePrefab;
    [Tooltip("Starting radius of each AOE (in world units)")]
    public float startRadius = 0.5f;
    [Tooltip("How fast each AOE expands (units/sec)")]
    public float expansionSpeed = 3f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SpawnAoe(AoeType.Safe);
        if (Input.GetMouseButtonDown(1))
            SpawnAoe(AoeType.Unsafe);
    }

    private void SpawnAoe(AoeType type)
    {
        // get click position in world
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        wp.z = 0f;

        // pick the right prefab for freeze vs burn
        AoeZone prefab = (type == AoeType.Safe)
            ? freezeAoePrefab
            : burnAoePrefab;

        // instantiate and apply settings
        AoeZone zone = Instantiate(prefab);
        zone.startRadius = startRadius;
        zone.expansionSpeed = expansionSpeed;
        zone.Initialize(wp, type);
    }

    public void SpawnAoeAtBot(AoeType type, GameObject bot)
    {
        // get click position in world
        Vector3 wp = bot.transform.position;
        wp.z = 0f;

        // pick the right prefab for freeze vs burn
        AoeZone prefab = (type == AoeType.Safe)
            ? freezeAoePrefab
            : burnAoePrefab;

        // instantiate and apply settings
        AoeZone zone = Instantiate(prefab);
        zone.startRadius = startRadius;
        zone.expansionSpeed = expansionSpeed;
        zone.Initialize(wp, type);
    }
}
