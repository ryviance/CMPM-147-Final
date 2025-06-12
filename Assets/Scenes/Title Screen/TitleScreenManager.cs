using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager Instance { get; private set; }

    [Header("Bot UI")]
    public GameObject botInputTemplate;
    public Transform botListContainer;
    public List<string> listofnames = new List<string>();

    private const int maxBots = 4;
    public int BotCount { get; private set; } = 1;  // persistent count

    void Awake()
    {
        // singleton + persist
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        // seed UI to match BotCount if reloading title scene
        BotCount = 1;
        Instance.BotCount = BotCount;
    }

    public void AddBot()
    {
        if (BotCount >= maxBots) return;

        BotCount++;
        Instance.BotCount = BotCount;

        var newBot = Instantiate(botInputTemplate, botListContainer);
        newBot.name = "BotInput_" + BotCount;
        newBot.SetActive(true);

        var tmp = newBot.GetComponentInChildren<TMP_InputField>();
        if (tmp != null) tmp.text = $"Bot {BotCount}";
        else
        {
            var nf = newBot.GetComponentInChildren<InputField>();
            if (nf != null) nf.text = $"Bot {BotCount}";
        }
    }

    public void RemoveBot()
    {
        if (BotCount <= 1) return;

        int last = botListContainer.childCount - 1;
        if (last >= 0 && botListContainer.GetChild(last).gameObject != botInputTemplate)
        {
            Destroy(botListContainer.GetChild(last).gameObject);
            BotCount--;
            Instance.BotCount = BotCount;
        }
    }

    public void StartGame()
    {
        GameObject participants = GameObject.Find("ParticipantsList");
        TextMeshProUGUI[] Names = participants.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var tmp in Names)
        {
            print(tmp.text);
            if (tmp.text != "Enter text...")
            {
                listofnames.Add(tmp.text);
            }
        }

        // TitleScreenManager survives, carrying BotCount
        SceneManager.LoadScene("Main Scene");
    }
}