using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPersonDialogue : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip dialogueClick;

    void Update(){
        if(Input.GetKeyDown(KeyCode.E)){
            CloseDialogue();
        }
    }

    public void CloseDialogue(){
        GameObject dialogue = gameObject.transform.parent.gameObject;
        audioSource.PlayOneShot(dialogueClick);
        dialogue.SetActive(false);
        GameManager.ResumeGame();
    }
}
