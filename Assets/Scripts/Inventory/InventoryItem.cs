using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Cursor")]
    public Texture2D cursorArrow;
    public Texture2D selectCursor;

    [Header("UI")]
    public Image image;
    public Text countText;

    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

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

    // Drag and drop
    public void OnBeginDrag(PointerEventData eventData){
        Cursor.SetCursor(selectCursor, Vector2.zero, CursorMode.ForceSoftware);
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData){
        Cursor.SetCursor(selectCursor, Vector2.zero, CursorMode.ForceSoftware);
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData){
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }

}
