using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [Header("Audio Settings")]
    [SerializeField] public AudioSource backgroundMusicSource = null;
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
    private List<Resolution> filteredResolutions;
    private int currentResolutionIndex = 0;
    private Resolution currentResolution;

    public void Start(){
        SaveLoad.SeriouslyDeleteAllSaveFiles();
        //GetAudioPrefs();
        //GetGraphicsPrefs();
    }

    public void SetVolume(float volume)
    {
        _volumeLevel = volume/100;
        volumeLevelText.text = volume.ToString("0") + "%" ;
    }

    public void SetResolution(int resolutionIndex)
    {
        currentResolution = filteredResolutions[resolutionIndex];
        PlayerPrefs.SetInt("resolutionWidth", currentResolution.width);
        PlayerPrefs.SetInt("resolutionHeight", currentResolution.height);
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
        filteredResolutions = new List<Resolution>();
        resolutionDropdown.ClearOptions();

        for(int i = 4; i < resolutions.Length; i++)
        {
            if(!(resolutions[i].width==1280 && resolutions[i].height==600))
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for(int i = 0; i < filteredResolutions.Count; i++)
        {
            string option = filteredResolutions[i].width + "x" + filteredResolutions[i].height;
            options.Add(option);
            if(filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
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

        Screen.SetResolution(PlayerPrefs.GetInt("resolutionWidth"), PlayerPrefs.GetInt("resolutionHeight"), Screen.fullScreen);

        PlayerPrefs.SetInt("fullScreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;

        //Debug.Log("Preferencias del jugador sobre la fullscreen: "+ PlayerPrefs.GetInt("fullscreen"));
        //Debug.Log("_isFullScreen: "+_isFullScreen);
    }
     
    public void AudioSettingsApply()
    {
        PlayerPrefs.SetFloat("volumeLevel", _volumeLevel);
        AudioListener.volume = _volumeLevel;
        PlayerPrefs.SetInt("backgroundMusic", (_backgroundMusic ? 1:0));
        if(backgroundMusicSource!=null){
            if(PlayerPrefs.GetInt("backgroundMusic") == 0){
                backgroundMusicSource.mute = true;
            }else{
                backgroundMusicSource.mute = false;
            }
        }
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
        if(PlayerPrefs.GetInt("fullScreen") == 0)
        {
            _isFullScreen = false;
            Screen.fullScreen = false;
            disableFullScreenButton.SetActive(false);
            enableFullScreenButton.SetActive(true);
        }else{
            _isFullScreen = true;
            Screen.fullScreen = true;
            disableFullScreenButton.SetActive(true);
            enableFullScreenButton.SetActive(false);
        }

        if(PlayerPrefs.HasKey("resolutionWidth")){
            Screen.SetResolution(PlayerPrefs.GetInt("resolutionWidth"), PlayerPrefs.GetInt("resolutionHeight"), Screen.fullScreen);
        }else{
            Screen.SetResolution(1920, 1080, true);
        }
    }

}
