using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{

    public GameObject bed;

    void FixedUpdate()
    {
        
        if(!isDead){

            //Enemy sound
            soundTimer -= Time.deltaTime;
            if(soundTimer <=0){
                audioSource.PlayOneShot(enemySound);
                soundTimer = Random.Range(10.0f, 30.0f);
            }

            //Attack timer
            if (isAttacking)
            {
                /*attackTimer -= Time.deltaTime;
                if (attackTimer < 0){
                    isAttacking = false;
                }*/
                
            }else{
                
                //Movement

                bool targetDetected;
                float distanceToPlayer = Vector2.Distance(playerTransform.transform.position, transform.position);

                if(distanceToPlayer <= followRange){

                    targetDetected = true;
                    
                    //Attack
                    if(!isAttacking){
                        Move(targetDetected);
                    }

                }else{
                    targetDetected = false;
                    Move(targetDetected);
                }
            }

            

        }        
    }


    public void Move(bool targetDetected){
        Vector3 destination;
        //Follow player
        if(targetDetected){
            destination = playerTransform.position;
            agent.SetDestination(destination);
        
        //Follow random path
        }else{
            float distance = Vector2.Distance(waypoints[currentPoint].position, transform.position);
            if(distance <= reachDistance)
            {
                int oldPoint = currentPoint;

                do currentPoint = Random.Range(0, waypoints.Count);
                while(oldPoint==currentPoint);
                
            
            }

            destination = waypoints[currentPoint].position;
            agent.SetDestination(destination);
            
        }
        
        //Animations
        Vector2 dir = destination - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
        //Right
        if(angle >= -45 && angle <= 45){
            animator.SetFloat("MoveX", 0.5f);
            animator.SetFloat("MoveY", 0f);
        }//Up
        else if(angle <= 135 && angle >=45){
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", 0.5f);
        }//Left
        else if((angle <= 225 && angle >= 135) || (angle>=-180 && angle <= -135)){
            animator.SetFloat("MoveX", -0.5f);
            animator.SetFloat("MoveY", 0f);
        }//Down
        else{
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", -0.5f);
        }
    }

    public void Damage(int amount){
        if(!isDead){
            currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
            healthBar.TakeDamageBar(amount);
            if(currentHealth <= 0)
            {
                Die();
            }else{
                audioSource.PlayOneShot(enemyHitSound);
                StartCoroutine("SwitchColor");
            }
        }
    }

    void Die(){
        isDead = true;
        agent.enabled = false;

        GameObject healthBar = this.gameObject.transform.GetChild(0).gameObject;
        healthBar.SetActive(false);

        animator.SetTrigger("Dead");
        audioSource.PlayOneShot(enemyDeathSound);
        gameObject.layer = LayerMask.NameToLayer("DeadEnemies");
        
        StartCoroutine(WaitToDestroy(3.5f));
    }

    IEnumerator WaitToDestroy(float timeToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroy);
        gameObject.SetActive(false);

        bed.transform.position = transform.position;
        bed.SetActive(true);
    }

    public IEnumerator LaunchAnimation(){
        isAttacking = true;
        animator.SetTrigger("Cast");
        agent.Stop();
        agent.enabled = false;
        yield return new WaitForSeconds(0.5f);
        StopLaunchAnimation();
    }

    public void StopLaunchAnimation(){
        agent.enabled=true;
        isAttacking = false;
    }

}
