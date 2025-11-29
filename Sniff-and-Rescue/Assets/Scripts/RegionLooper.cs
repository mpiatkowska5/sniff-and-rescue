using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class RegionLooper : MonoBehaviour
{
    public AudioSource source;
    public float loopStart = 15f;
    public float loopEnd = 20f;
    void Reset()
    {
        source = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (!source.isPlaying) return;
        if (loopEnd <= loopStart) return;
        if (source.time >= loopEnd)
        {
            source.time = loopStart;
        }
    }
}
