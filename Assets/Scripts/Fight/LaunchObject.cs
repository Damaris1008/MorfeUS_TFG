using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchObject : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    bool isLaunching;

    [Header("Damage Attack")]
    public Item bow;

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
        yield return new WaitForSeconds(0.6f);
        rigidbody2d.AddForce(-direction * force);
        gameObject.SetActive(false);
        isLaunching = false;
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.gameObject.CompareTag("Enemy")){
                other.collider.SendMessage("Damage", bow.stats);
        }
        hitEffect.gameObject.transform.position = transform.position;
        hitEffect.Play();
        audioSource.PlayOneShot(impactSound);
        gameObject.SetActive(false);
        isLaunching = false;
    }
}
