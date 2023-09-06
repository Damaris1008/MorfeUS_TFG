using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitBox : MonoBehaviour
{
    public Collider2D swordCollider;
    public Item sword;
    private Player player;

    void Start(){
        swordCollider.GetComponent<Collider2D>(); 
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("Enemy")){
            col.SendMessage("Damage", sword.stats * player.damageMultiplier);
        }
    }
}
