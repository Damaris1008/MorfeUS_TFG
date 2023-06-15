using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    private Player player;
    private Animator animator;
    public Animator popUpAnimator;

    [Header("Content")]
    public bool isKeyChest; //True: keyChest, False: coinChest
    public int amount;

    [Header("Pop-Up")]
    public GameObject popUp;
    public Text amountText;
    public GameObject coinImg;
    public GameObject keyImg;

    
    public bool opened = false;

    void Awake(){
        animator = GetComponent<Animator>();
    }
    void Start(){
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void Open(){

        if(!opened){
            animator.SetTrigger("Open");
            StartCoroutine("ActivatePopUp");
        }
        opened=true;
    }

    IEnumerator ActivatePopUp(){

        //Activate
        yield return new WaitForSeconds(0.7f);
        amountText.text = "+"+amount;
        if(isKeyChest){
            keyImg.SetActive(true);
            player.WinKeys(amount);
        }else{
            coinImg.SetActive(true);
            player.WinCoins(amount);
        }
        popUpAnimator.SetTrigger("RaiseText");

        //Deactivate
        yield return new WaitForSeconds(1.2f);
        amountText.text = "";
        coinImg.SetActive(false);
        keyImg.SetActive(false);
        popUp.SetActive(false);

    }

}
