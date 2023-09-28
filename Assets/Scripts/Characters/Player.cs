using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    [Header("Movement")]
    public bool canMove;
    Rigidbody2D rigidbody2d;
    Animator animator;
    float horizontal; 
    float vertical;
    public float speed;
    Vector2 lookDirection = new Vector2(0,-1);
    public Sprite idleUp;

    [Header("Fight")]
    public float punchDamageAttack;
    private Button canvasBackground; 

    [Header("Health")]
    public HealthHeartsManager healthHeartsManager;
    public int maxHealth = 24;
    public int currentHealth;
    public int health { get { return currentHealth; }}
    bool isHitted = false;
    bool isHealing = false;
    bool isDead = false;

    [Header("Resources")]
    public int coins;
    public int keys;

    [Header("Sprite Color When Damaged")]
    public float timeToColor = 0.25f;
    public Color hittedColor = new Color(1f, 0.30196078f, 0.30196078f);
    public Color healedColor = new Color(0.1952441f, 1f, 0f);
    SpriteRenderer spriteRenderer;
    Color defaultColor;

    [Header("Invincibility")]
    public float timeInvincible = 0.3f;
    public bool isInvincible;
    float invincibleTimer;

    [Header("Launch")]
    public GameObject arrow;

    [Header("Power-Up")]
    public float speedIncrease;
    public float damageMultiplier;

    [Header("Sounds")]
    AudioSource audioSource;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    public AudioClip arrowSound;

    [Header("Scripts")]
    private PopUpsManager popUpsManager;
    private InventoryController inventoryManager;

    [Header("Weapons")]
    public bool isUsingSword;
    public bool isUsingBow;

    void Awake(){
        //Power-up initialization
        speedIncrease = 0f;
        damageMultiplier = 1f;

        coins = 5;
        keys = 3;
        currentHealth = maxHealth;
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        defaultColor = spriteRenderer.color;
        animator = GetComponent<Animator>();
        isDead = false;
        canMove = true;
        canvasBackground = GameObject.FindWithTag("CanvasBackground").GetComponent<Button>();
    }

    public void Start()
    {
        canvasBackground.onClick.AddListener(Attack);

        // Scripts
        popUpsManager = GameObject.FindWithTag("PopUpsManager").GetComponent<PopUpsManager>();
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryController>();

        if(SceneManager.GetActiveScene().buildIndex == 4){
            animator.SetFloat("Look X", 0f);
            animator.SetFloat("Look Y", 1f);
        }else{
            animator.SetFloat("Look X", 0f);
            animator.SetFloat("Look Y", -1f);
        }

        if(SceneManager.GetActiveScene().buildIndex != 3){
            animator.Play("Idle");
        }else{
            animator.Play("Awaking");
            StartCoroutine(popUpsManager.ShowGameGoal());
        }

        popUpsManager.RefreshCoinsCounters(coins);
        popUpsManager.RefreshKeysCounters(keys);
    }

    void Update()
    {
        if(!canMove){
            return;
        }

        // Run
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 3.25f + speedIncrease;
        }else{
            speed = 2.5f + speedIncrease;
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Punch") || 
        animator.GetCurrentAnimatorStateInfo(0).IsName("HitSword") ||
        animator.GetCurrentAnimatorStateInfo(0).IsName("AttackBow") ||  
        animator.GetCurrentAnimatorStateInfo(0).IsName("Awaking") ||
        animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            return;
        }

        if(isUsingSword){
            animator.SetBool("Sword", true);
            animator.SetBool("Bow", false);
        }else if(isUsingBow){
            animator.SetBool("Sword", false);
            animator.SetBool("Bow", true);
        }else{
            animator.SetBool("Sword", false);
            animator.SetBool("Bow", false);  
        }

        // Movement
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);
        
        //If the main character is moving
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();

            //We are sending the variables to the animator
            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
        }
                
        animator.SetFloat("Speed", move.magnitude);

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Moving") || 
            animator.GetCurrentAnimatorStateInfo(0).IsName("MovingSword") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("MovingBow")){
            Vector2 position = rigidbody2d.position;
            //Multiplying by Time.deltaTime makes the character movement be the same regardless of how many frames per second are used to play the game
            //Time.deltaTime is the time Unity takes to reproduce a frame
            position = position + move * speed * Time.deltaTime;
            rigidbody2d.position = position;
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void Attack(){
        if(isUsingSword){
            animator.SetTrigger("Hit");
        }else if(isUsingBow){
            animator.SetTrigger("LaunchArrow");
            StartCoroutine("LaunchArrow");
        }else{
            animator.SetTrigger("Punch");
        }
    }

    public void Damage(int amount){
        if (isInvincible || isDead)
            return;
        isInvincible = true;
        isHitted = true;
        invincibleTimer = timeInvincible;

        //This assures that the currentHealth will never be less than 0 or greater than maxHealth
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        healthHeartsManager.DrawHearts();
        if(currentHealth <= 0)
        {
            Die();
            return;
        } 


        audioSource.PlayOneShot(hurtSound);
        StartCoroutine("SwitchColor");
    }

    public void Heal(int amount){ //Replace parameter with (Item item) and make the audio sound from here
        if(currentHealth == maxHealth || isDead){
            return;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        healthHeartsManager.DrawHearts();
        isHealing = true;
        StartCoroutine("SwitchColor");
    }

    IEnumerator SwitchColor(){
        if(isHitted){
            spriteRenderer.color = hittedColor;
            yield return new WaitForSeconds(timeToColor);
            spriteRenderer.color = defaultColor;
            isHitted = false;
        }else if(isHealing){
            spriteRenderer.color = healedColor;
            yield return new WaitForSeconds(timeToColor);
            spriteRenderer.color = defaultColor;
            isHealing = false;
        }
    }

    public void Die(){
        isDead = true;
        animator.SetTrigger("Dead");
        gameObject.layer = LayerMask.NameToLayer("DeadPlayer");
        //If its boss level
        if(SceneManager.GetActiveScene().buildIndex == 6){
            GameObject hackedPanel = GameObject.Find("Boss").GetComponent<Boss>().hackedPanel;
            //Deactivate hacked panel
            for(int i = 0; i < hackedPanel.transform.childCount; ++i) {
                hackedPanel.transform.GetChild(i).gameObject.SetActive(false);
            } 
        }
        audioSource.PlayOneShot(deathSound);
        StartCoroutine("WaitForDeathMenu");
    }

    IEnumerator WaitForDeathMenu(){
        yield return new WaitForSeconds(2f);
        if(maxHealth == 4){
            GameManager.GameOver();
        }else{
            popUpsManager.OpenDeathMenu(maxHealth-4);
        }
    }

    public void Revive(){
        if(isDead){
            Start();
            gameObject.layer = LayerMask.NameToLayer("Player");
            popUpsManager.CloseDeathMenu();
            isDead = false;
            GameObject startPoint = GameObject.FindWithTag("StartPoint");
            rigidbody2d.position = startPoint.transform.position;
            maxHealth = maxHealth - 4;
            currentHealth = maxHealth;
            healthHeartsManager.DrawHearts();

            if(SceneManager.GetActiveScene().name == "BossLevel"){
                //All enemies: revive and make inactive
                Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>(true);
                for(int i=0; i<enemies.Length;i++){
                    enemies[i].Revive();
                    enemies[i].gameObject.SetActive(false);
                }
                //Boss: revive
                Boss boss = GameObject.FindObjectOfType<Boss>(true);
                boss.Revive();
            }else{
                //All enemies: revive
                Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>(true);
                for(int i=0; i<enemies.Length;i++){
                    enemies[i].Revive();
                }
                //All chests: reopen
                GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");
                for(int i=0; i<chests.Length;i++){
                    chests[i].GetComponent<Chest>().CloseChest();
                }            
            }

        }
    }

    IEnumerator LaunchArrow(){
        
        yield return new WaitForSeconds(0.6f);

        float angle = Vector2.SignedAngle(Vector2.up, lookDirection);
        arrow.transform.position = rigidbody2d.position + Vector2.up * (0.1f);
        arrow.transform.rotation = Quaternion.Euler(x:0, y:0, z:angle);
        //If the player is looking to the right
        if(angle < 0 && angle > -180){
            arrow.GetComponent<SpriteRenderer>().flipX = true;
        }
        //If the player is looking down
        else if((angle>90 && angle<180) || (angle<-90 && angle > -180) || angle==180){
            arrow.transform.position = rigidbody2d.position + Vector2.up * (-0.15f);
        }
        //If the player is looking up
        else if(angle>-90 && angle<90){
            arrow.transform.position = rigidbody2d.position + Vector2.up * (0.5f);
        }

        audioSource.PlayOneShot(arrowSound);
        LaunchObject launch = arrow.GetComponent<LaunchObject>();
        launch.Launch(lookDirection, 350);
        

    }

    public void SpendCoins(int amount){
        int result = coins - amount;
        if(result >= 0){
            coins = result;
        }else{
            coins = 0;
        }
        popUpsManager.RefreshCoinsCounters(coins);
    }

    public void WinCoins(int amount){
        coins += amount;
        popUpsManager.RefreshCoinsCounters(coins);
    }

    public void SpendKeys(int amount){
        int result = keys - amount;
        if(result >= 0){
            keys = result;
        }else{
            keys = 0;
        }
        popUpsManager.RefreshKeysCounters(keys);
    }

    public void WinKeys(int amount){
        keys += amount;
        popUpsManager.RefreshKeysCounters(keys);
    }

    public void WinItem(Item item){
        inventoryManager.AddItem(item);
    }

    public void WinPowerUp(){
        speedIncrease = 0.75f;
        damageMultiplier = 1.5f;
        popUpsManager.ShowPowerUpInfo(speedIncrease, damageMultiplier);
    }

}
