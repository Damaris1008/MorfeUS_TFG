using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    Rigidbody2D rigidbody2d;
    Animator animator;

    [Header("Movement")]
    public float speed = 0.5f;
    public bool vertical;
    public float changeTime = 5.0f;
    float timer;
    int direction = 1;

    [Header("Enemy Sounds")]
    AudioSource audioSource;
    public AudioClip zombieSound;
    public AudioClip zombieHitSound1;
    public AudioClip zombieHitSound2;
    public AudioClip zombieDeathSound;
    //AudioClip[] hitSounds = {zombieHitSound1, zombieHitSound2};

    [Header("Damage Attack")]
    [SerializeField] public int damageAmount = 4; //one heart
    
    [Header("Health")]
    [SerializeField] public HPBar healthBar;
    [SerializeField] public int maxHealth = 12;
    private int currentHealth;
    public int health { get { return currentHealth; }}
    bool isHitted = false;
    bool isDead = false;

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
        defaultColor = spriteRenderer.color;
        timer = changeTime;
        currentHealth = maxHealth;
        isDead = false;
    }

    void Update(){
        
        if(!isDead){
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                direction = -direction;
                timer = changeTime;
                audioSource.Play();
            }

            Vector2 position = rigidbody2d.position;

            if (vertical){
                position.y = position.y + Time.deltaTime * direction * speed;
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", direction);
            }else{
                position.x = position.x + Time.deltaTime * direction * speed;
                animator.SetFloat("MoveX", direction);
                animator.SetFloat("MoveY", 0);
            }
    
            rigidbody2d.position = position;
        }
    }

    public void Damage(int amount){
        
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        healthBar.TakeDamageBar(amount);
        if(currentHealth <= 0)
        {
            Die();
        }
        isHitted = true;
        audioSource.PlayOneShot(zombieHitSound1);
        StartCoroutine("SwitchColor");
    }

    IEnumerator SwitchColor(){
        spriteRenderer.color = hittedColor;
        yield return new WaitForSeconds(timeToColor);
        spriteRenderer.color = defaultColor;
        isHitted = false;
    }

    void Die(){
        isDead = true;
        animator.SetTrigger("Dead");
        
        GameObject healthBar = this.gameObject.transform.GetChild(0).gameObject;
        healthBar.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("DeadEnemies");
        
        StartCoroutine("WaitToDestroy");
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    
    void OnCollisionStay2D(Collision2D other)
    {
        SwordHitBox swordHitBox = other.gameObject.GetComponent<SwordHitBox>();
        if(swordHitBox == null){
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null && !isDead) {        
                player.Damage(damageAmount);
            }
        }

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<Player>() == null && other.gameObject.GetComponent<LaunchObject>() == null
        && other.gameObject.GetComponent<SwordHitBox>() == null){
            direction = -direction;
            timer = changeTime;
        }
    }
    
}
