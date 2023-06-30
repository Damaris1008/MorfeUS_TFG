using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHeartsManager : MonoBehaviour
{
    public GameObject heartPrefab;
    public Player player;
    List<HealthHeart> hearts = new List<HealthHeart>();

    private void Start(){
        DrawHearts();
    }

    public void ClearHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }

    public void CreateEmptyHeart(){
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void DrawHearts(){
        ClearHearts();
        int heartsToDraw = (int)((player.maxHealth/4) + (player.maxHealth%4));
        for(int i = 0; i<heartsToDraw; i++){
            CreateEmptyHeart();
        }
        for(int i = 0; i<hearts.Count; i++){
            //This restricts the number to be between 0 and 4 to get the status of the current heart
            int heartStatusRemainder = (int)Mathf.Clamp(player.health - (i*4), 0, 4); 
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }
    }

}
