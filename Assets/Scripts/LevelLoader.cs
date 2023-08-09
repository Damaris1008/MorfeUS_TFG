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

    void Start(){
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void LoadNextLevel(){
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex){
        transition.SetTrigger("StartCrossfade");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
        player.canMove = true;

    }
}
