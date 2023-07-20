using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectiles : MonoBehaviour
{

    [SerializeField]
    List<GameObject> projectiles;

    [SerializeField]
    Sprite virusSprite;

    [SerializeField]
    Sprite evolutionedVirusSprite;

    GameObject boss;
    Vector2 startPoint;
    float radius, moveSpeed;
    bool isLaunching;

    void Awake()
    {
        for(int i=0; i < transform.childCount; i++){
            projectiles.Add(transform.GetChild(i).gameObject);
        }
    }

    void Start(){
        radius = 5f;
        moveSpeed = 5f;
        boss = GameObject.Find("Boss");
    }

    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.M)){
            StartCoroutine("LaunchRadialProjectiles");
        }

        if(isLaunching){
           StopLaunchingProjectiles(); 
        }
    }

    IEnumerator LaunchRadialProjectiles(){

        boss.GetComponent<Boss>().StartCoroutine("LaunchAnimation");

        float angleStep = 360f / projectiles.Count;
        float angle = 0f;
        SpawnRandomProjectiles();

        yield return new WaitForSeconds(0.7f);
        startPoint = boss.transform.position;
        for(int i=0;  i<projectiles.Count; i++){

            //Go back to start point without velocity
            projectiles[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
            projectiles[i].transform.position = boss.transform.position;
            
            //Calclate direction
            float projectileDirXPosition = boss.transform.position.x + Mathf.Sin((angle*Mathf.PI)/180)*radius;
            float projectileDirYPosition = boss.transform.position.y + Mathf.Cos((angle*Mathf.PI)/180)*radius;

            Vector2 projectileVector = new Vector2(projectileDirXPosition, projectileDirYPosition);
            Vector2 projectileMoveDirection = (projectileVector - new Vector2(boss.transform.position.x, boss.transform.position.y)).normalized * moveSpeed;

            //Set active and add velocity
            projectiles[i].SetActive(true);
            projectiles[i].GetComponent<Rigidbody2D>().velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);
            Debug.Log("he entrado");

            angle+=angleStep;
        }

        isLaunching = true;

    }

    void StopLaunchingProjectiles(){

        bool allProjectilesInactive = true;

        for(int i=0;  i<projectiles.Count; i++){
            float distanceToStart = Vector2.Distance(startPoint, projectiles[i].transform.position);
            if(distanceToStart >= 5){
                projectiles[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
                projectiles[i].transform.position = boss.transform.position;
                projectiles[i].SetActive(false);
            }else{
                allProjectilesInactive = false;
            }
        }

        if(allProjectilesInactive){
            isLaunching=false;
        }
    }
    
    void SpawnRandomProjectiles(){
        for(int i=0; i<projectiles.Count; i++){
            int randomInt = Random.Range(0,10); //0-9

            if(randomInt <= 4){ //50% of spawning an evolutioned virus
                projectiles[i].GetComponent<SpriteRenderer>().sprite = evolutionedVirusSprite;
                projectiles[i].name = "EvolutionedVirus";
            }else{
                projectiles[i].GetComponent<SpriteRenderer>().sprite = virusSprite;
                projectiles[i].name = "Virus";
            }
        }
    }
}
