using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataBetweenLevels : MonoBehaviour
{
    void Awake()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        if(scene >= 3 && scene <= 6){
            DontDestroyOnLoad(gameObject);
        }
    }
}
