using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Boss")]
    public GameObject bed;
    public BossPhases bossPhasesScript;
    private BossDialogueManager dialogueManager;

    void FixedUpdate()
    {
        
        if(!isDead){

            //Invincibility
            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer < 0)
                    isInvincible = false;
            }

            //Enemy sound
            soundTimer -= Time.deltaTime;
            if(soundTimer <=0){
                audioSource.PlayOneShot(enemySound);
                soundTimer = Random.Range(10.0f, 30.0f);
            }

            //Movement
            if(agent.enabled){
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

    public new void Move(bool targetDetected){
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

    public new void Damage(int amount){
        if(!isDead){

            //Invincibility
            if (isInvincible) return;
            isInvincible = true;
            invincibleTimer = timeInvincible;

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
        bossPhasesScript.CancelInvoke();
        isDead = true;
        agent.enabled = false;
        spriteRenderer.sortingOrder = 3;

        //Deactivate hacked panel
        for(int i = 0; i < hackedPanel.transform.childCount; ++i) {
            hackedPanel.transform.GetChild(i).gameObject.SetActive(false);
        } 
        
        GameObject healthBar = this.gameObject.transform.GetChild(0).gameObject;
        healthBar.SetActive(false);

        bossPhasesScript.dialogueManager.ShowDialogue(3);

        animator.SetTrigger("Dead");
        audioSource.PlayOneShot(enemyDeathSound);
        gameObject.layer = LayerMask.NameToLayer("DeadEnemies");
        
        StartCoroutine(WaitToDestroy(3.5f));

        //All the enemies spawned by the boss also die
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies){
            if(enemy.name!="Boss"){
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                enemyScript.Die(false);
            }
        }

    }

    public new void Revive(){
        bed.SetActive(false);
        gameObject.SetActive(true);
        GameObject bossStartPoint = GameObject.FindWithTag("BossStartPoint");
        rigidbody2d.position = bossStartPoint.transform.position;
        isDead = false;
        agent.enabled = true;
        spriteRenderer.sortingOrder = 4;
        currentHealth = maxHealth;
        GameObject healthBar = this.gameObject.transform.GetChild(0).gameObject;
        healthBar.SetActive(true);
        healthBar.GetComponentInChildren<HPBar>().Start();
        gameObject.layer = LayerMask.NameToLayer("Enemies");
        bossPhasesScript.RestartPhases();
    }

    new IEnumerator WaitToDestroy(float timeToDestroy)
    {
        bossPhasesScript.CancelInvoke();

        yield return new WaitForSeconds(timeToDestroy/3);
        bossPhasesScript.LaunchRadialProjectiles();
        yield return new WaitForSeconds(timeToDestroy/6);
        bossPhasesScript.LaunchRadialProjectiles();
        yield return new WaitForSeconds(timeToDestroy/6);
        bossPhasesScript.LaunchRadialProjectiles();
        yield return new WaitForSeconds(timeToDestroy/3);
        
        gameObject.SetActive(false);

        bed.transform.position = transform.position;
        bed.SetActive(true);
    }

    public IEnumerator LaunchAnimation(){
        isAttacking = true;
        animator.SetTrigger("Cast");
        agent.isStopped = true;
        agent.enabled = false;
        yield return new WaitForSeconds(0.5f);
        StopLaunchAnimation();
    }

    public void StopLaunchAnimation(){
        agent.enabled=true;
        isAttacking = false;
    }

}
