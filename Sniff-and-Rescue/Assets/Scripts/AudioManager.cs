using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioLibrary audioLibrary;
    [SerializeField] private AudioSource musicSource;

    private string currentTrack;
    private float targetVolume = 1f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        targetVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        musicSource.volume = targetVolume;
    }

    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        AudioClip newClip = audioLibrary.GetClipFromName(trackName);

        if (newClip == null)
        {
            Debug.LogWarning("Track not found: " + trackName);
            return;
        }

        // ONLY skip if SAME clip is already playing
        if (musicSource.clip == newClip && musicSource.isPlaying)
            return;

        currentTrack = trackName;

        StopAllCoroutines();
        StartCoroutine(AnimateMusicCrossfade(newClip, fadeDuration));
    }

    public void SetVolume(float volume)
    {
        targetVolume = volume;
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration)
    {
        float startVolume = musicSource.volume;
        float percent = 0;

        // Fade out
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(startVolume, 0, percent);
            yield return null;
        }

        musicSource.clip = nextTrack;
        musicSource.Play();

        percent = 0;

        // Fade in to target volume (not always 1!)
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(0, targetVolume, percent);
            yield return null;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Instance == null) return;

        Debug.Log("Scene: " + scene.name);

        switch (scene.name)
        {
            case "UI_MainMenu":
                PlayMusic("MainMenu");
                break;

            case "Bryan_modular_kit_prototype":
                PlayMusic("Game - wind");
                break;
        }
    }
}