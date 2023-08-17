using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private AudioSource audioSource;
    private Player player;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void OpenDoor(){
        //player.canMove = false;
        audioSource.Play();
        StartCoroutine("WaitToLoadScene");
    }

    IEnumerator WaitToLoadScene(){
        yield return new WaitForSeconds(1f);
        GameManager.NextScene();
    }

}
