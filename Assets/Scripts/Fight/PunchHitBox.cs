using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchHitBox : MonoBehaviour
{
    public Collider2D hitBoxCollider;
    public float damageAttack;

    void Start(){
        hitBoxCollider.GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("Enemy")){
            col.SendMessage("Damage", damageAttack);
        }
    }
}
