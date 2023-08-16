using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private float winTime;
    public static LevelLoader levelLoader;

    private void Awake(){
        levelLoader = GameObject.FindWithTag("LevelLoader").GetComponent<LevelLoader>();
    }

    private void Start(){
        Time.timeScale = 1;
        
        //Apply audio settings
        AudioListener.volume = PlayerPrefs.GetFloat("volumeLevel");
        GameObject backgroundMusicSource = GameObject.FindWithTag("MusicSource");
        if(backgroundMusicSource!=null){
            AudioSource backgroundMusic = backgroundMusicSource.GetComponent<AudioSource>();
            if(PlayerPrefs.GetInt("backgroundMusic")==1 || PlayerPrefs.GetInt("backgroundMusic")==null){
                backgroundMusic.mute = false;
            }else{
                backgroundMusic.mute = true;
            }
        }

    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.O)){
            Save();
        }
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

    public float WinGame(){
        winTime = TimerHolder.timer;
        return winTime;
    }

    public static void NextScene(){
        int scene = SceneManager.GetActiveScene().buildIndex;
        if(scene==3 || scene==4 || scene==5) //Only save on levels
        {
            Save();
        }
        levelLoader.LoadNextLevel();
    }

    public static void Save(){
        GameEvents.OnSaveInitiated();
    }
}
