using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchObject : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    bool isLaunching;
    private Player player;

    [Header("Object Type")]
    public bool isArrow;
    public bool isBone;

    [Header("Damage Attack")]
    public Item bow;
    public Enemy skeleton;

    [Header("Particles")]
    public ParticleSystem hitEffect;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip impactSound;
   

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
        isLaunching = false;
    }
    
    void Start(){
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void Launch(Vector2 direction, float force)
    {
        if(!isLaunching){
            isLaunching = true;
            gameObject.SetActive(true);
            rigidbody2d.AddForce(direction * force);
            StartCoroutine(ActivateAndDeactivateLaunch(direction,force));
        }
    }

    private IEnumerator ActivateAndDeactivateLaunch(Vector2 direction, float force){
        
        //Activate
        if(isBone){
            yield return new WaitForSeconds(0.2f);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }

        //Deactivate
        if(isBone){
            yield return new WaitForSeconds(1.5f);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }else{
            yield return new WaitForSeconds(0.6f);
        }
        rigidbody2d.AddForce(-direction * force);
        gameObject.SetActive(false);
        isLaunching = false;

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(isArrow && other.collider.gameObject.CompareTag("Enemy")){
            other.collider.SendMessage("Damage", bow.stats * player.damageMultiplier);
        }
        if(hitEffect!=null){
            hitEffect.gameObject.transform.position = transform.position;
            hitEffect.Play();
        }

        //audioSource.PlayOneShot(impactSound);
        gameObject.SetActive(false);
        isLaunching = false;
    }
}
