using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectiles : MonoBehaviour
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
    // numOfRadialProjectiles = 12;
    
    // Third phase (1/4 health):

    // Invoke 2 skeletons
    // angleStep = 5f;
    // numOfSpirals = 4;
    // rateOfSpiralProjectiles = 0.1f;
    // rateOfRadialProjectiles = 0.3f;
    // numOfRadialProjectiles = 14;

    bool isSpiralPattern;

    [Header("Spiral Pattern")]
    float angle = 0f;
    float angleStep = 5f;
    int numOfSpirals = 3; 
    float rateOfSpiralProjectiles = 0.2f;

    [Header("Radial Pattern")]
    float rateOfRadialProjectiles = 0.6f;
    int numOfRadialProjectiles = 10;

    GameObject boss;
    Vector2 startPoint;
    float radius, moveSpeed;
    
    float timeToKeepInvoking = 7f;
    float invokingTimer;
    bool isLaunching;


    void Start(){
        radius = 5f;
        moveSpeed = 5f;
        boss = GameObject.Find("Boss");
        invokingTimer = timeToKeepInvoking;
    }

    void Update()
    {

        invokingTimer -= Time.deltaTime;
        if(invokingTimer <=0){
            CancelInvoke();
            isLaunching=false;
        }
        
        if(Input.GetKeyDown(KeyCode.M) && !isLaunching){
            boss.GetComponent<Boss>().StartCoroutine("LaunchAnimation");
            isSpiralPattern=true;
            StartCoroutine(WaitForLaunch(isSpiralPattern));
        }

        if(Input.GetKeyDown(KeyCode.N) && !isLaunching){
            boss.GetComponent<Boss>().StartCoroutine("LaunchAnimation");
            isSpiralPattern=false;
            StartCoroutine(WaitForLaunch(isSpiralPattern));
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

    void LaunchRadialProjectiles(){
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
    
    
}
