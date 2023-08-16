using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDialogueManager : IntroductionDialogueManager
{
    [Header("Dialogue Interventions")]
    public string[] interventions = new string[4];

    bool sceneStarting;
    GameObject levelLoader;

    new void Awake(){
        continueButton.onClick.AddListener(CloseDialogue);
        audioSource = GetComponent<AudioSource>();
        levelLoader = GameObject.FindWithTag("LevelLoader");
        textOnScreen.text = interventions[0];
    }

    public void Start(){
        sceneStarting = true;
    }

    void Update(){
        //Pause at start
        if(sceneStarting && levelLoader.gameObject.GetComponent<CanvasGroup>().alpha <= 0f){
            ShowDialogue(0);
            sceneStarting = false;
        }
    }

    private void CloseDialogue(){
        audioSource.PlayOneShot(continueSound);
        transform.GetChild(0).gameObject.SetActive(false);
        //Resume game
        GameManager.ResumeGame();
    }

    public void ShowDialogue(int intervention){
        GameManager.PauseGame();
        textOnScreen.text = interventions[intervention];
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
