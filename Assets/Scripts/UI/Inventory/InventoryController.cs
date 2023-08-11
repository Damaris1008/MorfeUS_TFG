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
    public GameObject inventoryItemPrefab;

    [Header("Select Items")]
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

    private void Start(){
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        ChangeSelectedSlot(0);
        toolbarNumOfSlots = toolbar.transform.childCount;

        //Save and load
        GameEvents.SaveInitiated += Save;
        Load();
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
        InventorySlot slot = inventorySlots[slotNumber];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if(itemInSlot == null){
            SpawnNewItem(item,slot);
            itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            itemInSlot.count = amount;
            itemInSlot.RefreshCount();
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

    public Item FindItemById(int itemId){
        Item item = new Item();
        for(int i=0; i<itemsList.Length; i++){
            if(itemsList[i].id == itemId){
                item = itemsList[i];
            }
        }
        return item;
    }

    public List<Tuple<int,int>> ToDataList(){ //First int of tuple: itemId, Second int of tuple: itemAmount
        List<Tuple<int,int>> dataList = new List<Tuple<int,int>>();

        for(int i=0; i<inventorySlots.Length;i++){
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot == null){
                dataList.Add(Tuple.Create(-1, -1));
            }else{
                dataList.Add(Tuple.Create(itemInSlot.item.id, itemInSlot.count));
            }
        }
        return dataList;
    }

    public void FillWithDataList(List<Tuple<int,int>> dataList){
        for(int i=0; i<inventorySlots.Length;i++){
            Tuple<int,int> tuple = dataList[i];
            int itemId = tuple.Item1;
            int itemAmount = tuple.Item2;
            
            if(itemAmount>0){
                Item item = FindItemById(itemId);
                SpawnAmountOfItemInSlot(item, i, itemAmount);
            }

        }
    }

    void Save(){
        List<Tuple<int,int>> dataList = ToDataList();
        SaveLoad.Save<List<Tuple<int,int>>>(dataList, "Inventory");
    }

    void Load(){
        if(SaveLoad.SaveExists("Inventory")){
            List<Tuple<int,int>> dataList = SaveLoad.Load<List<Tuple<int,int>>>("Inventory");
            FillWithDataList(dataList);
        }
    }

}
