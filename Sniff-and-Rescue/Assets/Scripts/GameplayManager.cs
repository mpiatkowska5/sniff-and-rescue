using UnityEngine;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    public static float time;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text medkitText;

    void Update()
    {
        time += Time.deltaTime;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        medkitText.text = string.Format($"{medkit.medKitsCollected}/8");
    }
}
