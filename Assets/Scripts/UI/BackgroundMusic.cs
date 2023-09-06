using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip menuBM;
    public AudioClip levelBM;
    public AudioClip gameOverBM;
    public AudioClip introductionBM;
    public AudioClip bossLevelBM;
    public AudioClip finalDialoguedSceneBM;
    public AudioClip creditsBM;

    void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    public void SetBackgroundMusic(int sceneIndex){
        audioSource.loop = true;
        switch(sceneIndex){
            case 0:
                audioSource.clip = menuBM;
                audioSource.Play();
                break;
            case 1:
                audioSource.clip = gameOverBM;
                audioSource.loop = false;
                audioSource.Play();
                break;
            case 2:
                audioSource.clip = introductionBM;
                audioSource.Play();
                
                break;
            case 3:
                audioSource.clip = levelBM;
                audioSource.Play();
                break;
            case 6:
                audioSource.clip = bossLevelBM;
                audioSource.Play();
                break; 
            case 7:
                audioSource.clip = finalDialoguedSceneBM;
                audioSource.Play();
                break;        
            case 8:
                audioSource.clip = creditsBM;
                audioSource.Play();
                break;
            default:
                break;
        }
    }
}
