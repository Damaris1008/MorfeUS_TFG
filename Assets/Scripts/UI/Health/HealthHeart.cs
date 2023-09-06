using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    public Sprite fullHeart, threeQuartersHeart, halfHeart, quarterHeart, emptyHeart;
    Image heartImage;

    private void Awake(){
        heartImage = GetComponent<Image>();

    }

    public void SetHeartImage(HeartStatus status){
        switch(status){
            case HeartStatus.Empty:
                heartImage.sprite = emptyHeart;
                break;
            case HeartStatus.Quarter:
                heartImage.sprite = quarterHeart;
                break;
            case HeartStatus.Half:
                heartImage.sprite = halfHeart;
                break;
            case HeartStatus.ThreeQuarters:
                heartImage.sprite = threeQuartersHeart;
                break;
            case HeartStatus.Full:
                heartImage.sprite = fullHeart;
                break;
        }
    }

}

public enum HeartStatus
{
    Empty = 0,
    Quarter = 1,
    Half = 2,
    ThreeQuarters = 3,
    Full = 4
}
