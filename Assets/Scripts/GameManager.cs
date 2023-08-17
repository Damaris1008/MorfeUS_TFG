using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static LevelLoader levelLoader;
    private Image brightnessPanel;
    public static bool inventorySaved;
    public static bool playerSaved;

    private void Awake(){
        levelLoader = GameObject.FindWithTag("LevelLoader").GetComponent<LevelLoader>();
        brightnessPanel = GameObject.FindWithTag("BrightnessPanel").GetComponent<Image>();
    }

    private void Start(){
        Time.timeScale = 1;
        
        //Apply audio settings
        AudioListener.volume = PlayerPrefs.GetFloat("volumeLevel");
        GameObject backgroundMusicSource = GameObject.FindWithTag("MusicSource");
        if(backgroundMusicSource!=null){
            AudioSource backgroundMusic = backgroundMusicSource.GetComponent<AudioSource>();
            if(PlayerPrefs.GetInt("backgroundMusic")==0){
                backgroundMusic.mute = true;
            }else{
                backgroundMusic.mute = false;
            }
        }

        //Apply graphics settings (brightness)
        float brightnessSliderValue = PlayerPrefs.GetFloat("Brightness", 1f);
        brightnessPanel.color = new Color(brightnessPanel.color.r, brightnessPanel.color.g, brightnessPanel.color.b, (100-brightnessSliderValue)*180/100/255);


    }

    public static void PlayNewGame()
    {
        SceneManager.LoadSceneAsync(2);
    }

    //Close game
    public static void QuitGame()
    {
        Application.Quit();
    }

    //Go back to main menu
    public static void LeaveGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public static void PauseGame()
    {
        Time.timeScale = 0;
    }
    
    public static void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public static void GameOver(){
        SaveLoad.SeriouslyDeleteAllSaveFiles();
        SceneManager.LoadSceneAsync(1);
    }

    public static void NextScene(){
        int scene = SceneManager.GetActiveScene().buildIndex;
        /*if(scene==3 || scene==4 || scene==5) //Only save on levels
        {
            Save();
        }*/
        levelLoader.LoadNextLevel();
    }

    public static void RefreshScripts(){

        InventoryController inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryController>();
        inventoryManager.Start();

        GameObject player = GameObject.FindWithTag("Player");
        GameObject startPoint = GameObject.FindWithTag("StartPoint");
        player.transform.position = startPoint.transform.position;
        player.GetComponent<Player>().Start();
    }

    /*public static void Save(){
        GameEvents.OnSaveInitiated();
    }*/
}
