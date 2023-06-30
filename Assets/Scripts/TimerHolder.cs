using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerHolder : MonoBehaviour
{
    public static float timer = 0;
    public float visibleTimer;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        timer+=Time.deltaTime;
        visibleTimer = timer;
    }
}
