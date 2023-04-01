using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Cursor")]
    public Texture2D arrowCursor;
    public Texture2D selectCursor;

    [Header("UI")]
    public Image image;
    public Text countText;

    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    private bool onDrag;

    private PopUpsManager popUpsManager;
    private GameObject buttonToConsume;

    private void Start(){
        popUpsManager = GameObject.FindWithTag("PopUpsManager").GetComponent<PopUpsManager>();
        InventoryController inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryController>();
        buttonToConsume = inventoryManager.buttonToConsume;
    }

    public void InitialiseItem(Item newItem){
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount(){
        countText.text = count.ToString();
        bool textActive = count > 1;
        if(textActive){
            countText.text = count.ToString();
        }else{
            countText.text = "";
        }
        
    }

    // Hover and unhover

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData){

        InventorySlot inventorySlot = transform.parent.gameObject.GetComponent<InventorySlot>();
        // Hover over an item
        popUpsManager.ShowItemInfo(item);
        // Hover over an selected item
        if(inventorySlot.isSelected && item.type==ItemType.CONSUMABLE && transform.parent.transform.parent.name=="Toolbar"
        && Time.timeScale == 1.0f){
            buttonToConsume.transform.position = eventData.position;
            buttonToConsume.SetActive(true);
        }
        
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData){
        if(!onDrag){
            InventorySlot inventorySlot = transform.parent.gameObject.GetComponent<InventorySlot>();
            // Unhover item
            popUpsManager.HideItemInfo();
            // Unhover selected item
            if(inventorySlot.isSelected && item.type==ItemType.CONSUMABLE && transform.parent.transform.parent.name=="Toolbar"){
                buttonToConsume.SetActive(false);
            }
        }

    }

    // Drag and drop
    public void OnBeginDrag(PointerEventData eventData){
        onDrag = true;
        buttonToConsume.SetActive(false);
        Cursor.SetCursor(selectCursor, Vector2.zero, CursorMode.ForceSoftware);
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData){
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData){
        onDrag = false;
        Cursor.SetCursor(arrowCursor, Vector2.zero, CursorMode.ForceSoftware);
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }

}
