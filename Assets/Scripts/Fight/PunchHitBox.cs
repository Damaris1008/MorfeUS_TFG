using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchHitBox : MonoBehaviour
{
    public Collider2D hitBoxCollider;
    private float damageAttack;
    private Player player;

    void Start(){
        hitBoxCollider.GetComponent<Collider2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        damageAttack = player.punchDamageAttack;
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("Enemy")){
            col.SendMessage("Damage", damageAttack * player.damageMultiplier);
        }
    }
}
