using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    Rigidbody2D rigidbody2d;
    Animator animator;

    [Header("Enemy Sounds")]
    AudioSource audioSource;
    public AudioClip enemySound;
    public AudioClip enemyHitSound;
    public AudioClip enemyDeathSound;
    public float soundTimer;

    [Header("Movement")]
    public float speed = 0.5f;
    public Transform path;
    public List<Transform> waypoints;
    public bool loop;
    private int currentPoint = 0;
    private float reachDistance = 0.01f;
    private string direction = "going";

    [Header("Damage Attack")]
    [SerializeField] public int damageAmount = 4; //one heart
    
    [Header("Health")]
    [SerializeField] public HPBar healthBar;
    [SerializeField] public int maxHealth = 12;
    private int currentHealth;
    public int health { get { return currentHealth; }}
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
        currentHealth = maxHealth;
        isDead = false;
        soundTimer = Random.Range(2.0f, 5.0f);
    }

    void Start()
    {
        waypoints = new List<Transform>();
        for(int i=0; i<path.childCount; i++)
        {
            waypoints.Add(path.GetChild(i));
        }
    }

    void FixedUpdate()
    {
        soundTimer -= Time.deltaTime;
        if(soundTimer <=0){
            audioSource.PlayOneShot(enemySound);
            soundTimer = Random.Range(2.0f, 5.0f);
        }

        //Distance between the enemy and the waypoint he's going to
        float distance = Vector2.Distance(waypoints[currentPoint].position, transform.position);

        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentPoint].position, Time.deltaTime*speed);

        Vector2 dir = waypoints[currentPoint].position - transform.position;
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
        else if(angle <= 225 && angle >= 135){
            animator.SetFloat("MoveX", -0.5f);
            animator.SetFloat("MoveY", 0f);
        }//Down
        else{
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", -0.5f);
        }

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
        speed = 0;
        animator.SetTrigger("Dead");
        audioSource.PlayOneShot(enemyDeathSound);
        
        GameObject healthBar = this.gameObject.transform.GetChild(0).gameObject;
        healthBar.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("DeadEnemies");
        
        StartCoroutine("WaitToDestroy");
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
    
}
