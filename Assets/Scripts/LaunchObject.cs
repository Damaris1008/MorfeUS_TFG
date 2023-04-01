using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchObject : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    bool isLaunching;

    [Header("Damage Attack")]
    public int damageAttack = 2; //half heart

    [Header("Particles")]
    public ParticleSystem hitEffect;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip impactSound;

    [Header("Punch/Arrow")]
    public bool isPunch;
    public bool isArrow;
    public Item bow;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
        isLaunching = false;
    }

    public void Launch(Vector2 direction, float force)
    {
        if(!isLaunching){
            isLaunching = true;
            gameObject.SetActive(true);
            rigidbody2d.AddForce(direction * force);
            StartCoroutine(DeactivateLaunch(direction,force));
        }
    }

    private IEnumerator DeactivateLaunch(Vector2 direction, float force)
    {        
        yield return new WaitForSeconds(0.4f);
        rigidbody2d.AddForce(-direction * force);
        gameObject.SetActive(false);
        isLaunching = false;
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.collider.ToString());
        if(other.collider.gameObject.CompareTag("Enemy")){
            if(isArrow){
                other.collider.SendMessage("Damage", bow.stats);
            }else if(isPunch){
                other.collider.SendMessage("Damage", damageAttack);
            }
            
        }
        hitEffect.gameObject.transform.position = transform.position;
        hitEffect.Play();
        audioSource.PlayOneShot(impactSound);
        gameObject.SetActive(false);
        isLaunching = false;
    }
}
