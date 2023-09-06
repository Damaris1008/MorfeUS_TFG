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
    public int id;
    public ItemType type;
    public string name;
    public int stats = 0; //If type is TOOL, stats will be the damage. If it is CONSUMABLE, stats will be the healing
    public int cost = 1;

    [Header("Sound")]
    public AudioClip sound;

    public string GetItemName(){
        return name;
    }

    public ItemType GetItemType(){
        return type;
    }

    public float GetItemStats(){
        return stats;
    }

}

public enum ItemType{
    TOOL,
    CONSUMABLE
}