using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    private Animator transition;
    public float transitionTime = 2f;
    private Player player;
    private bool levelSaved;

    public void Awake(){
        transition = GetComponent<Animator>();
    }

    public void LoadNextLevel(){
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private bool AreDataSaved()
    {
        return GameManager.inventorySaved && GameManager.playerSaved;
    }

    private bool IsNewLevelLoaded(int levelIndex)
    {
        return (SceneManager.GetActiveScene().buildIndex == levelIndex);
    }

    IEnumerator LoadLevel(int levelIndex){
        //yield return new WaitUntil(AreDataSaved);
        
        transition.SetTrigger("StartCrossfade");
        yield return new WaitForSeconds(transitionTime);
        transition.SetTrigger("EndCrossfade");
        SceneManager.LoadScene(levelIndex);

        //Refresh scripts
        yield return new WaitUntil(() => IsNewLevelLoaded(levelIndex));
        GameManager.RefreshScripts();

        /*int scene = SceneManager.GetActiveScene().buildIndex;
        if(scene == 3 || scene == 4 || scene == 5 || scene == 6){
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
            player.canMove = true;
        }*/
    
    }
}
