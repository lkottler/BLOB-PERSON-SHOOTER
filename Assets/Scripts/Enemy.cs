using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public float health;
    public float speed = 10f;
    private Vector3 bunkerPosition;

    void Start()
    {
        bunkerPosition = GameObject.Find("Maiden").transform.position;
        health = 10;
    }

    void Update()
    {
        Vector3 dir = bunkerPosition - transform.position;
        transform.Translate(dir.normalized*speed*Time.deltaTime);

        if(health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
