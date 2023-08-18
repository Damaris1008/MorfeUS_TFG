using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossDialogueManager : IntroductionDialogueManager
{
    [Header("Dialogue Interventions")]
    public string[] interventions = new string[4];

    bool sceneStarting;
    public GameObject levelLoader;

    public void Start(){
        levelLoader = GameObject.FindWithTag("LevelLoader");
        if(SceneManager.GetActiveScene().buildIndex == 6){
            continueButton.onClick.AddListener(CloseDialogue);
            audioSource = GetComponent<AudioSource>();
            textOnScreen.text = interventions[0];
            sceneStarting = true;
        }
    }

    void Update(){
        if(SceneManager.GetActiveScene().buildIndex == 6){
            //Pause at start
            if(sceneStarting && levelLoader.gameObject.GetComponent<CanvasGroup>().alpha <= 0f){
                ShowDialogue(0);
                sceneStarting = false;
            }
        }
        if(Input.GetKeyDown(KeyCode.E)){
            CloseDialogue();
        }
    }

    private void CloseDialogue(){
        if(transform.GetChild(0).gameObject.activeSelf){
            audioSource.PlayOneShot(continueSound);
            transform.GetChild(0).gameObject.SetActive(false);
            GameManager.ResumeGame();
        }
    }

    public void ShowDialogue(int intervention){
        GameManager.PauseGame();
        textOnScreen.text = interventions[intervention];
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
