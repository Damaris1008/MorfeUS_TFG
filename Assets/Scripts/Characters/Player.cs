using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    Rigidbody2D rigidbody2d;
    float horizontal; 
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(0,-1);

    [Header("Speed")]
    public float speed = 1.80f;
    
    [Header("Health")]
    public HealthHeartsManager healthHeartsManager;
    public int maxHealth = 24;
    int currentHealth;
    public int health { get { return currentHealth; }}
    bool isHitted = false;
    bool isHealing = false;
    bool healthIsFull;
    bool isDead = false;

    [Header("Sprite Color When Damaged")]
    public float timeToColor = 0.25f;
    public Color hittedColor = new Color(1f, 0.30196078f, 0.30196078f);
    public Color healedColor = new Color(0.1952441f, 1f, 0f);
    SpriteRenderer spriteRenderer;
    Color defaultColor;

    [Header("Invincibility")]
    public float timeInvincible = 1.0f;
    bool isInvincible;
    float invincibleTimer;

    [Header("Punch")]
    public GameObject punchHand;

    [Header("Sounds")]
    AudioSource audioSource;
    public AudioClip hurtSound;
    public AudioClip deathSound;

    [Header("PopUpsManager")]
    public PopUpsManager popUpsManager;

    void Awake(){
        currentHealth = maxHealth;
        healthIsFull = true;
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        defaultColor = spriteRenderer.color;
        animator = GetComponent<Animator>();
        isDead = false;
    }

    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex != 2){
            animator.Play("Idle");
        }
    }

    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Awaking") || 
        animator.GetCurrentAnimatorStateInfo(0).IsName("Punch") ||
        animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            return;
        }

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);
        
        //If the main character is moving
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
                
        //We are sending the variables to the animator
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Moving")){
            Vector2 position = rigidbody2d.position;
            //Multiplying by Time.deltaTime makes the character movement be the same regardless of how many frames per second are used to play the game
            //Time.deltaTime is the time Unity takes to reproduce a frame
            position = position + move * speed * Time.deltaTime;
            rigidbody2d.position = position;
        }


        if (Input.GetKey(KeyCode.Space))
        {
            Punch();
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
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
        healthIsFull = false;

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
        if(healthIsFull || isDead){
            return;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        healthHeartsManager.DrawHearts();
        isHealing = true;
        if(currentHealth == maxHealth){
            healthIsFull = true;
        }
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
            popUpsManager.CloseDeathMenu();
            isDead = false;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameObject startPoint = GameObject.FindWithTag("StartPoint");
            rigidbody2d.position = startPoint.transform.position;
            animator.Play("Awaking");
            maxHealth = maxHealth - 4;
            currentHealth = maxHealth;
            healthIsFull = true;
            healthHeartsManager.DrawHearts();
        }
    }

    public void Punch(){

        float angle = Vector2.SignedAngle(Vector2.up, lookDirection);
        punchHand.transform.position = rigidbody2d.position + Vector2.up * (-0.2f);
        punchHand.transform.rotation = Quaternion.Euler(x:0, y:0, z:angle);
        if(angle < 0 && angle > -180){
            punchHand.GetComponent<SpriteRenderer>().flipX = true;
        }

        Punch punch = punchHand.GetComponent<Punch>();
        punch.Launch(lookDirection, 150);

        animator.SetTrigger("Punch");

    }

}
