using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public GameObject menu;

    public GameObject inventory;

    public bool muted = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(inventory.activeSelf){
                OpenOrCloseInventory();
            }else{
                ShowOrHideMenu();
            }    
        }
        if(Input.GetKeyDown(KeyCode.R)){
            OpenOrCloseInventory();
        }
    }

    public void ShowOrHideMenu(){
        if(menu.activeSelf){
            menu.SetActive(false);
            ResumeGame();
        }else{
            menu.SetActive(true);
            PauseGame();
            /*GameObject audioToggle = GameObject.Find("AudioToggle");
            if(AudioListener.volume == 0){
                audioToggle.GetComponent<Toggle>().isOn = true;
            }*/
        }
    }

    public void OpenOrCloseInventory(){
        if(inventory.activeSelf){
            inventory.SetActive(false);
            ResumeGame();
        }else{
            inventory.SetActive(true);
            PauseGame();
            /*GameObject audioToggle = GameObject.Find("AudioToggle");
            if(AudioListener.volume == 0){
                audioToggle.GetComponent<Toggle>().isOn = true;
            }*/
        }
    }

    public static void PauseGame ()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
    }
    
    public static void ResumeGame ()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    public static void QuitGame ()
    {
        Application.Quit();
    }

    public static void RestartGame()
    {
        SceneManager.LoadScene("MainScene");
        AudioListener.pause = false;
    }

    public void MuteAudio(){
        if(muted){
            AudioListener.volume = 1;
            muted = false;
        }else{
            AudioListener.volume = 0;
            muted = true;
        }
    }

}

