using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveCanvas : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if(SceneManager.GetActiveScene().buildIndex < 3 || SceneManager.GetActiveScene().buildIndex > 6){
            Destroy(this.gameObject);
        }
    }
}
