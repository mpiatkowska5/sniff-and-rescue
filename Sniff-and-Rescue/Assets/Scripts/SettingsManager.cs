using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider cameraSlider;
    
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
        Debug.Log("Current selected GameObject : " + eventSystem.currentSelectedGameObject);
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        //Save();
    }

    public void ChangeSensitivity()
    {
        PlayerController.mouseSensitivity = (20 * cameraSlider.value) + 2;
        //Debug.Log(PlayerController.mouseSensitivity);
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
