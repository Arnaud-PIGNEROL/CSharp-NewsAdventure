using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public int MaxHealth;
    public int currentHealth;

   public HealthBar healthBar;
    
    private void Start()
    {
        currentHealth = MaxHealth;
        healthBar.SetMaxHealth(currentHealth);

    }
    void Update()
    {
   
        if (currentHealth <= 0)
        {
            Destroy(gameObject);

        }
    }
    public void TakeDamage(int damage)
    {

        currentHealth -= damage;

       healthBar.SetHealth(currentHealth);
        

        Debug.Log("damage taken"+damage+"     pv ennemis"+ currentHealth);
    }
}
