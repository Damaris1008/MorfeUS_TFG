using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    private Animator transition;
    public float transitionTime = 1f;
    private Player player;

    public void Awake(){
        transition = GetComponent<Animator>();
    }

    public void LoadNextLevel(){
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex){
        transition.SetTrigger("StartCrossfade");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);

        int scene = SceneManager.GetActiveScene().buildIndex;
        if(scene == 3 || scene == 4 || scene == 5 || scene == 6){
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
            player.canMove = true;
        }
    }
}
