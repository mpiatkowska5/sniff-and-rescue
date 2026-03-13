using UnityEngine;
using System.Collections;

public class ParticleFadeOnInteract : MonoBehaviour
{
    public ParticleSystem particles;
    public float fadeDuration = 2f;

    private ParticleSystem.MainModule mainModule;
    private bool fading = false;

    void Start()
    {
        if (particles == null)
            particles = GetComponent<ParticleSystem>();

        mainModule = particles.main;
    }

    public void OnKeyItemInteracted()
    {
        if (!fading)
            StartCoroutine(FadeOutParticles());
    }

    IEnumerator FadeOutParticles()
    {
        fading = true;

        float startAlpha = mainModule.startColor.color.a;
        float time = 0f;

        var emission = particles.emission;
        emission.rateOverTime = 0f; // stop new particles

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, time / fadeDuration);

            Color c = mainModule.startColor.color;
            c.a = alpha;
            mainModule.startColor = c;

            yield return null;
        }

        particles.Stop();
    }
}
