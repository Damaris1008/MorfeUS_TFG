using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    bool isLaunching;

    [Header("Damage Attack")]
    public float damageAttack = 2.0f; //half heart

    [Header("Particles")]
    public ParticleSystem punchHitEffect;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip punchSound;

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
            StartCoroutine(DeactivatePunch(direction,force));
        }
    }

    private IEnumerator DeactivatePunch(Vector2 direction, float force)
    {        
        yield return new WaitForSeconds(0.4f);
        rigidbody2d.AddForce(-direction * force);
        gameObject.SetActive(false);
        isLaunching = false;
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Punch Collision with " + other.gameObject);
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        punchHitEffect.gameObject.transform.position = transform.position;
        punchHitEffect.Play();
        enemy.Damage(damageAttack);
        
        audioSource.PlayOneShot(punchSound);
        gameObject.SetActive(false);
        isLaunching = false;
    }
}
