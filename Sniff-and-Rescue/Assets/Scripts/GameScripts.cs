using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    void Start()
    {
        MusicManager.Instance.PlayMusic("Game - wind");
    }
}