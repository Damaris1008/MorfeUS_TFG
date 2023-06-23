using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject buttonPopUp;
    public GameObject missingKey;

    public bool touchingVendingMachine;
    public bool touchingChest;
    public bool touchingDoor;

    [Header("Scripts")]
    public PopUpsManager popUpsManager;
    public Chest chest;

    void Update(){
        if(Input.GetKeyDown(KeyCode.E)){
            if(touchingVendingMachine){
                popUpsManager.OpenShop();
            }else if(touchingChest){
                chest.Open();
            }else if(touchingDoor){
                //Door audio
                //Call method from GameManager to pass the next scene
            }
            
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        Player player = col.collider.GetComponent<PlayerHitBox>().player;
        if(player!=null){
            if(gameObject.CompareTag("VendingMachine")){
                buttonPopUp.SetActive(true);
                touchingVendingMachine = true;
            }else if(gameObject.CompareTag("Chest") && !chest.opened){
                if(player.keys>=1){
                    buttonPopUp.SetActive(true);
                    touchingChest = true;
                }else{
                    missingKey.SetActive(true);
                }
            }else if(gameObject.CompareTag("Door")){
                buttonPopUp.SetActive(true);
                touchingDoor = true;
            }
        }
    }

    void OnCollisionStay2D(Collision2D col){
        // If its a chest and it is already opened
        if(chest!=null && chest.opened){
            buttonPopUp.SetActive(false);
        }
    }

    void OnCollisionExit2D(Collision2D col){
        Player player = col.collider.GetComponent<PlayerHitBox>().player;
        if(player!=null){
            buttonPopUp.SetActive(false);
            touchingVendingMachine = false;
            touchingChest = false;
            touchingDoor = false;
            if(chest!=null){
                missingKey.SetActive(false);
            }
        }
    }

}
