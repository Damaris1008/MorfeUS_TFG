using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    public Enemy enemy;

    float maxHealthBarScaleX;
    float maxHealthEnemy;

    void Start(){
        maxHealthBarScaleX = transform.localScale.x;
        maxHealthEnemy = enemy.maxHealth;
    }

    public void TakeDamageBar(float damage){
        float newBarScale = enemy.health * maxHealthBarScaleX / maxHealthEnemy;
        if(newBarScale >= maxHealthBarScaleX){
            newBarScale = 0.0f;
        }
        transform.localScale = new Vector3(newBarScale, transform.localScale.y, transform.localScale.z);
    }
}
