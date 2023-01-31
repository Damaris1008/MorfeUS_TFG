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

    public static void GameOver(){
        SceneManager.LoadSceneAsync(1);
    }
}
