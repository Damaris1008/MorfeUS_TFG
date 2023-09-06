using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessLevelOutOfGame : MonoBehaviour
{
    void Start()
    {
        //Apply graphics settings (brightness)
        Image brightnessPanel = gameObject.GetComponent<Image>();
        float brightnessSliderValue = PlayerPrefs.GetFloat("Brightness", 1f);
        brightnessPanel.color = new Color(brightnessPanel.color.r, brightnessPanel.color.g, brightnessPanel.color.b, (100-brightnessSliderValue)*180/100/255);
    }
}
