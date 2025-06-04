using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject botInputTemplate;
    public Transform botListContainer;

    private int botCount = 1;
    private const int maxBots = 4;

    void Start() { }

    public void AddBot()
    {
        if (botCount >= maxBots) return;
        botCount++;
        GameObject newBot = Instantiate(botInputTemplate, botListContainer);
        newBot.name = "BotInput_" + botCount;
        newBot.SetActive(true);

        TMP_InputField tmpField = newBot.GetComponentInChildren<TMP_InputField>();
        if (tmpField != null)
            tmpField.text = $"Bot {botCount}";
        else
        {
            InputField normalInput = newBot.GetComponentInChildren<InputField>();
            if (normalInput != null)
                normalInput.text = $"Bot {botCount}";
        }
    }

    public void RemoveBot()
    {
        if (botCount <= 1) return;
        int lastIndex = botListContainer.childCount - 1;
        if (lastIndex >= 0)
        {
            Transform lastBot = botListContainer.GetChild(lastIndex);
            if (lastBot.gameObject != botInputTemplate)
            {
                Destroy(lastBot.gameObject);
                botCount--;
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main Scene");
    }
}