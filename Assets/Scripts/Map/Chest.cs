using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    private Player player;
    private Animator animator;
    private AudioSource audioSource;
    public Animator popUpAnimator;
    

    [Header("Content")]
    public Item item;
    public int typeOfChest; //0: coin, 1: key, 2: item, 3: power-up
    public int amount;

    [Header("Pop-Up")]
    public GameObject popUp;
    public Text amountText;
    public GameObject rewardImg;

    public bool opened = false;

    void Awake(){
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    void Start(){
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void Open(){
        if(!opened){
            opened=true;
            player.SpendKeys(1);
            audioSource.Play();
            StartCoroutine("OpenChest");
        }
    }

    IEnumerator OpenChest(){
        yield return new WaitForSeconds(0.08f);
        animator.SetTrigger("Open");

        if(typeOfChest<0 || typeOfChest>5){
            Debug.Log("Trying to open an incorrect type of chest! (0: coins, 1: keys, 2: item, 3: power-up)");
            StopCoroutine("OpenChest");
        }

        StartCoroutine("PopUp");
    }

    IEnumerator PopUp(){

        //Activate
        yield return new WaitForSeconds(0.7f);

        // Animation
        rewardImg.SetActive(true);
        if(typeOfChest == 0 || typeOfChest == 1) // Resource Chest
        {
            amountText.text = "+"+amount;
            popUpAnimator.SetTrigger("RaiseText");

            //Deactivate
            yield return new WaitForSeconds(1.2f);
            amountText.text = "";
        }
        else // Special Chest
        {
            popUpAnimator.SetTrigger("RaiseText");
            //Deactivate
            yield return new WaitForSeconds(1.8f);
        }

        // Reward
        switch(typeOfChest)
        {
            case 0: //Coins
                player.WinCoins(amount);
                break;
            case 1: //Keys
                player.WinKeys(amount);
                break;
            case 2: //Item
                player.WinItem(item);
                break;
            case 3: //Power-up
                break;
        }

        rewardImg.SetActive(false);
        popUp.SetActive(false);
        
    }

}
