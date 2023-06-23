using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{

    public Player player;

    [Header("Shop Items")]
    public List<ShopSlot> shopSlots;
    public Transform content;

    [Header("Select Items")]
    int selectedSlot;
    private int toolbarNumOfSlots;

    [Header("Item info")]
    public Text itemName;
    public Image itemImg;
    public Text itemStats;
    public Text itemCost;

    [Header("Show/Hide Item Info")]
    public GameObject itemInfoContent;
    public GameObject itemCostContent;
    public GameObject buyButton;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonClickAudio;
    public AudioClip buyAudio;

    [Header("Scripts")]
    public InventoryController inventoryController;

    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i<content.childCount; i++){
            shopSlots.Add(content.GetChild(i).GetComponent<ShopSlot>());
        }
        selectedSlot = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if(selectedSlot == -1){
            itemInfoContent.SetActive(false);
            itemCostContent.SetActive(false);
        }else{
            itemInfoContent.SetActive(true);
            itemCostContent.SetActive(true);
        }
    }

    public void ChangeSelectedSlot(int newValue){

        //Audio
        audioSource.PlayOneShot(buttonClickAudio);

        //Change the selected slot
        if(selectedSlot!=-1){
            shopSlots[selectedSlot].Deselect();
        }
        shopSlots[newValue].Select();        
        selectedSlot = newValue;
        
        //Show info of the item
        Item item = shopSlots[newValue].item;
        itemName.text = item.name;
        itemImg.sprite = item.image;
        double convertedStats = (double)item.stats/4.0;
        itemStats.text = convertedStats.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        itemCost.text = item.cost.ToString();

        RefreshCostPanel();
    }

    public void ResetSelectedSlot(){
        if(selectedSlot!=-1){
            shopSlots[selectedSlot].Deselect();
        }
        selectedSlot = -1;
    }

    public void buyItem(){
        Item item = shopSlots[selectedSlot].item;
        if(item.cost <= player.coins){
            audioSource.PlayOneShot(buyAudio);
            player.SpendCoins(item.cost);
            inventoryController.AddItem(item);
            RefreshCostPanel();
        }else{
            Debug.Log("No coins to spend!");
        }

    }

    public void RefreshCostPanel(){
        Item item = shopSlots[selectedSlot].item;
        //Can the player buy it
        if(item.cost > player.coins){
            itemCost.color = Color.red;
            buyButton.SetActive(false);
        }else{
            itemCost.color = Color.black;
            buyButton.SetActive(true);
        }
    }
}
