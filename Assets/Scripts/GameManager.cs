using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private float winTime;

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
        SceneManager.LoadSceneAsync(1);
    }

    public float WinGame(){
        winTime = TimerHolder.timer;
        return winTime;
    }

    public static void NextScene(){
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(activeScene+1);
    }
}
