using UnityEngine;

namespace Quiztastic.Gameplay
{
    public sealed class CoinPickup : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 120f;
        [SerializeField] private float bobHeight = 0.25f;
        [SerializeField] private float bobSpeed = 2f;
        [SerializeField] private int coinValue = 1;

        private Vector3 startPosition;

        private ScoreManager scoreManager;

        private void Awake()
        {
            startPosition = transform.position;
            scoreManager = Resources.Load<ScoreManager>("ScoreManager");
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

            float offset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = startPosition + Vector3.up * offset;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerController>() == null && !other.CompareTag("Player"))
            {
                return;
            }

            scoreManager.IncreaseScore(coinValue);
            Destroy(gameObject);
        }
    }
}
