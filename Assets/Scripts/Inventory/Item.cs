using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{

    [Header("UI")]
    public bool stackable = true;
    public int maxStack = 12;
    public Sprite image; 

    [Header("Item Info")]
    public ItemType type;
    public string name;
    public string description;
    public float health = 0;
    public float damage = 0;

}

public enum ItemType{
    TOOL,
    CONSUMABLE
}