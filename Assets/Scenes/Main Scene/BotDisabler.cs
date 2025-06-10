using UnityEngine;

public class BotDisabler : MonoBehaviour
{
    public GameObject[] botSlots;  // assign 0→3 in inspector

    void Start()
    {
        int keep = TitleScreenManager.Instance.BotCount;
        for (int i = keep; i < botSlots.Length; i++)
            botSlots[i].SetActive(false);
    }
}