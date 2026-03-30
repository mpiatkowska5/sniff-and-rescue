using UnityEngine;

public class BirdFlock : MonoBehaviour
{
    public float speed = 10f;
    public float waveIntensity = 1.5f;
    public float lifeSpan = 15f; 

    private Vector3 noiseOffset;

    void Start()
    {
        
        noiseOffset = new Vector3(Random.value, Random.value, Random.value) * 100f;
        Destroy(gameObject, lifeSpan); 
    }

    void Update()
    {
        
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

      
        float x = (Mathf.PerlinNoise(Time.time, noiseOffset.x) - 0.5f);
        float y = (Mathf.PerlinNoise(Time.time, noiseOffset.y) - 0.5f);

        Vector3 drift = new Vector3(x, y, 0) * waveIntensity * Time.deltaTime;
        transform.position += transform.TransformDirection(drift);
    }
}