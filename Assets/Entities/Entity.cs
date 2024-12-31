using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageTaker
{
    private Rigidbody2D rb;

    protected float hp = 500;
    protected float mass = 1;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Probably a better formula for this out there
        if(mass == 0) rb.velocity = rb.velocity / Mathf.Abs(mass);
    }
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if(hp < 0)
        {
            hp = 0;
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        Destroy(this);
    }
}
