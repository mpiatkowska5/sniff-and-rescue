using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using System.Collections;

public class Languages : MonoBehaviour
{
    private void Start()
    {
        Invoke("Delay", 0.1f);
    }

    private void Delay()
    {
        string lang = PlayerPrefs.GetString("lang", "en"); // default English
        
    }

    public void ChangeLanguage(string lang)
    {
        StartCoroutine(SetLocale(lang));
    }

    private IEnumerator SetLocale(string lang)
    {
        yield return LocalizationSettings.InitializationOperation;

        if (lang == "fi")
        {
            LocalizationSettings.SelectedLocale =
                LocalizationSettings.AvailableLocales.GetLocale("fi");
        }
        else // default to English
        {
            LocalizationSettings.SelectedLocale =
                LocalizationSettings.AvailableLocales.GetLocale("en");
        }

        PlayerPrefs.SetString("lang", lang);

    }

}
