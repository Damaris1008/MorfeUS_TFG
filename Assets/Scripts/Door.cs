using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OpenDoor(){
        audioSource.Play();
        StartCoroutine("WaitToLoadScene");
    }

    IEnumerator WaitToLoadScene(){
        yield return new WaitForSeconds(1f);
        GameManager.NextScene();
    }

}
