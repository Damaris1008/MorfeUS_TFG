using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject buttonPopUp;

    public bool touchingVendingMachine;

    [Header("Scripts")]
    public PopUpsManager popUpsManager;

    void Update(){
        if(Input.GetKeyDown(KeyCode.E)){
            if(touchingVendingMachine){
                popUpsManager.OpenShop();
            }
            //TO DO: Door
            //With door audio
            //Call method from GameManager to pass the next scene
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        Player player = col.collider.GetComponent<PlayerHitBox>().player;
        if(player!=null){
            buttonPopUp.SetActive(true);
            touchingVendingMachine = true;
        }
    }

    void OnCollisionExit2D(Collision2D col){
        Player player = col.collider.GetComponent<PlayerHitBox>().player;
        if(player!=null){
            buttonPopUp.SetActive(false);
            touchingVendingMachine = false;
        }
    }

}
