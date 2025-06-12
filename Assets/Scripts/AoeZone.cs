using UnityEngine;

public enum AoeType { Safe, Unsafe }

public class AoeZone : MonoBehaviour
{
    [Header("Tweakables")]
    public float startRadius = 0.5f;   // initial world-unit radius
    public float expansionSpeed = 3f;     // units per second
    public float lifeTime = 5f;     // seconds before auto-destroy
    public float maxRadius = 20f;    // world-unit cap on radius

    public AoeType Type { get; private set; }
    public float Radius { get; private set; }

    private Vector3 _center;
    private float _elapsed;
    private bool _initialized;            // ← only run Update after Initialize

    /// <summary>
    /// Must call right after Instantiate to set up and start the zone.
    /// </summary>
    public void Initialize(Vector3 center, AoeType type)
    {
        _initialized = true;
        _center = center;
        transform.position = center;
        Type = type;
        Radius = startRadius;
        transform.localScale = Vector3.one * startRadius * 2f;
        _elapsed = 0f;              // reset timer
    }

    void Update()
    {
        if (!_initialized) return;             // ← ignore until Initialize()

        _elapsed += Time.deltaTime;
        if (_elapsed >= lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        // grow but cap at maxRadius
        Radius = Mathf.Min(Radius + expansionSpeed * Time.deltaTime, maxRadius);
        transform.localScale = Vector3.one * Radius * 2f;
    }

    public Vector3 Center => _center;
}