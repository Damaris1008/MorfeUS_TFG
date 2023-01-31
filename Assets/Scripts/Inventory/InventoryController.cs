using System;
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

    [Header("Select Items")]
    public GameObject toolbar;
    int selectedSlot = -1;
    private int toolbarNumOfSlots;

    private void Start(){
        ChangeSelectedSlot(0);
        toolbarNumOfSlots = toolbar.transform.childCount;
    }

    void Update()
    {
        float mouseScrollWheelValue = Input.GetAxis("Mouse ScrollWheel");
        if(mouseScrollWheelValue != null){
            inventorySlots[selectedSlot].Deselect();
            if(mouseScrollWheelValue > 0.0){
                selectedSlot = (selectedSlot + 1)%9;
            }
            else if(mouseScrollWheelValue < 0.0){
                selectedSlot = ((selectedSlot - 1)<0?(toolbarNumOfSlots-1):(selectedSlot-1)%9);
            }
            ChangeSelectedSlot(selectedSlot); 
        }
        

        if(Input.inputString != null){
            
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber && number > 0 && number < 10){
                ChangeSelectedSlot(number - 1);
            }
            
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
    }

    public void ChangeSelectedSlot(int newValue){
        if(selectedSlot >= 0) {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
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

    public Item GetSelectedItem(){
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if(itemInSlot != null){
            Item item = itemInSlot.item;
            return item;
        }else{
            return null;
        }
    }

    public Item UseSelectedItem(){
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if(itemInSlot != null){
            Item item = itemInSlot.item;
            itemInSlot.count--;
            if(itemInSlot.count <= 0){
                Destroy(itemInSlot.gameObject);
            }else{
                itemInSlot.RefreshCount();
            }
            return item;
        }else{
            return null;
        }
    }
}
