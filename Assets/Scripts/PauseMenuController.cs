using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Header("Pause menu")]
    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] GameObject pauseButton = null;

    [Header("Main Inventory")]
    [SerializeField] GameObject inventory = null;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !inventory.activeSelf){
            if(pauseMenu.activeSelf){
                ClosePauseMenu();
            }else{
                OpenPauseMenu();
            }
            
        }
    }

    public void OpenPauseMenu(){
        GameManager.PauseGame();
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
    }

    
    public void ClosePauseMenu(){
        GameManager.ResumeGame();
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
    }
}
