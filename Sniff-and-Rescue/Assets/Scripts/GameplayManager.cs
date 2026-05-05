using UnityEngine;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    public static float time;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text medkitText;
    [SerializeField] GameObject tipText;
    [SerializeField] GameObject runTipText;

    private void Awake()
    {
        tipText.SetActive(false);
    }

    void Update()
    {
        time += Time.deltaTime;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        medkitText.text = string.Format($"{medkit.medKitsCollected}/8");
        ActivateJumpTip();
        ActivateRunTip();

    }

    private void ActivateJumpTip()
    {
        if (PlayerController.jumpUnlocked == true)
        {
            tipText.SetActive(true);
        }
        else
        {
            tipText.SetActive(false);
        }
    }

    private void ActivateRunTip()
    {
        if(PlayerController.runTipShouldShow == true)
        {
            runTipText.SetActive(true);
        }
        else
        {
            runTipText.SetActive(false);
        }
    }
}
