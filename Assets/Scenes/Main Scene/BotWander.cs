using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotWander : MonoBehaviour
{
    [Header("Roam Settings")]
    public float roamRadius = 15f;  // max leg distance
    public float minRoamDistance = 2f;  // min leg distance
    public float roamDelay = 0f;  // delay between picks

    private NavMeshAgent agent;
    private float nextRoamTime;

    // map each AoE type to a response action
    private Dictionary<AoeType, Action<AoeZone>> aoeHandlers;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // keep bots visible above your background
        Vector3 p = transform.position;
        p.z = 0f;
        transform.position = p;

        nextRoamTime = Time.time + roamDelay;
        agent.updateRotation = false;

        // build our AoE‐type → behavior map
        aoeHandlers = new Dictionary<AoeType, Action<AoeZone>>()
        {
            { AoeType.Safe,   HandleSafeAoe   },
            { AoeType.Unsafe, HandleUnsafeAoe }
            // ← when you add new AoeType enums, just add them here
        };
    }

    void Update()
    {
        if (!agent.isOnNavMesh) return;

        // 1) check for any overlapping AoE
        foreach (var zone in FindObjectsOfType<AoeZone>())
        {
            float d = Vector3.Distance(transform.position, zone.Center);
            if (d <= zone.Radius)
            {
                // invoke the handler if it exists, or default to no effect
                if (aoeHandlers.TryGetValue(zone.Type, out var handler))
                    handler(zone);
                else
                    agent.isStopped = false;  // default: keep wandering

                return; // skip normal wander this frame
            }
        }

        // 2) no AoE under us → resume wander
        agent.isStopped = false;
        if (agent.pathPending || agent.remainingDistance > 0.5f) return;
        if (Time.time < nextRoamTime) return;

        DoWander();
    }

    private void DoWander()
    {
        // pick a random 2D direction and distance
        Vector2 rnd = UnityEngine.Random.insideUnitCircle.normalized;
        float dist = UnityEngine.Random.Range(minRoamDistance, roamRadius);
        Vector3 target = transform.position + new Vector3(rnd.x, rnd.y, 0f) * dist;

        if (NavMesh.SamplePosition(target, out var hit, roamRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(new Vector3(hit.position.x, hit.position.y, 0f));
        }

        nextRoamTime = Time.time + roamDelay;
        transform.rotation = Quaternion.identity; // keep upright
    }

    private void HandleSafeAoe(AoeZone zone)
    {
        // freeze in place
        agent.isStopped = true;
    }

    private void HandleUnsafeAoe(AoeZone zone)
    {
        // flee outward
        agent.isStopped = false;
        Vector3 dir = (transform.position - zone.Center).normalized;
        Vector3 raw = transform.position + dir * roamRadius;
        if (NavMesh.SamplePosition(raw, out var hit, roamRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(new Vector3(hit.position.x, hit.position.y, 0f));
        }
    }

    void LateUpdate()
    {
        // maintain Z offset
        Vector3 p = transform.position;
        p.z = 0f;
        transform.position = p;
    }
}