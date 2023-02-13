﻿using System.Collections;
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
    [SerializeField] public int damageAmount = 4;
    
    [Header("Health")]
    [SerializeField] public float maxHealth = 12.0f;
    private float currentHealth;
    public float health { get { return currentHealth; }}
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
        //Debug.Log(hitSounds.ToString());
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
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }else{
            position.x = position.x + Time.deltaTime * direction * speed;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
 
        rigidbody2d.position = position;
    }

    public void Damage(float amount){
        
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        Debug.Log(currentHealth+"/"+maxHealth);
        if(currentHealth <= 0)
        {
            isDead = true;
            //animator.SetTrigger("Dead");
            Debug.Log("Enemy Dead!");
            if(maxHealth == 4){
                GameManager.GameOver();
            }
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

    private void OnTriggerEnter2D(Collider2D collider) {
        Player player = collider.GetComponent<Player>();
        if (player != null) {            
            player.Damage(damageAmount);
        }
    }

    
}