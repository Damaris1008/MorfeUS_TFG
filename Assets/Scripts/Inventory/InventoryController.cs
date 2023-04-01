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

    [Header("Cursor")]
    public Texture2D arrowCursor;
    public Texture2D crosshairCursor;

    [Header("Pop Up Consume")]
    public GameObject buttonToConsume;

    [Header("Scripts")]
    public PopUpsManager popUpsManager;

    private void Start(){
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
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
        } 

        if(Input.GetKey(KeyCode.R)){
            if(selectedSlot>=0 && selectedSlot <= toolbarNumOfSlots-1){
                UseSelectedItem();
            }
        }
    }

    public void ChangeSelectedSlot(int newValue){
        if(selectedSlot != null) {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        InventoryItem inventoryItem = inventorySlots[newValue].GetComponentInChildren<InventoryItem>();
        if(inventoryItem!=null && inventoryItem.item.name == "SWORD"){
            player.isUsingSword = true;
            player.isUsingBow = false;
            Cursor.SetCursor(arrowCursor, Vector2.zero, CursorMode.ForceSoftware);
        }else if(inventoryItem!=null && inventoryItem.item.name == "BOW"){
            player.isUsingSword = false;
            player.isUsingBow = true;
            Cursor.SetCursor(crosshairCursor, Vector2.zero, CursorMode.ForceSoftware);
        }else{
            player.isUsingSword = false;
            player.isUsingBow = false;
            Cursor.SetCursor(arrowCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
        
        selectedSlot = newValue;
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
        if(itemInSlot != null && Time.timeScale == 1.0f){
            Item item = itemInSlot.item;
            if(item.type == ItemType.CONSUMABLE){
                itemInSlot.count--;
                if(itemInSlot.count <= 0){
                    Destroy(itemInSlot.gameObject);
                }else{
                    itemInSlot.RefreshCount();
                }
                player.Heal(item.stats);
                buttonToConsume.SetActive(false);
                popUpsManager.HideItemInfo();
                Debug.Log("Healing of "+item.stats+"pieces of heart");
            }
            return item;
        }else{
            return null;
        }
    }
}
