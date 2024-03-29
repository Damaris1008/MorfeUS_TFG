using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopUpsManager : MonoBehaviour
{

    [Header("Open/Close Pause menu")]
    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] GameObject pauseButton = null;
    [SerializeField] GameObject mainMenu = null;
    [SerializeField] GameObject optionsMenu = null;
    [SerializeField] GameObject graphicsSettingsMenu = null;
    [SerializeField] GameObject controlsMenu = null;
    [SerializeField] GameObject controlsMenuContent = null;
    [SerializeField] GameObject soundSettingsMenu = null;

    [Header("Open/Close Inventory")]
    public GameObject inventory;
    public GameObject showInventoryButton;

    [Header("Open/Close Shop")]
    public ShopController shopController;
    public GameObject shopMenu;

    [Header("Show/Hide Item Info")]
    [SerializeField] GameObject itemInfo = null;
    [SerializeField] Text itemInfoName = null;
    [SerializeField] Image itemInfoImg = null;
    [SerializeField] Text itemInfoStats = null;

    [Header("Open/Close Death Menu")]
    [SerializeField] GameObject deathMenu = null;
    [SerializeField] Text deathMenuLives = null;

    [Header("Show/Hide Hacked Screen")]
    [SerializeField] GameObject hackedPanel = null;

    [Header("FPS Counter")]
    float deltaTime = 0.0f;
    [SerializeField] GameObject fpsCounter = null;

    [Header("Coins/Keys Counters")]
    [SerializeField] Text inventoryCoinsCounter = null;
    [SerializeField] Text shopCoinsCounter = null;
    [SerializeField] Text inventoryKeysCounter = null;

    [Header("Power-Up Info")]
    [SerializeField] GameObject powerUpInfo = null;

    [Header("Dead Random Person Dialogue")]
    [SerializeField] GameObject deadRandomPersonDialogue = null;

    [Header("Game Goal")]
    [SerializeField] GameObject gameGoal = null;

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

        if(Input.GetKeyDown(KeyCode.Escape) && shopMenu.activeSelf){
            CloseShop();
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
        controlsMenu.SetActive(false);
        //Disable brightness panel test
        pauseMenu.GetComponent<MainMenuController>().DisableBrightnessPanelTest();
        //Leave the main menu active
        mainMenu.SetActive(true);

    }

    public void ShowControlsMenu(){
        OpenPauseMenu();
        controlsMenu.SetActive(true);
        controlsMenuContent.transform.localPosition = new Vector3(0,0,0);
    }
    
    // INVENTORY

    public void OpenInventory(){
        inventory.SetActive(true);
        pauseButton.SetActive(false);
        showInventoryButton.SetActive(false);
        GameManager.PauseGame();
    }

    public void CloseInventory(){
        inventory.SetActive(false);
        pauseButton.SetActive(true);
        HideItemInfo();
        showInventoryButton.SetActive(true);
        GameManager.ResumeGame();
    }

    // FPS COUNTER

    void OnGUI(){
        int scene = SceneManager.GetActiveScene().buildIndex;
        if(scene == 3 || scene == 4 || scene == 5 || scene == 6){
            float fps = 1.0f / deltaTime;
            fpsCounter.GetComponent<Text>().text = string.Format("{0:0.} fps", fps);
        }
    }

    // ITEM INFO

    public void ShowItemInfo(Item item){
        itemInfoImg.sprite = item.image;
        double stats = item.stats/4.0;
        if(item.type.ToString() == "TOOL"){
            itemInfoName.text = item.name + " (ARMA)";
            itemInfoStats.text = "DAÑO DE ATAQUE: \n" + stats.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + " ❤️";
        }else if(item.type.ToString() =="CONSUMABLE"){
            itemInfoName.text = item.name + " (CONSUMIBLE)";
            itemInfoStats.text = "CURACIÓN: \n" + stats.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + " ❤️";
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

    // HACKED SCREEN
    public IEnumerator HackScreen(){
        //Activate hacked panel
        for(int i = 0; i < hackedPanel.transform.childCount; ++i) {
            hackedPanel.transform.GetChild(i).gameObject.SetActive(true);
        } 
        yield return new WaitForSeconds(3f);
        //Dectivate hacked panel
        for(int i = 0; i < hackedPanel.transform.childCount; ++i) {
            hackedPanel.transform.GetChild(i).gameObject.SetActive(false);
        } 
    }

    public void CloseHackedPanel(){
        for(int i = 0; i < hackedPanel.transform.childCount; ++i) {
            hackedPanel.transform.GetChild(i).gameObject.SetActive(false);
        }  
    }

    // SHOP MENU

    public void OpenShop(){
        shopMenu.SetActive(true);
        GameManager.PauseGame();
    }

    public void CloseShop(){
        shopController.ResetSelectedSlot();
        shopMenu.SetActive(false); 
        GameManager.ResumeGame();
    }

    // COINS AND KEYS COUNTERS

    public void RefreshCoinsCounters(int coins){
        inventoryCoinsCounter.text = "x "+coins.ToString();
        shopCoinsCounter.text = "x "+coins.ToString();
    }

    public void RefreshKeysCounters(int keys){
        inventoryKeysCounter.text = "x "+keys.ToString();
    }

    // POWER UP INFO

    public void ShowPowerUpInfo(float speedIncrease, float damageMultiplier){
        powerUpInfo.GetComponent<Text>().text = "DAÑO DE ATAQUE x "+damageMultiplier.ToString()+" ↑\nVELOCIDAD + "+speedIncrease.ToString()+" ↑";
        powerUpInfo.SetActive(true);
        StartCoroutine("HidePowerUpInfo");
    }

    public IEnumerator HidePowerUpInfo(){
        yield return new WaitForSeconds(3f);
        powerUpInfo.SetActive(false);
    }

    // RANDOM DEAD PERSON DIALOGUE
    public void OpenDeadRandomPersonDialogue(){
        GameManager.PauseGame();
        deadRandomPersonDialogue.SetActive(true);
    }

    public void CloseDeadRandomPersonDialogue(){
        deadRandomPersonDialogue.SetActive(false);
        GameManager.ResumeGame();
    }

    // GAME GOAL

    public IEnumerator ShowGameGoal(){
        yield return new WaitForSeconds(3.0f);
        gameGoal.SetActive(true);
        GameManager.PauseGame();
    }

    public void HideGameGoal(){
        gameGoal.SetActive(false);
        GameManager.ResumeGame();
    }

}
