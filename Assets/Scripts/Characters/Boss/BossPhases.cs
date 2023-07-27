using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhases : MonoBehaviour
{
    // First phase:

    // angleStep = 10f;
    // numOfSpirals = 2;
    // rateOfSpiralProjectiles = 0.3f;
    // rateOfRadialProjectiles = 0.8f;
    // numOfRadialProjectiles = 10;

    // Second phase (half of the health):

    // Invoke 2 grim reapers
    // angleStep = 5f;
    // numOfSpirals = 3;
    // rateOfSpiralProjectiles = 0.2f;
    // rateOfRadialProjectiles = 0.6f;
    // numOfRadialProjectiles = 11;
    
    // Third phase (1/4 health):

    // Invoke 2 skeletons
    // angleStep = 5f;
    // numOfSpirals = 4;
    // rateOfSpiralProjectiles = 0.2f;
    // rateOfRadialProjectiles = 0.3f;
    // numOfRadialProjectiles = 14;

    bool isSpiralPattern;

    [Header("Spiral Pattern")]
    float angle = 0f;
    float angleStep = 5f;
    int numOfSpirals = 4; 
    float rateOfSpiralProjectiles = 0.15f;

    [Header("Radial Pattern")]
    float rateOfRadialProjectiles = 0.3f;
    int numOfRadialProjectiles = 12;

    [Header("Boss Phases")]
    public BossDialogueManager dialogueManager;
    int phase;
    float attackTimer;
    

    [Header("Spawn of Enemies")]
    public GameObject spawn1;
    public GameObject spawn2;

    [Header("Grim Reapers")]
    public GameObject grimReaper1;
    public GameObject grimReaper2;

    [Header("Skeletons")]
    public GameObject skeleton1;
    public GameObject skeleton2;


    GameObject boss;
    Boss bossScript;
    Vector2 startPoint;
    float radius;
    
    float timeToKeepInvoking = 7f;
    float invokingTimer;
    bool isLaunching;


    void Start(){
        radius = 5f;
        boss = GameObject.Find("Boss");
        bossScript = boss.GetComponent<Boss>();
        invokingTimer = timeToKeepInvoking;
        StartPhase(1);
        attackTimer = Random.Range(1,6);
    }

    void Update()
    {
        if(bossScript.health>0){ //if its not dead

            
            if(isLaunching){ //Cancel or not the launching
                invokingTimer -= Time.deltaTime;
                if(invokingTimer <= 0){
                    CancelInvoke();
                    isLaunching=false;
                }
            }else{ //Attack or not
                attackTimer -= Time.deltaTime;
                if(attackTimer < 0 ){
                    isLaunching = true;
                    attackTimer = Random.Range(1,6);
                    RandomTypeOfAttack();
                }
            }
            
            if(Input.GetKeyDown(KeyCode.M) && !isLaunching){
                bossScript.StartCoroutine("LaunchAnimation");
                isSpiralPattern=true;
                StartCoroutine(WaitForLaunch(isSpiralPattern));
            }

            if(Input.GetKeyDown(KeyCode.N) && !isLaunching){
                bossScript.StartCoroutine("LaunchAnimation");
                isSpiralPattern=false;
                StartCoroutine(WaitForLaunch(isSpiralPattern));
            }

            // Phases
            int healthPercentage = bossScript.maxHealth/bossScript.health;
            if(phase==1 && healthPercentage >= 2){
                StartPhase(2);
            }else if(phase==2 && healthPercentage >= 4){
                StartPhase(3);
            }
        }
    }

    IEnumerator WaitForLaunch(bool isSpiralPattern){
        yield return new WaitForSeconds(0.7f);
        if(isSpiralPattern){
            InvokeRepeating("LaunchSpiralProjectiles",0f,rateOfSpiralProjectiles); 
        }else{
            InvokeRepeating("LaunchRadialProjectiles",0f,rateOfRadialProjectiles); 
        }
        invokingTimer = timeToKeepInvoking;
        
    }

    public void LaunchRadialProjectiles(){
        isLaunching = true;

        float angleStep = 360f / numOfRadialProjectiles;
        float angle = 0f;

        for(int i=0;  i<numOfRadialProjectiles; i++){

            float projDirX = boss.transform.position.x + Mathf.Sin((angle*Mathf.PI)/180)*radius;
            float projDirY = boss.transform.position.y + Mathf.Cos((angle*Mathf.PI)/180)*radius;

            Vector3 projMoveVector = new Vector3(projDirX, projDirY, 0f);
            Vector2 projDir = (projMoveVector - boss.transform.position).normalized;

            GameObject proj = BossProjectilePool.bossProjectilePoolInstance.GetBullet();
            proj.transform.position = boss.transform.position;
            proj.transform.rotation = boss.transform.rotation;
            proj.SetActive(true);
            proj.GetComponent<Virus>().SetMoveDirection(projDir);

            angle+=angleStep;
        }
    }

    void LaunchSpiralProjectiles(){
        isLaunching=true;
    
        float separationAngle = 360f/numOfSpirals;

        for(int i=0; i<numOfSpirals; i++){
            float projDirX = boss.transform.position.x + Mathf.Sin(((angle+separationAngle*i)*Mathf.PI)/180f);
            float projDirY = boss.transform.position.y + Mathf.Cos(((angle+separationAngle*i)*Mathf.PI)/180f);

            Vector3 projMoveVector = new Vector3(projDirX, projDirY, 0f);
            Vector2 projDir = (projMoveVector - boss.transform.position).normalized;

            GameObject proj = BossProjectilePool.bossProjectilePoolInstance.GetBullet();
            proj.transform.position = boss.transform.position;
            proj.transform.rotation = boss.transform.rotation;
            proj.SetActive(true);
            proj.GetComponent<Virus>().SetMoveDirection(projDir);

            angle+=angleStep;

            if(angle>=360f){
                angle=0f;
            }
        }
    }

    void StartPhase(int phaseToStart){
        switch(phaseToStart){
            case 1:
                phase=1;
                angleStep = 10f;
                numOfSpirals = 2;
                rateOfSpiralProjectiles = 0.3f;
                rateOfRadialProjectiles = 0.8f;
                numOfRadialProjectiles = 10;
                break;
            case 2:
                phase=2;
                dialogueManager.ShowDialogue();
                SpawnEnemies(); // Invoke 2 grim reapers
                angleStep = 5f;
                numOfSpirals = 3;   
                rateOfSpiralProjectiles = 0.2f;
                rateOfRadialProjectiles = 0.6f;
                numOfRadialProjectiles = 11;
                break;
            case 3:
                phase=3;   
                dialogueManager.ShowDialogue();             
                SpawnEnemies(); // Invoke 2 skeletons
                angleStep = 5f;
                numOfSpirals = 4;
                rateOfSpiralProjectiles = 0.2f;
                rateOfRadialProjectiles = 0.3f;
                numOfRadialProjectiles = 14;
                break;
            default:
                break;
        }
    }

    void RandomTypeOfAttack(){
        int randomNumber = Random.Range(0,2);

        bossScript.StartCoroutine("LaunchAnimation");
        if(randomNumber==0){
            isSpiralPattern=true;
        }else{
            isSpiralPattern=false;
        }
        StartCoroutine(WaitForLaunch(isSpiralPattern));
    }
    
    void SpawnEnemies(){
        if(phase==2){
            grimReaper1.transform.position = spawn1.transform.position;
            grimReaper2.transform.position = spawn2.transform.position;
            grimReaper1.SetActive(true);
            grimReaper2.SetActive(true);
        }else if(phase==3){
            skeleton1.transform.position = spawn1.transform.position;
            skeleton2.transform.position = spawn2.transform.position;
            skeleton1.SetActive(true);
            skeleton2.SetActive(true);
        }
    }
    
}
