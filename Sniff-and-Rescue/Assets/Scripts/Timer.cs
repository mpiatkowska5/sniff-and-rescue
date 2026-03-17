using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public static float time;
    TMP_Text timerText;

    void Awake()
    {
        timerText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        time += Time.deltaTime;
        int  minutes = Mathf.FloorToInt(time/60);
        int seconds = Mathf.FloorToInt(time%60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
