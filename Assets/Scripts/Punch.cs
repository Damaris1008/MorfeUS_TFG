using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public ParticleSystem punchHitEffect;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject != null)
        {
            Destroy(gameObject, 0.4f);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Punch Collision with " + other.gameObject);
        Instantiate(punchHitEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
