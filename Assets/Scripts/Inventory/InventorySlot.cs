using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;
    public GameObject buttonToConsume;
    private bool isSelected;
    private PopUpsManager popUpsManager;

    public void Awake(){
        Deselect();
    }

    private void Start(){
        popUpsManager = GameObject.FindWithTag("PopUpsManager").GetComponent<PopUpsManager>();
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

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData){

        InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();
        
        if(inventoryItem!=null){
            // Hover over an item
            popUpsManager.ShowItemInfo(inventoryItem.item);
            // Hover over an selected item
            if(isSelected && inventoryItem!=null && inventoryItem.item.type==ItemType.CONSUMABLE && transform.parent.name=="Toolbar"){
                buttonToConsume.transform.position = eventData.position;
                buttonToConsume.SetActive(true);
            }
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData){

        InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();

        if(inventoryItem!=null){
            // Unhover item
            popUpsManager.HideItemInfo();
            // Unhover selected item
            if(isSelected && inventoryItem!=null && inventoryItem.item.type==ItemType.CONSUMABLE && transform.parent.name=="Toolbar"){
                buttonToConsume.SetActive(false);
            }
        }
    }

}
