using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;
    public bool isSelected;

    public void Awake(){
        Deselect();
    }

    public void Select(){
        image.color = selectedColor;
        isSelected = true;
    }

    public void Deselect(){
        image.color = notSelectedColor;
        isSelected = false;
    }

    public void OnDrop(PointerEventData eventData){
        if(transform.childCount == 0){
            GameObject dropped = eventData.pointerDrag;
            InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }else{
            //Dropped item
            GameObject dropped = eventData.pointerDrag;
            InventoryItem droppedInventoryItem = dropped.GetComponent<InventoryItem>();
            //Item on that slot
            GameObject itemInSlot = transform.GetChild(0).gameObject;
            InventoryItem inSlotInventoryItem = itemInSlot.GetComponent<InventoryItem>();
            itemInSlot.transform.SetParent(droppedInventoryItem.parentAfterDrag);
            //Dropped item
            droppedInventoryItem.parentAfterDrag = transform;
        }
    }

}
