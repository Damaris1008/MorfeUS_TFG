using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryController inventoryManager;
    public Item[] itemsToPickup;

    public void PickupItem(int id){
        bool result = inventoryManager.AddItem(itemsToPickup[id]);
        if(result == true){
            Debug.Log("Item added");
        }else{
            Debug.Log("ITEM NOT ADDED!");
        }
    }
}
