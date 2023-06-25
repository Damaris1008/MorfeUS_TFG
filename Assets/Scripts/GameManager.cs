using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    [Header("Map")]
    public GameObject finalWall;


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
        //Final wall
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0){
            RaiseFinalWall();
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

    public void RaiseFinalWall(){
        finalWall.SetActive(false);
    }
}
