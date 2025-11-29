using UnityEngine;
public class AudioRandomizer : MonoBehaviour
{
    [Header("Timing")]
    public float minDelay = 10f;
    public float maxDelay = 30f;
    [Header("Audio")]
    public AudioSource source;
    public AudioClip[] sounds;
    void Start()
    {
        StartCoroutine(Loop());
    }
    private System.Collections.IEnumerator Loop()
    {
        while (true)
        {
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);
            if (sounds.Length > 0)
            {
                source.clip = sounds[Random.Range(0, sounds.Length)];
                source.Play();
            }
        }
    }
}
