using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{

    [Header("Open/Close Inventory")]
    public GameObject inventory;
    public GameObject showInventoryButton;

    [Header("Add Items")]
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;


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

    public bool AddItem(Item item){

        //Check if any slot has the sqame item with count lower than max
        for(int i = 0; i < inventorySlots.Length; i++){
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot != null &&
            itemInSlot.item == item &&
            itemInSlot.item.stackable == true &&
            itemInSlot.count < item.maxStack){
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        //Find any empty slot
        for(int i = 0; i < inventorySlots.Length; i++){
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot == null){
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot){
        GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
}
