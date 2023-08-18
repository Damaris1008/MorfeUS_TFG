using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPersonDialogue : MonoBehaviour
{
    void Update(){
        if(Input.GetKeyDown(KeyCode.E)){
            CloseDialogue();
        }
    }

    public void CloseDialogue(){
        GameObject dialogue = gameObject.transform.parent.gameObject;
        dialogue.SetActive(false);
        GameManager.ResumeGame();
    }
}
