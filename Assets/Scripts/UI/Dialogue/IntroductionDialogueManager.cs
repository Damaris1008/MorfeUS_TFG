using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroductionDialogueManager : MonoBehaviour
{

    [Header("Params")]
    private float typingSpeed = 0.04f;

    [Header("Audio")]
    public AudioClip continueSound;
    public AudioClip typingSound;
    public AudioSource audioSource;
    
    [Header("UI Dialogue")]
    public Text dialogueText;
    public Text textOnScreen;
    public Button continueButton;

    void Awake(){
        continueButton.onClick.AddListener(SkipTypingText);
        audioSource = GetComponent<AudioSource>();
    }

    void Start(){
        StartCoroutine("DisplayText");
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            SkipTypingText();
        }
    }

    private void SkipTypingText(){
        audioSource.Stop();
        audioSource.PlayOneShot(continueSound);
        if(textOnScreen.text != dialogueText.text){
            StopCoroutine("DisplayText");
            textOnScreen.text = dialogueText.text;
        }else{
            GameManager.NextScene(); 
        }
    }

    public IEnumerator DisplayText(){
        audioSource.clip = typingSound;
        audioSource.Play();
        textOnScreen.text = "";
        foreach (char letter in dialogueText.text.ToCharArray()){
            textOnScreen.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        audioSource.Stop();
    }
}

