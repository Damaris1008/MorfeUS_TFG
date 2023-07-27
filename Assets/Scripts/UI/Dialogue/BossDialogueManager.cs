using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDialogueManager : IntroductionDialogueManager
{
    [Header("Dialogue Interventions")]
    public string[] interventions = new string[4];
    private int counter;

    bool sceneStarting;
    GameObject levelLoader;

    new void Awake(){
        continueButton.onClick.AddListener(NextIntervention);
        audioSource = GetComponent<AudioSource>();
        counter=0;
        levelLoader = GameObject.FindWithTag("LevelLoader");
        textOnScreen.text = interventions[0];
    }

    void Start(){
        sceneStarting = true;
    }

    void Update(){

        //Pause at start
        if(sceneStarting && levelLoader.gameObject.GetComponent<CanvasGroup>().alpha <= 0.2f){
            GameManager.PauseGame();
            transform.GetChild(0).gameObject.SetActive(true);
            sceneStarting = false;
        }
    }

    private void  NextIntervention(){
        //Hide dialog
        audioSource.PlayOneShot(continueSound);
        transform.GetChild(0).gameObject.SetActive(false);

        //Prepare next intervention
        if(counter<interventions.Length-1){
            counter = counter+1;
            textOnScreen.text = interventions[counter];
        }


        //Resume game
        GameManager.ResumeGame();
    }

    public void ShowDialogue(){
        GameManager.PauseGame();
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
