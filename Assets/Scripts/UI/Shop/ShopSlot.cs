using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{

    public Item item;
    private Image image;
    public Color selectedColor, notSelectedColor;
    public bool isSelected;

    public void Awake(){
        image = GetComponent<Image>();
    }

    public void Select(){
        image.color = selectedColor;
        isSelected = true;
    }

    public void Deselect(){
        image.color = notSelectedColor;
        isSelected = false;
    }
}
