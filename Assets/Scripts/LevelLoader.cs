using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    private Animator transition;
    public float transitionTime = 1f;
    private Player player;
    private bool levelSaved;

    public void Awake(){
        transition = GetComponent<Animator>();
    }

    public void LoadNextLevel(){
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadTheLevel(int levelIndex){
        StartCoroutine(LoadLevel(levelIndex));
    }

    public bool IsNewLevelLoaded(int levelIndex)
    {
        return (SceneManager.GetActiveScene().buildIndex == levelIndex);
    }

    IEnumerator LoadLevel(int levelIndex){
        transition.SetTrigger("StartCrossfade");
        yield return new WaitForSeconds(transitionTime);
        transition.SetTrigger("EndCrossfade");
        SceneManager.LoadScene(levelIndex);

        //Refresh scripts
        yield return new WaitUntil(() => IsNewLevelLoaded(levelIndex));
        if(levelIndex >= 3 && levelIndex <= 6){
            GameManager.RefreshScripts();
        }else{
            GameManager.DeleteAllSavedGameObjects();
            SetBrightnessAlpha(levelIndex);
        }    
    }

    public void SetBrightnessAlpha(int sceneIndex){
        Image brightnessPanel = GameObject.FindWithTag("BrightnessPanel").GetComponent<Image>();
        float brightnessSliderValue = PlayerPrefs.GetFloat("Brightness", 1f);
        brightnessPanel.color = new Color(brightnessPanel.color.r, brightnessPanel.color.g, brightnessPanel.color.b, (100-brightnessSliderValue)*180/100/255);
    }
}
