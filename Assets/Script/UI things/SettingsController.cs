using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsController : MonoBehaviour
{
    public AudioMixer _mixer;
    public Slider _volumeSlider;
    public TMPro.TMP_Dropdown qualityDropDown;
    public GameObject settingsMenu;

    public const string MasterVolume = "volume";

    private void Awake()
    {
        _volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void Start()
    {
        _volumeSlider.value = PlayerPrefs.GetFloat(MasterVolume, 1f);
    }


    private void OnDisable()
    {
        PlayerPrefs.SetInt("Quality", QualitySettings.GetQualityLevel());

        //Saves the volume level in playerOutline prefs
        PlayerPrefs.SetFloat(MasterVolume, _volumeSlider.value);
    }



    

    private void SetVolume(float volume)
    {
        //Sets the the volume to the mixer
        //Mathf.log10 is used as the slides gives a linear value from -1 to 1 but the mixer gives a logarithmic value which goes by x10 or /10 
        //Times 20 to reach -80 db


        _mixer.SetFloat(MasterVolume, Mathf.Log10(volume) * 20);


    }


    public void Update()
    {
        UpdateSettings();
    }


    public void SetQuality(int QualityIndex)
    {
        QualitySettings.SetQualityLevel(QualityIndex);
    }

    public void UpdateSettings()
    {
        
        qualityDropDown.value = QualitySettings.GetQualityLevel();
    }

    public void ReturnToPreviousMenu()
    {
        settingsMenu.SetActive(false);
    }

    public void OnApplicationQuit()
    {
        //Saves the volume level in playerOutline prefs
        //PlayerPrefs.SetFloat(MasterVolume, _volumeSlider.value);
        PlayerPrefs.SetInt("Quality", QualitySettings.GetQualityLevel());
    }
}
