using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static void PlayNewGame()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    public static void PauseGame()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
    }
    
    public static void ResumeGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    public static void GameOver(){
        SceneManager.LoadSceneAsync(1);
    }
}
