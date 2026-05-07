using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    private void Awake()
    {
        if (levelManager == null)
        {
            levelManager = FindFirstObjectByType<LevelManager>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || levelManager == null)
        {
            return;
        }

        levelManager.EndLevel();
    }
}
