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
        }
    }

}