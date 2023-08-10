using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public PopUpsManager popUpsManager;
    public InventoryController inventoryManager;

    [Header("Weapons")]
    public bool isUsingSword;
    public bool isUsingBow;

    void Awake(){
        //Power-up initialization
        speedIncrease = 0f;
        damageMultiplier = 1f;

        coins = 3;
        keys = 2;
        currentHealth = maxHealth;
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        defaultColor = spriteRenderer.color;
        animator = GetComponent<Animator>();
        isDead = false;
        canMove = true;

        //Save and load
        GameEvents.SaveInitiated += Save;
        Load();
    }

    void Start()
    {

        if(SceneManager.GetActiveScene().buildIndex == 4){
            animator.SetFloat("Look X", 0f);
            animator.SetFloat("Look Y", 1f);
        }
        if(SceneManager.GetActiveScene().buildIndex != 3){
            animator.Play("Idle");
        }

        popUpsManager.RefreshCoinsCounters(coins);
        popUpsManager.RefreshKeysCounters(keys);

    }

    void Update()
    {
        if(!canMove){
            return;
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

        // Attack
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isUsingSword){
                animator.SetTrigger("Hit");
            }else if(isUsingBow){
                animator.SetTrigger("LaunchArrow");
                StartCoroutine("LaunchArrow");
            }else{
                animator.SetTrigger("Punch");
            }

        }

        // Run
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 2.5f + speedIncrease;
        }else{
            speed = 1.5f + speedIncrease;
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
            gameObject.layer = LayerMask.NameToLayer("Player");
            popUpsManager.CloseDeathMenu();
            isDead = false;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameObject startPoint = GameObject.FindWithTag("StartPoint");
            rigidbody2d.position = startPoint.transform.position;
            animator.Play("Awaking");
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
        launch.Launch(lookDirection, 250);
        

    }

    public void SpendCoins(int amount){
        coins -= amount;
        popUpsManager.RefreshCoinsCounters(coins);
    }

    public void WinCoins(int amount){
        coins += amount;
        popUpsManager.RefreshCoinsCounters(coins);
    }

    public void SpendKeys(int amount){
        keys -= amount;
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
        speedIncrease = 1.25f;
        damageMultiplier = 1.5f;
        popUpsManager.ShowPowerUpInfo(speedIncrease, damageMultiplier);
    }

    public List<int> ToDataList1(){ 
        List<int> dataList = new List<int>();
        dataList.Add(keys);
        dataList.Add(coins);
        dataList.Add(maxHealth);
        dataList.Add(currentHealth);
        return dataList;
    }

    public List<float> ToDataList2(){ 
        List<float> dataList = new List<float>();
        dataList.Add(speedIncrease);
        dataList.Add(damageMultiplier);
        return dataList;
    }

    public void FillWithDataList(List<int> dataList1, List<float> dataList2){
        keys = dataList1[0];
        coins = dataList1[1];
        maxHealth = dataList1[2];
        currentHealth = dataList1[3];
        speedIncrease = dataList2[0];
        damageMultiplier = dataList2[1];
    }

    void Save(){
        List<int> dataList1 = ToDataList1();
        List<float> dataList2 = ToDataList2();
        SaveLoad.Save<List<int>>(dataList1, "PlayerStatus_part1");
        SaveLoad.Save<List<float>>(dataList2, "PlayerStatus_part2");
    }

    void Load(){
        if(SaveLoad.SaveExists("PlayerStatus_part1") && SaveLoad.SaveExists("PlayerStatus_part2")){
            Debug.Log("Loading Player Status!");
            List<int> dataList1 = SaveLoad.Load<List<int>>("PlayerStatus_part1");
            List<float> dataList2 = SaveLoad.Load<List<float>>("PlayerStatus_part2");
            FillWithDataList(dataList1, dataList2);
        }
    }

}
