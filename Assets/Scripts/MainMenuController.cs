using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [Header("Audio Settings")]
    [SerializeField] private Text volumeLevelText = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private GameObject muteMusicButton = null;
    [SerializeField] private GameObject unmuteMusicButton = null;

    [Header("Graphics Settings")]
    [SerializeField] private Text brightnessLevelText = null;
    [SerializeField] private Slider brightnessSlider = null;
    private float _brightnessLevel;
    private bool _isFullScreen;
    private int _qualityLevel;
    
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeLevelText.text = volume.ToString("0") + "%" ;
    }

    /*public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessLevelText = brightness.ToString("0.0");
    }

    public void SetFullscreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {

    }*/
     
    public void AudioSettingsApply()
    {
        PlayerPrefs.SetFloat("volumeLevel", AudioListener.volume);
        
        if(muteMusicButton.activeSelf){
            PlayerPrefs.SetInt("backgroundMusic", 1); //Play music
        }else{
            PlayerPrefs.SetInt("backgroundMusic", 0); //Mute music
        }
    }

    public void GetAudioPrefs()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volumeLevel");
        volumeLevelText.text = PlayerPrefs.GetFloat("volumeLevel").ToString("0") + "%" ;

        if(PlayerPrefs.GetInt("backgroundMusic") == 0)
        {
            unmuteMusicButton.SetActive(true);
            muteMusicButton.SetActive(false);
        }else{
            unmuteMusicButton.SetActive(false);
            muteMusicButton.SetActive(true);
        }

    }

}
