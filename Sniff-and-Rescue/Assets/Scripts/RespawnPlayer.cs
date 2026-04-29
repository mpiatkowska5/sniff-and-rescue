using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] public Transform respawnPoint;
    //[SerializeField] GameObject playerChar;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player in water");
            if (collider.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.SetParent(null);
                Debug.Log(respawnPoint.name);
                player.Respawn(respawnPoint);
            }
        }
    }
}
