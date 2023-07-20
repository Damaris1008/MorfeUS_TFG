using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{

    PopUpsManager popUpsManager;

    void Start(){
        popUpsManager = GameObject.FindWithTag("PopUpsManager").GetComponent<PopUpsManager>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        gameObject.SetActive(false);
        if(gameObject.name == "EvolutionedVirus" && other.collider.tag=="Player"){
            popUpsManager.StartCoroutine("HackScreen");
        }
    }
}
