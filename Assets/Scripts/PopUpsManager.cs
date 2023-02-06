using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpsManager : MonoBehaviour
{

    [Header("Open/Close Pause menu")]
    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] GameObject pauseButton = null;
    [SerializeField] GameObject mainMenu = null;
    [SerializeField] GameObject optionsMenu = null;
    [SerializeField] GameObject graphicsSettingsMenu = null;
    [SerializeField] GameObject soundSettingsMenu = null;

    [Header("Open/Close Inventory")]
    public GameObject inventory;
    public GameObject showInventoryButton;


    private void Update(){

        //Open and close inventory
        if(Input.GetKeyDown(KeyCode.R) && !pauseMenu.activeSelf){
            if(inventory.activeSelf){
                CloseInventory();
            }else{
                OpenInventory();
            } 
            return; 
        }
        if(Input.GetKeyDown(KeyCode.Escape) && inventory.activeSelf){
            CloseInventory();
            return;
        }

        //Open and close pause menu
        if(Input.GetKeyDown(KeyCode.Escape) && !inventory.activeSelf){    
            if(pauseMenu.activeSelf){
                ClosePauseMenu();
            }else{
                OpenPauseMenu();
            }
            return;
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
        //Close the options menu if opened
        optionsMenu.SetActive(false);
        graphicsSettingsMenu.SetActive(false);
        soundSettingsMenu.SetActive(false);
        //Leave the main menu active
        mainMenu.SetActive(true);

    }

    public void OpenInventory(){
        inventory.SetActive(true);
        showInventoryButton.SetActive(false);
        GameManager.PauseGame();
    }

    public void CloseInventory(){
        inventory.SetActive(false);
        showInventoryButton.SetActive(true);
        GameManager.ResumeGame();
    }
}
