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

    [Header("FPS Counter")]
    float deltaTime = 0.0f;
    [SerializeField] GameObject fpsCounter = null;



    private void Update(){

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        //Show and hide FPS counter
        if(Input.GetKeyDown(KeyCode.F1)){
            if(fpsCounter.activeSelf){
                fpsCounter.SetActive(false);
            }else{
                fpsCounter.SetActive(true);
            }
        }

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

    // PAUSE MENU

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
    
    // INVENTORY

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

    // FPS COUNTER

    void OnGUI(){
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        fpsCounter.GetComponent<Text>().text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
    }

    // ITEM INFO

    public void ShowItemInfo(Item item){
        itemInfoName.text = item.name + " (" + item.type.ToString() + ")";
        itemInfoImg.sprite = item.image;
        double stats = item.stats/4.0;
        if(item.type.ToString() == "TOOL"){
            itemInfoStats.text = "AT. DAMAGE: \n" + stats.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + " ❤️";
        }else if(item.type.ToString() =="CONSUMABLE"){
            itemInfoStats.text = "HEALING: \n" + stats.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + " ❤️";
        }
        itemInfo.SetActive(true);
        
    }

    public void HideItemInfo(){
        itemInfo.SetActive(false);
    }

    // DEATH MENU

    public void OpenDeathMenu(int lives){
        GameManager.PauseGame();
        deathMenu.SetActive(true);
        deathMenuLives.text = "x "+(lives/4).ToString();

    }

    public void CloseDeathMenu(){
        GameManager.ResumeGame();
        deathMenu.SetActive(false);
    }

    // SHOP MENU

    //When opening it, update selected slot to none (-1)
}
