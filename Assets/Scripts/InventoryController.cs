using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{

    public GameObject inventory;
    public GameObject showInventoryButton;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && inventory.activeSelf){
            CloseInventory();
        }

        if(Input.GetKeyDown(KeyCode.R)){
            if(inventory.activeSelf){
                CloseInventory();
            }else{
                OpenInventory();
            }  
        }
    }

    public void OpenInventory(){
        inventory.SetActive(true);
        showInventoryButton.SetActive(false);
        MenuController.PauseGame();
    }

    public void CloseInventory(){
        inventory.SetActive(false);
        showInventoryButton.SetActive(true);
        MenuController.ResumeGame();
    }
}
