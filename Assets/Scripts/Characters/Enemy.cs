using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    Rigidbody2D rigidbody2d;
    Animator animator;
    private Player player;

    [Header("Enemy Type")]
    public bool isGrimReaper;
    public bool isSkeleton;

    [Header("Enemy Sounds")]
    AudioSource audioSource;
    public AudioClip enemySound;
    public AudioClip enemyHitSound;
    public AudioClip enemyDeathSound;
    public float soundTimer;

    [Header("Movement")]
    public Transform path;
    public List<Transform> waypoints;
    public bool loop;
    private int currentPoint = 0;
    private float reachDistance = 0.1f;
    private string direction = "going";

    [Header("Follow Player")]
    public Transform playerTransform;
    private NavMeshAgent agent;
    public float followRange = 3.0f;

    [Header("Attack")]
    [SerializeField] public int damageAmount = 4; //one heart
    public float timeToAttack;
    private bool isAttacking;
    private float attackTimer;
    
    [Header("Health")]
    [SerializeField] public HPBar healthBar;
    [SerializeField] public int maxHealth = 12;
    private int currentHealth;
    public int health { get { return currentHealth; }}
    bool isDead = false;

    [Header("Drops")]
    public int dropPercentage;
    public Animator popUpAnimator;
    public Text amountText;
    public GameObject coinImg;
    public GameObject keyImg;
    private GameObject rewardImg;

    [Header("Sprite Color When Damaged")]
    public float timeToColor = 0.25f;
    public Color hittedColor = new Color(1f, 0.30196078f, 0.30196078f);
    SpriteRenderer spriteRenderer;
    Color defaultColor;

    void Awake(){
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        defaultColor = spriteRenderer.color;
        currentHealth = maxHealth;
        isDead = false;
        soundTimer = Random.Range(2.0f, 5.0f);
        timeToAttack = 2f;
    }

    void Start()
    {
        GameObject playerGo = GameObject.FindWithTag("Player");
        player = playerGo.GetComponent<Player>();
        playerTransform = playerGo.transform;
        
        //To make the agent appear on the screen
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        waypoints = new List<Transform>();
        for(int i=0; i<path.childCount; i++)
        {
            waypoints.Add(path.GetChild(i));
        }

    }

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
                attackTimer -= Time.deltaTime;
                if (attackTimer < 0){
                    isAttacking = false;
                }
                
            }

            //Movement

            bool targetDetected;
            float distanceToPlayer = Vector2.Distance(playerTransform.transform.position, transform.position);

            if(distanceToPlayer <= followRange){

                targetDetected = true;
                
                //Attack
                if(!isAttacking){
                    if(isSkeleton && distanceToPlayer<=7){
                        // Attack animation
                        // Launch bone
                    }else if(isGrimReaper && distanceToPlayer<=1){
                        StartCoroutine("GrimReaperAttack");
                    }else{
                        Move(targetDetected);
                    }
                }

            }else{
                targetDetected = false;
                Move(targetDetected);
            }

        }        
    }

    IEnumerator GrimReaperAttack(){
        isAttacking = true;
        agent.Stop();
        agent.enabled = false;
        agent.enabled = true;
        animator.SetTrigger("Attack");
        attackTimer = timeToAttack;
        yield return new WaitForSeconds(0.5f);
        Move(true);
    }

    public void Move(bool targetDetected){
        Vector3 destination;
        //Follow player
        if(targetDetected){
            destination = playerTransform.position;
            agent.SetDestination(destination);
        //Follow path
        }else{
            float distance = Vector2.Distance(waypoints[currentPoint].position, transform.position);
            if(currentPoint == 0){
                direction = "going";
            }
            if(distance <= reachDistance)
            {
                if(direction=="going") currentPoint += 1;
                else if(direction=="comingBack") currentPoint -= 1;
            }
            if(currentPoint == path.childCount)
            {
                if(loop) currentPoint = 0;
                else{
                    direction = "comingBack";
                    currentPoint -= 1;
                } 
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

    IEnumerator SwitchColor(){
        spriteRenderer.color = hittedColor;
        yield return new WaitForSeconds(timeToColor);
        spriteRenderer.color = defaultColor;
    }

    void Die(){
        isDead = true;
        agent.enabled = false;

        GameObject healthBar = this.gameObject.transform.GetChild(0).gameObject;
        healthBar.SetActive(false);

        animator.SetTrigger("Dead");
        audioSource.PlayOneShot(enemyDeathSound);
        gameObject.layer = LayerMask.NameToLayer("DeadEnemies");

        // Drops
        int randomNumber = Random.Range(0,101);
        //Debug.Log("Primer dado: "+randomNumber);
        if(randomNumber<=dropPercentage){
            int keyOrCoin = Random.Range(0,2);
            int randomNumber2 = Random.Range(0,101);
            //Debug.Log("Segundo dado: "+randomNumber2);
            int dropAmount;
            bool isKey;
            if(keyOrCoin==0){ //Coin
                isKey=false;
                if(randomNumber2<5){ 
                    dropAmount = 5;
                }else if(randomNumber2<10){ 
                    dropAmount = 4;
                }else if(randomNumber2<25){ 
                    dropAmount = 3;
                }else if(randomNumber2<50){ 
                    dropAmount = 2;
                }else{
                    dropAmount = 1;
                }
            }else{ //Key
                isKey=true;
                if(randomNumber2<10){ 
                    dropAmount = 3;
                }else if(randomNumber2<25){ 
                    dropAmount = 2;
                }else{
                    dropAmount = 1;
                }
            }

            StartCoroutine(PopUp(isKey, dropAmount));
        }
        
        StartCoroutine("WaitToDestroy");
    }

    IEnumerator PopUp(bool isKey, int dropAmount){

        //Activate animation
        yield return new WaitForSeconds(0.3f);
        if(isKey){
            keyImg.SetActive(true);
            player.WinKeys(dropAmount);
        }else{
            coinImg.SetActive(true);
            player.WinCoins(dropAmount);
        }
        amountText.text = "+"+dropAmount;
        popUpAnimator.SetTrigger("RaiseText");

        //Deactivate animation
        yield return new WaitForSeconds(4f);
        amountText.text = "";
        keyImg.SetActive(false);
        coinImg.SetActive(false);
        
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
    
}
