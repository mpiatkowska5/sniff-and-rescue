using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab;
    public Transform cameraTransform;

    [Header("Settings")]
    public float spawnDistance = 40f;  
    public float sideSpread = 20f;   
    public float heightOffset = 5f;   

    void Start()
    {
        if (cameraTransform == null) cameraTransform = Camera.main.transform;
        InvokeRepeating("SpawnFlock", 2f, Random.Range(5f, 10f));
    }

    void SpawnFlock()
    {

        Vector3 spawnPos = cameraTransform.position
                           - (cameraTransform.forward * 10f)
                           + (cameraTransform.right * Random.Range(-sideSpread, sideSpread))
                           + (Vector3.up * heightOffset);


        Vector3 targetPos = cameraTransform.position
                            + (cameraTransform.forward * spawnDistance)
                            + (cameraTransform.right * Random.Range(-sideSpread, sideSpread));


        GameObject flock = Instantiate(birdPrefab, spawnPos, Quaternion.LookRotation(targetPos - spawnPos));


        flock.GetComponent<BirdFlock>().speed = Random.Range(8f, 15f);
    }
}