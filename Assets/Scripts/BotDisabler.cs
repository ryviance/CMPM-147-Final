using UnityEngine;
using UnityEngine.AI;

public class BotDisabler : MonoBehaviour
{
    public GameObject[] botSlots;  // assign 0→3 in inspector

    void Start()
    {
        int keep = TitleScreenManager.Instance.BotCount;
        for (int i = keep; i < botSlots.Length; i++)
            botSlots[i].SetActive(false);

        for (int i = 0; i < TitleScreenManager.Instance.listofnames.Count; i++)
        {
            botSlots[i].name = TitleScreenManager.Instance.listofnames[i];
        }
    }

    public void PlaceBots()
    {
        for (int i = 0; i < TitleScreenManager.Instance.listofnames.Count; i++)
        {
            botSlots[i].transform.position = GetRandomWalkablePosition(new Vector3(25.5f, 25.5f, 0), 25f);
        }
    }

    public Vector3 GetRandomWalkablePosition(Vector3 center, float range)
    {
        for (int i = 0; i < 1000; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            randomPoint.z = 0f;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                print(hit.position);
                return hit.position;
            }
        }

        Debug.LogWarning("Could not find a valid walkable position.");
        return center;
    }
}