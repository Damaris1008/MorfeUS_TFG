using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("Cursor")]
    public Texture2D cursorArrow;

    private void Awake(){
        Time.timeScale = 1;
        AudioListener.volume = PlayerPrefs.GetFloat("volumeLevel", 0.5f);
    }

    private void Start(){
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
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
