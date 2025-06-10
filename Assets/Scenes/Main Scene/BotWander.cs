using UnityEngine;
using UnityEngine.AI;

public class BotWander : MonoBehaviour
{
    [Header("Roam Settings")]
    public float roamRadius = 15f;        // max leg distance
    public float minRoamDistance = 2f;    // min leg distance
    public float roamDelay = 0f;          // delay between picks

    private NavMeshAgent agent;
    private float nextRoamTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // offset in Z so bot is visible
        var startPos = transform.position;
        startPos.z = -1f;
        transform.position = startPos;

        nextRoamTime = Time.time + roamDelay;
        agent.updateRotation = false;
    }

    void Update()
    {
        if (!agent.isOnNavMesh) return;
        if (agent.pathPending || agent.remainingDistance > 0.5f) return;
        if (Time.time < nextRoamTime) return;

        // pick random dir & distance
        Vector2 dir2D = Random.insideUnitCircle.normalized;
        float dist = Random.Range(minRoamDistance, roamRadius);
        var rawTarget = transform.position + new Vector3(dir2D.x, dir2D.y, 0f) * dist;

        // sample on NavMesh
        if (NavMesh.SamplePosition(rawTarget, out var hit, roamRadius, NavMesh.AllAreas))
        {
            var dest = new Vector3(hit.position.x, hit.position.y, 0f);
            agent.SetDestination(dest);
        }

        nextRoamTime = Time.time + roamDelay;
        transform.rotation = Quaternion.identity;  // keep upright
    }

    void LateUpdate()
    {
        // maintain Z offset
        var p = transform.position;
        p.z = -1f;
        transform.position = p;
    }
}