using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public float speed;

    void Update()
    {
        if(transform.position.y < 1725f){
            transform.Translate(Vector3.up * Time.deltaTime * speed);
        }
    }
}