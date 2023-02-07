using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public Text itemName;
    public Image itemImg;
    public Text itemStats;

    public void SetUp(string name, Sprite itemSprite, string itemType, float stats){
        itemName.text = name;
        itemImg.sprite = itemSprite;
        itemStats.text = stats.ToString();
        if(itemType=="TOOL"){
            itemStats.text = "DAMAGE: " + stats;
        }else if(itemType=="CONSUMABLE"){
            itemStats.text = "HEALTH: " + stats + " ❤️";
        }
    }
}
