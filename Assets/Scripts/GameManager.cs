using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static LevelLoader levelLoader;
    private static Image brightnessPanel;
    private static GameObject backgroundMusicSource;
    private static BackgroundMusic backgroundMusicScript;

    private void Awake(){
        levelLoader = GameObject.FindWithTag("LevelLoader").GetComponent<LevelLoader>();
        brightnessPanel = GameObject.FindWithTag("BrightnessPanel").GetComponent<Image>();
    }

    private void Start(){
        Time.timeScale = 1;
        
        //Apply audio settings
        AudioListener.volume = PlayerPrefs.GetFloat("volumeLevel");
        backgroundMusicSource = GameObject.FindWithTag("MusicSource");
        if(backgroundMusicSource!=null){
            AudioSource backgroundMusic = backgroundMusicSource.GetComponent<AudioSource>();
            backgroundMusicScript = backgroundMusicSource.GetComponent<BackgroundMusic>();
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
        DeleteAllSavedGameObjects();
        SceneManager.LoadSceneAsync(2);
        backgroundMusicScript.SetBackgroundMusic(2);
    }

    //Close game
    public static void QuitGame()
    {
        Application.Quit();
    }

    //Go back to main menu
    public static void LeaveGame()
    {
        DeleteAllSavedGameObjects();
        Destroy(backgroundMusicSource);
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
        SceneManager.LoadSceneAsync(1);
        backgroundMusicScript.SetBackgroundMusic(1);        
    }

    public static void NextScene(){
        levelLoader = GameObject.FindWithTag("LevelLoader").GetComponent<LevelLoader>();
        levelLoader.LoadNextLevel();

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        backgroundMusicScript.SetBackgroundMusic(nextScene); 
    }

    public static void RefreshScripts(){
        InventoryController inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryController>();
        inventoryManager.Start();

        GameObject player = GameObject.FindWithTag("Player");
        GameObject startPoint = GameObject.FindWithTag("StartPoint");
        player.transform.position = startPoint.transform.position;
        player.GetComponent<Player>().Start();
    }

    public static void DeleteAllSavedGameObjects(){
        GameObject canvas = GameObject.FindWithTag("Canvas");
        GameObject scripts = GameObject.FindWithTag("Scripts");
        GameObject player = GameObject.FindWithTag("Player");
        GameObject arrowExplosion = GameObject.FindWithTag("Punch");
        if(canvas!=null){
            Destroy(canvas);
        }
        if(scripts!=null){
            Destroy(scripts);
        }
        if(player!=null){
            Destroy(player);
        }
        if(arrowExplosion != null){
            Destroy(arrowExplosion);
        }
    }


}
