using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        CinemachineVirtualCamera vcam = gameObject.GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = player.transform;
    }

}
