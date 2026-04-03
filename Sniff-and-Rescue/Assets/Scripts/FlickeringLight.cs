using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickeringLight : MonoBehaviour
{
    private Light lightToFlicker;
    public float minIntensity = 35.0f;
    public float maxIntensity = 70.0f;
    public float flickerSpeed = 1.0f;

    private float currentTimer;

    private void Start()
    {
        lightToFlicker = GetComponent<Light>();

        InvokeRepeating("Flicker", 0f, flickerSpeed);
    }

    private void Flicker()
    {
        float randomIntensity = Random.Range(minIntensity, maxIntensity);
        lightToFlicker.intensity = randomIntensity;
    }

   
}
