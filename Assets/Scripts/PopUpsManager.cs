using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Show/Hide Item Info")]
    [SerializeField] GameObject itemInfo = null;
    [SerializeField] Text itemInfoName = null;
    [SerializeField] Image itemInfoImg = null;
    [SerializeField] Text itemInfoStats = null;

    [Header("Open/Close Death Menu")]
    [SerializeField] GameObject deathMenu = null;
    [SerializeField] Text deathMenuLives = null;


    private void Update(){

        //Open and close inventory
        if(Input.GetKeyDown(KeyCode.I) && !pauseMenu.activeSelf){
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

    public void ShowItemInfo(Item item){
        itemInfoName.text = item.name;
        itemInfoImg.sprite = item.image;
        float stats = item.stats/4;
        if(item.type.ToString() == "TOOL"){
            itemInfoStats.text = "AT. DAMAGE: " + stats.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        }else if(item.type.ToString() =="CONSUMABLE"){
            itemInfoStats.text = "HEALING: " + stats.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + " ❤️";
        }
        itemInfo.SetActive(true);
        
    }

    public void HideItemInfo(){
        itemInfo.SetActive(false);
    }

    public void OpenDeathMenu(int lives){
        GameManager.PauseGame();
        deathMenu.SetActive(true);
        deathMenuLives.text = "x "+(lives/4).ToString();

    }

    public void CloseDeathMenu(){
        GameManager.ResumeGame();
        deathMenu.SetActive(false);
    }
}
