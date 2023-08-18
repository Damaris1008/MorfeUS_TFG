using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tuples
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class InventoryController : MonoBehaviour
{

    private Player player;

    [Header("Add Items")]
    public InventorySlot[] inventorySlots;
    public List<GameObject> inventorySlotsGameObjects;
    public GameObject inventoryItemPrefab;

    [Header("Select Items")]
    public GameObject mainInventory;
    public GameObject toolbar;
    int selectedSlot;
    private int toolbarNumOfSlots;

    [Header("All Items")]
    public Item[] itemsList;


    [Header("Pop Up Consume")]
    public GameObject buttonToConsume;

    [Header("Scripts")]
    public PopUpsManager popUpsManager;

    [Header("Scroll Views Opened")]
    public GameObject shopMenu;
    public GameObject controlsMenu;

    public void Awake(){
        for(int i=0; i<toolbar.transform.childCount; i++){
            inventorySlotsGameObjects.Add(toolbar.transform.GetChild(i).gameObject);
        }
        for(int i=0; i<mainInventory.transform.childCount; i++){
            inventorySlotsGameObjects.Add(mainInventory.transform.GetChild(i).gameObject);
        }
    }

    public void Start(){
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
        

        /*if(Input.inputString != null){
            
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber && number > 0 && number < 10){
                ChangeSelectedSlot(number - 1);
            }
        } */
        
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            ChangeSelectedSlot(0);
        }else if(Input.GetKeyDown(KeyCode.Alpha2)){
            ChangeSelectedSlot(1);
        }else if(Input.GetKeyDown(KeyCode.Alpha3)){
            ChangeSelectedSlot(2);
        }else if(Input.GetKeyDown(KeyCode.Alpha4)){
            ChangeSelectedSlot(3);
        }else if(Input.GetKeyDown(KeyCode.Alpha5)){
            ChangeSelectedSlot(4);
        }else if(Input.GetKeyDown(KeyCode.Alpha6)){
            ChangeSelectedSlot(5);
        }else if(Input.GetKeyDown(KeyCode.Alpha7)){
            ChangeSelectedSlot(6);
        }else if(Input.GetKeyDown(KeyCode.Alpha8)){
            ChangeSelectedSlot(7);
        }else if(Input.GetKeyDown(KeyCode.Alpha9)){
            ChangeSelectedSlot(8);
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
        if(inventoryItem!=null && inventoryItem.item.id == 1){
            player.isUsingSword = true;
            player.isUsingBow = false;
        }else if(inventoryItem!=null && inventoryItem.item.id == 0){
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

    public void SpawnAmountOfItemInSlot(Item item, int slotNumber, int amount){
        if(inventorySlots[slotNumber].GetComponentInChildren<InventoryItem>() == null){
            SpawnNewItem(item,inventorySlots[slotNumber]);
            inventorySlots[slotNumber].GetComponentInChildren<InventoryItem>().count = amount;
            inventorySlots[slotNumber].GetComponentInChildren<InventoryItem>().RefreshCount();
        }
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
                itemInSlot.count--;
                if(itemInSlot.count <= 0){
                    Destroy(itemInSlot.gameObject);
                    popUpsManager.HideItemInfo();
                }else{
                    itemInSlot.RefreshCount();
                }
                player.GetComponent<AudioSource>().PlayOneShot(item.sound);
                player.Heal(item.stats);

                buttonToConsume.SetActive(false);
            }
            return item;
        }else{
            return null;
        }
    }

}
