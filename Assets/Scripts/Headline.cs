using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Headline : MonoBehaviour
{
    float newsWidth;
    float pixelsPerSecond;
    RectTransform rt;

    public float GetXPosition {  get {  return rt.anchoredPosition.x; } }
    public float GetWidth { get { return rt.rect.width; } }

    public void Initialize(float newsWidth, float pixelsPerSecond, string message)
    {
        this.newsWidth = newsWidth;
        this.pixelsPerSecond = pixelsPerSecond;
        rt = GetComponent<RectTransform>();
        TextMeshProUGUI line = GetComponent<TextMeshProUGUI>();
        if (line != null )
        {
            line.text = message + " | ";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rt == null)
        {
            return;
        }
        rt.anchoredPosition += Vector2.left * pixelsPerSecond * Time.deltaTime;
        if (GetXPosition <= 0 - newsWidth - GetWidth)
        {
            Destroy(gameObject);
        }
    }
}
