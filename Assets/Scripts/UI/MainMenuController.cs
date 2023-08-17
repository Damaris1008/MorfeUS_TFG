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

    [Header("Graphics Settings: FullScreen")]
    [SerializeField] private GameObject enableFullScreenButton = null;
    [SerializeField] private GameObject disableFullScreenButton = null;
    private bool _isFullScreen;

    [Header("Graphics Settings: Brightness")]
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Text brightnessLevelText = null;
    [SerializeField] private Image brightnessPanel;
    [SerializeField] private Image brightnessPanelTest;
    public float brightnessSliderValue;

    public void Start(){
        SaveLoad.SeriouslyDeleteAllSaveFiles();
    }

    public void SetVolume(float volume)
    {
        _volumeLevel = volume/100;
        volumeLevelText.text = volume.ToString("0") + "%" ;
    }

    public void SetFullscreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;
    }

    public void SetBackgroundMusic(bool backgroundMusic)
    {
        _backgroundMusic = backgroundMusic;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetInt("fullScreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;

        //Brightness
        PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
        brightnessPanel.color = new Color(brightnessPanel.color.r, brightnessPanel.color.g, brightnessPanel.color.b, (100-brightnessSlider.value)*180/100/255);
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

        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1f);
        brightnessPanelTest.color = new Color(brightnessPanel.color.r, brightnessPanel.color.g, brightnessPanel.color.b, (100-brightnessSlider.value)*180/100/255);
    }

    public void ChangeSlider(float value)
    {
        brightnessSliderValue = value;
        brightnessLevelText.text = value.ToString("0") + "%" ;

        //Formula explanation:
        //100-brightnessSlider.value -> more slider value, less alpha (more brightness)
        //Multiplied by 180 -> 180 will be max value on alpha
        //Divided by 100 and 255 -> in order to make alpha be between 0 and 1
        brightnessPanelTest.color = new Color(brightnessPanel.color.r, brightnessPanel.color.g, brightnessPanel.color.b, (100-brightnessSlider.value)*180/100/255);
    }

    public void EnableBrightnessPanelTest(){
        brightnessPanelTest.gameObject.SetActive(true);
        brightnessPanel.gameObject.SetActive(false);
    }

    public void DisableBrightnessPanelTest(){
        brightnessPanelTest.gameObject.SetActive(false);
        brightnessPanel.gameObject.SetActive(true);
    }



}
