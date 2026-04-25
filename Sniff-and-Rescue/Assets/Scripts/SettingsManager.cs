using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider cameraSlider;
    EventSystem eventSystem;


    private void Awake()
    {
        eventSystem = EventSystem.current;
    }

    void Start()
    {
        if(!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
           // Load();
        }

        //else
        // {
        //     Load();
        //}

    }

    private void OnEnable()
    {
        eventSystem.SetSelectedGameObject(volumeSlider.gameObject);
        //volumeSlider.Select();
        Debug.Log("Current selected GameObject : " + eventSystem.currentSelectedGameObject);
        
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        //Save();
    }

    //private void Load()
    //{
    //    volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    //}
    //private void Save()
    //{
       // PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    //}
}
