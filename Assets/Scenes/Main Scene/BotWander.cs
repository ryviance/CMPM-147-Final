using UnityEngine;
using UnityEngine.AI;

public class BotWander : MonoBehaviour
{
    public float roamRadius = 15f;
    public float minRoamDistance = 1f;
    public float roamDelay = 0f;

    private NavMeshAgent agent;
    private float nextRoamTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // Kick‐start the Bot at Z = -0.1
        Vector3 startPos = transform.position;
        startPos.z = 5f;
        transform.position = startPos;

        nextRoamTime = Time.time + roamDelay;
    }

    void Update()
    {
        if (!agent.isOnNavMesh)
            return;

        if (agent.pathPending || agent.remainingDistance > 0.5f)
            return;

        if (Time.time < nextRoamTime)
            return;

        Vector3 randomDestination = GetRandomNavMeshPoint(roamRadius);
        if (Vector3.Distance(transform.position, randomDestination) >= minRoamDistance)
        {
            // Only X/Y matters; NavMeshAgent will move the Bot on the mesh’s plane (Z=0)
            agent.SetDestination(new Vector3(randomDestination.x, randomDestination.y, 0f));
            nextRoamTime = Time.time + roamDelay;
        }
    }

    // After the agent has moved this frame, force Z back to +0.1 so that the user can see bots
    void LateUpdate()
    {
        Vector3 p = transform.position;
        p.z = 5f;
        transform.position = p;
    }

    Vector3 GetRandomNavMeshPoint(float radius)
    {
        Vector3 rnd = transform.position + Random.insideUnitSphere * radius;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(rnd, out hit, radius, NavMesh.AllAreas))
        {
            // Return the NavMesh coordinates, but Z=0 (agent will sit at Z=0 on the mesh)
            return new Vector3(hit.position.x, hit.position.y, 0f);
        }
        return new Vector3(transform.position.x, transform.position.y, 0f);
    }
}