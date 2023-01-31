using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryController inventoryManager;
    public Item[] itemsToPickup;

    public void PickupItem(int id){
        bool result = inventoryManager.AddItem(itemsToPickup[id]);
        /*if(result == true){
            Debug.Log("Item added");
        }else{
            Debug.Log("ITEM NOT ADDED!");
        }*/
    }

    public void GetSelectedItem(){
        Item receivedItem = inventoryManager.GetSelectedItem();
        if(receivedItem != null){
            Debug.Log("Received Item: "+receivedItem);
        }else{
            Debug.Log("No item received!");
        }
    }

    public void UseSelectedItem(){
        Item usedItem = inventoryManager.UseSelectedItem();
        if(usedItem != null){
            Debug.Log("Used Item: "+usedItem);
        }else{
            Debug.Log("No item used!");
        }
    }
}
