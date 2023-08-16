using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    protected Rigidbody2D rigidbody2d;
    protected Animator animator;
    private Player player;

    [Header("Enemy Type")]
    public bool isZombie;
    public bool isGrimReaper;
    public bool isSkeleton;

    [Header("Enemy Sounds")]
    public AudioSource audioSource;
    public AudioClip enemySound;
    public AudioClip enemyBiteSound;
    public AudioClip enemyHitSound;
    public AudioClip enemyDeathSound;
    public float soundTimer;
    public float minSoundTimer;
    public float maxSoundTimer;

    [Header("Movement")]
    public Transform path;
    public List<Transform> waypoints;
    public bool loop;
    protected int currentPoint;
    protected float reachDistance = 0.1f;
    protected string direction = "going";

    [Header("Follow Player")]
    protected Transform playerTransform;
    public NavMeshAgent agent;
    public float followRange = 3.0f;

    [Header("Attack")]
    [SerializeField] public int damageAmount = 4; //one heart
    public float timeToAttack;
    protected bool isAttacking;
    public float attackTimer;
    public GameObject bone;
    
    [Header("Health")]
    [SerializeField] public HPBar healthBar;
    [SerializeField] public int maxHealth = 12;
    protected int currentHealth;
    public int health { get { return currentHealth; }}
    protected bool isDead = false;

    [Header("Invincibility")]
    public float timeInvincible = 0.2f;
    public bool isInvincible;
    protected float invincibleTimer;

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
    protected SpriteRenderer spriteRenderer;
    Color defaultColor;

    void Awake(){
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if(!agent){
            agent = GetComponent<NavMeshAgent>();
        }
        defaultColor = spriteRenderer.color;
        currentHealth = maxHealth;
        isDead = false;
        soundTimer = Random.Range(minSoundTimer, maxSoundTimer);
        timeToAttack = 2f;
        currentPoint = 0;
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
                soundTimer = Random.Range(minSoundTimer, maxSoundTimer);
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

            if(agent.enabled){
                bool targetDetected;
                float distanceToPlayer = Vector2.Distance(playerTransform.transform.position, transform.position);

                if(distanceToPlayer <= followRange){

                    targetDetected = true;
                    
                    //Attack
                    if(!isAttacking){
                        if(isGrimReaper && distanceToPlayer<=1){
                            StartCoroutine("GrimReaperAttack");
                        }else if(isSkeleton && distanceToPlayer<=5){
                            Move(targetDetected);
                            SkeletonAttack();
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
    }

    IEnumerator GrimReaperAttack(){
        isAttacking = true;
        animator.SetTrigger("Attack");
        attackTimer = timeToAttack;
        yield return new WaitForSeconds(0.5f);
        Move(true);
    }

    void SkeletonAttack(){
        isAttacking = true;
        animator.SetTrigger("Attack");
        if(isSkeleton){
            LaunchBone();
        }
        attackTimer = timeToAttack;
    }


    void LaunchBone(){
        bone.transform.position = rigidbody2d.position + Vector2.up * (0.1f);
        Vector3 rotation = playerTransform.position - bone.transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        bone.transform.rotation = Quaternion.Euler(0,0,rotZ);

        Vector3 launchDirection = (playerTransform.position - transform.position).normalized;
        LaunchObject launch = bone.GetComponent<LaunchObject>();
        launch.Launch(launchDirection, 200);
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

            //Invincibility
            if (isInvincible) return;
            isInvincible = true;
            invincibleTimer = timeInvincible;

            currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
            healthBar.TakeDamageBar(amount);
            if(currentHealth <= 0)
            {
                Die(true);
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

    public void Die(bool withDrop){
        isDead = true;
        agent.enabled = false;
        spriteRenderer.sortingOrder = 3;

        GameObject healthBar = this.gameObject.transform.GetChild(0).gameObject;
        healthBar.SetActive(false);

        animator.SetTrigger("Dead");
        audioSource.PlayOneShot(enemyDeathSound);
        gameObject.layer = LayerMask.NameToLayer("DeadEnemies");

        // Drops
        if(withDrop){
            int randomNumber = Random.Range(0,101);
            if(randomNumber<=dropPercentage){
                int randomNumber1 = Random.Range(0,101);
                int randomNumber2 = Random.Range(0,101);
                int dropAmount;
                bool isKey;
                if(randomNumber1>=25){ //Coin: 75%
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
                }else{ //Key: 25%
                    isKey=true;
                    if(randomNumber2<10){ 
                        dropAmount = 3;
                    }else if(randomNumber2<15){ 
                        dropAmount = 2;
                    }else{
                        dropAmount = 1;
                    }
                }
            StartCoroutine(PopUp(isKey, dropAmount));
            }
        }
        
        StartCoroutine(WaitToDestroy(2f));
    }

    public void Revive(){
        gameObject.SetActive(true);
        isDead = false;
        agent.enabled = true;
        spriteRenderer.sortingOrder = 4;
        currentHealth = maxHealth;
        GameObject healthBar = this.gameObject.transform.GetChild(0).gameObject;
        healthBar.SetActive(true);
        healthBar.GetComponentInChildren<HPBar>().Start();
        gameObject.layer = LayerMask.NameToLayer("Enemies");
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
        yield return new WaitForSeconds(1.5f);
        amountText.text = "";
        keyImg.SetActive(false);
        coinImg.SetActive(false);
        
    }

    protected IEnumerator WaitToDestroy(float timeToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroy);
        gameObject.SetActive(false);
    }
    
}
