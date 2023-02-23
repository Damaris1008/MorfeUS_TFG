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
    private bool _backgroundMusic = true;
    private float _volumeLevel = 0.5f;

    [Header("Graphics Settings")]
    [SerializeField] private GameObject enableFullScreenButton = null;
    [SerializeField] private GameObject disableFullScreenButton = null;
    private bool _isFullScreen;

    [Header("Resolution Dropdowns")]
    public Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    public void Start()
    {
        _isFullScreen = Screen.fullScreen;
    }

    public void SetVolume(float volume)
    {
        _volumeLevel = volume/100;
        volumeLevelText.text = volume.ToString("0") + "%" ;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;
    }

    public void SetBackgroundMusic(bool backgroundMusic)
    {
        _backgroundMusic = backgroundMusic;
    }

    public void LoadResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i=0; i < resolutions.Length ; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetInt("fullscreen", (_isFullScreen ? 1:0));
        Screen.fullScreen = _isFullScreen;
    }
     
    public void AudioSettingsApply()
    {
        PlayerPrefs.SetFloat("volumeLevel", _volumeLevel);
        AudioListener.volume = _volumeLevel;
        PlayerPrefs.SetInt("backgroundMusic", (_backgroundMusic ? 1:0));

    }

    public void GetAudioPrefs()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volumeLevel", 0.5f) * 100;
        volumeLevelText.text = (PlayerPrefs.GetFloat("volumeLevel",0.5f)*100).ToString("0") + "%" ;

        if(PlayerPrefs.GetInt("backgroundMusic") == 0)
        {
            unmuteMusicButton.SetActive(true);
            muteMusicButton.SetActive(false);
        }else{
            unmuteMusicButton.SetActive(false);
            muteMusicButton.SetActive(true);
        }
    }

    public void GetGraphicsPrefs()
    {
        if(PlayerPrefs.GetInt("fullscreen") == 0)
        {
            disableFullScreenButton.SetActive(false);
            enableFullScreenButton.SetActive(true);
        }else{
            disableFullScreenButton.SetActive(true);
            enableFullScreenButton.SetActive(false);
        }
    }

}
