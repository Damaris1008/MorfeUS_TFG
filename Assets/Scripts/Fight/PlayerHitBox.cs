using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    public Collider2D hitBoxCollider;
    public Player player;

    void Start(){
        hitBoxCollider.GetComponent<Collider2D>();
    }

    void OnCollisionStay2D(Collision2D col){
        if(col.collider.gameObject.CompareTag("Enemy")){
            Enemy enemy = col.collider.gameObject.GetComponent<Enemy>();
            player.SendMessage("Damage", enemy.damageAmount);
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.collider.gameObject.CompareTag("Enemy")){
            Enemy enemy = col.collider.gameObject.GetComponent<Enemy>();
            player.SendMessage("Damage", enemy.damageAmount);
        }
    }

    // Grim reaper hitboxes 
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("EnemyHitBox")){
            Enemy enemy = col.gameObject.transform.parent.gameObject.GetComponent<Enemy>();
            player.SendMessage("Damage", enemy.damageAmount);
        }
    }
}
