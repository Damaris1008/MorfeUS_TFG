using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalWall : MonoBehaviour
{

    public GameObject finalWall;

    void Update()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0){
            RaiseFinalWall();
        }
    }

    public void RaiseFinalWall(){
        finalWall.SetActive(false);
    }
}
