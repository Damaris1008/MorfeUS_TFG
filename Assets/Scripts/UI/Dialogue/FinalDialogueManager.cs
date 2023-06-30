using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDialogueManager : DialogueManager
{

    public enum Talker{
        MUM,
        ME
    }

    [Header("Dialogue Interventions")]
    public string[] interventions = new string[6];
    public Talker[] talkers = new Talker[6];
    private int counter;

    [Header("Dialogue Portraits")]
    public GameObject mumImg;
    public GameObject myImg;

    [Header("Scene Changes")]
    public GameObject mum;
    public GameObject sleptPlayer;
    public GameObject awakePlayer;
    public GameObject lookingPlayer;


    void Awake(){
        continueButton.onClick.AddListener(SkipTypingText);
        audioSource = GetComponent<AudioSource>();
        counter=0;
    }

    void Start(){
        ChangeDialogueIntervention();
    }

    private void SkipTypingText(){
        audioSource.PlayOneShot(continueSound);
        if(textOnScreen.text != dialogueText.text){
            StopCoroutine("DisplayText");
            textOnScreen.text = dialogueText.text;
        }else{
            ChangeDialogueIntervention();
        }
    }

    private void ChangeDialogueIntervention(){

        if(counter>=interventions.Length){
            GameManager.NextScene();
            return;
        }

        //Scene changes
        if(counter==1){
                sleptPlayer.SetActive(false);
                awakePlayer.SetActive(true);
        }else if(counter==3){
                awakePlayer.SetActive(false);
                lookingPlayer.SetActive(true);
                mum.transform.position = new Vector3(-7.853f, mum.transform.position.y, mum.transform.position.z);
        }

        dialogueText.text = interventions[counter];
        Talker talker = talkers[counter];
        if(talker==Talker.MUM){
            mumImg.SetActive(true);
            myImg.SetActive(false);
        }else{
            mumImg.SetActive(false);
            myImg.SetActive(true);
        }
        counter++;
        StartCoroutine("DisplayText");
    }

}
