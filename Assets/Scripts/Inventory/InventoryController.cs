using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{

    private Player player;

    [Header("Add Items")]
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    [Header("Select Items")]
    public GameObject toolbar;
    int selectedSlot;
    private int toolbarNumOfSlots;

    [Header("Pop Up Consume")]
    public GameObject buttonToConsume;

    [Header("Scripts")]
    public PopUpsManager popUpsManager;

    [Header("Scroll Views Opened")]
    public GameObject shopMenu;
    public GameObject controlsMenu;

    private void Start(){
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        ChangeSelectedSlot(0);
        toolbarNumOfSlots = toolbar.transform.childCount;
    }

    void Update()
    {

        float mouseScrollWheelValue = Input.GetAxis("Mouse ScrollWheel");
        if(mouseScrollWheelValue != 0.0f && shopMenu.activeSelf == false && controlsMenu.activeSelf == false){
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
        } 

        if(Input.GetKeyDown(KeyCode.R)){
            UseSelectedItem();
        }

    }

    public void ChangeSelectedSlot(int newValue){
        inventorySlots[selectedSlot].Deselect();
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        RefreshItemSelected();
    }

    public void RefreshItemSelected(){
        InventoryItem inventoryItem = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();
        if(inventoryItem!=null && inventoryItem.item.name == "SWORD"){
            player.isUsingSword = true;
            player.isUsingBow = false;
        }else if(inventoryItem!=null && inventoryItem.item.name == "BOW"){
            player.isUsingSword = false;
            player.isUsingBow = true;
        }else{
            player.isUsingSword = false;
            player.isUsingBow = false;
        }
    }



    public bool AddItem(Item item){

        //Check if any slot has the same item with count lower than max
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
        RefreshItemSelected();
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
        if(itemInSlot != null && Time.timeScale == 1.0f){
            Item item = itemInSlot.item;
            if(item.type == ItemType.CONSUMABLE && player.currentHealth != player.maxHealth){
                Debug.Log("Used " + item.name + ". Healing "+ (item.stats > player.maxHealth - player.currentHealth ? player.maxHealth - player.currentHealth : item.stats) +" pieces of heart!");
                itemInSlot.count--;
                if(itemInSlot.count <= 0){
                    Destroy(itemInSlot.gameObject);
                    popUpsManager.HideItemInfo();
                }else{
                    itemInSlot.RefreshCount();
                }
                player.Heal(item.stats);
                buttonToConsume.SetActive(false);
            }
            return item;
        }else{
            return null;
        }
    }
}
