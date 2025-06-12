using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewsManager : MonoBehaviour
{
    private static NewsManager _instance;
    public static NewsManager Instance { get { return _instance; } }

    public GameObject headlinePrefab;
    public RectTransform newsContainer;
    public float pixelsPerSecond = 100f;
    public float lineSpacing = 50f;
    public float nextNewsX = 0f;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void clickHeadline(string[] names, AoeType eventType)
    {
        if (eventType == AoeType.Unsafe)
        {
            generateNegative(names);
        }
        if (eventType == AoeType.Safe)
        {
            generatePositive(names);
        }
    }

    private void generatePositive(string[] names)
    {
        string[] positiveEvents = new string[]
        {
            "Gifts containing medicine fall from the sky.",
            "Filled rice cookers were discovered in the ground.",
            "A medicinal mist has spread in the area.",
            "Bright red mushrooms have spawned along the ground.",
            names[Random.Range(0, names.Length)] + " casts Curaga.",
            "Magic beans with healing properties sprout in the area."
        };
        string chosenEvent = positiveEvents[Random.Range(0, positiveEvents.Length)];
        createHeadline(chosenEvent);
        string victims = "";
        for (int i = 0; i < names.Length; i++)
        {
            victims += names[i] + ", ";
        }
        createHeadline(victims + "have recoverd x health");
    }

    private void generateNegative(string[] names)
    {
        string[] negativeEvents = new string[]
        {
            "Wasps are released.",
            "The dogs are loose.",
            "A toxic gas is now being emitted.",
            "A fire has started in the area.",
            "Explosives rain from the sky.",
            "A mysterious fog grows nearby."
        };
        string chosenEvent = negativeEvents[Random.Range(0, negativeEvents.Length)];
        createHeadline(chosenEvent);
        string victims = "";
        for (int i = 0; i < names.Length; i++) {
            victims += names[i] + ", ";
        }
        createHeadline(victims + "have recieved x damage");
    }

    public void createHeadline(string message)
    {
        GameObject newHeadline = Instantiate(headlinePrefab, newsContainer);
        Headline headlineScript = newHeadline.GetComponent<Headline>();
        headlineScript.Initialize(newsContainer.rect.width, pixelsPerSecond, message);

        RectTransform rt = newHeadline.GetComponent<RectTransform>();
        float yPos = rt.anchoredPosition.y;
        Vector2 startPos = new Vector2(newsContainer.rect.width + nextNewsX, yPos);
        rt.anchoredPosition = startPos;
        nextNewsX += rt.rect.width + lineSpacing;
    }
}
