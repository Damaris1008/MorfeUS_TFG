using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDialogueManager : IntroductionDialogueManager
{
    [Header("Dialogue Interventions")]
    public string[] interventions = new string[4];
    public int counter;

    bool sceneStarting;
    GameObject levelLoader;

    new void Awake(){
        continueButton.onClick.AddListener(CloseDialogue);
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

    private void  CloseDialogue(){
        audioSource.PlayOneShot(continueSound);
        transform.GetChild(0).gameObject.SetActive(false);

        //Resume game
        GameManager.ResumeGame();
    }

    public void ShowDialogue(){
        GameManager.PauseGame();
        
        //Next intervention
        if(counter<interventions.Length-1){
            counter = counter+1;
            textOnScreen.text = interventions[counter];
        }

        transform.GetChild(0).gameObject.SetActive(true);
    }
}
