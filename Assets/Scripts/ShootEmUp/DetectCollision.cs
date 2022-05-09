using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public bool destroyOnCollision = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ShouldCheckCollision(collision))
        {
            return;
        }

        if (destroyOnCollision)
        {
            Destroy(gameObject);
            Debug.Log($"Collision 2D between {gameObject.name} and {collision.name}: destroy {gameObject.name}");
        }

        Health health = GetComponent<Health>();
        if (health == null)
        {
            Debug.Log($"Collision 2D between {gameObject.name} and {collision.name}: {gameObject.name} does not have health");
            return;
        }

        Damage damage = collision.GetComponent<Damage>();
        if (damage == null)
        {
            Debug.Log($"Collision 2D between {gameObject.name} and {collision.name}: {collision.name} does not have damage");
            return;
        }

        int oldHP = health.current;
        int newHP = health.damage(damage.amount);
        Debug.Log($"Collision 2D between {gameObject.name} and {collision.name}: {collision.name} does {damage.amount} to {gameObject.name} ({oldHP} HP -> {newHP} HP)");
        damage.amount = 0;
    }

    private bool ShouldCheckCollision(Collider2D collision)
    {
        if (gameObject.CompareTag("Player"))
        {
            return collision.CompareTag("Enemy");
        }

        if (gameObject.CompareTag("Enemy"))
        {
            return collision.CompareTag("Player");
        }

        return false;
    }
}
