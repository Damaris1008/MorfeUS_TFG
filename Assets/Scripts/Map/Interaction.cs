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
    public bool touchingDeadRandomPerson;

    [Header("Scripts")]
    public PopUpsManager popUpsManager;
    public Chest chest;
    public Door door;

    void Update(){
        if(Input.GetKeyDown(KeyCode.E)){
            if(touchingDeadRandomPerson){
                popUpsManager.OpenDeadRandomPersonDialogue();
            }else if(touchingVendingMachine){
                popUpsManager.OpenShop();
            }else if(touchingChest){
                chest.Open();
            }else if(touchingDoor){
                door.OpenDoor();
            }
            
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.collider.tag == "Player"){
            Player player = col.collider.GetComponent<PlayerHitBox>().player;
            if(gameObject.CompareTag("DeadRandomPerson")){
                buttonPopUp.SetActive(true);
                touchingDeadRandomPerson = true;
            }else if(gameObject.CompareTag("VendingMachine")){
                buttonPopUp.SetActive(true);
                touchingVendingMachine = true;
            }else if(gameObject.CompareTag("Chest") && !chest.opened){
                if(player.keys>=1 || chest.typeOfChest == 2 || chest.typeOfChest == 3){
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
        if(col.collider.tag == "Player" && chest!=null && chest.opened){
            buttonPopUp.SetActive(false);
        }
    }

    void OnCollisionExit2D(Collision2D col){
        if(col.collider.tag == "Player"){
            Player player = col.collider.GetComponent<PlayerHitBox>().player;
            buttonPopUp.SetActive(false);
            touchingVendingMachine = false;
            touchingChest = false;
            touchingDoor = false;
            touchingDeadRandomPerson = false;
            if(chest!=null){
                missingKey.SetActive(false);
            }
        }
    }

}
