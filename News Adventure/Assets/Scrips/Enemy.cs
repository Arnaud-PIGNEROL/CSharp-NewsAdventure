﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public int health;

    private void Start()
    {
        

    }
    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);

        }
    }
    public void TakeDamage(int damage)
    {
        health -=damage;

        Debug.Log("damage taken"+damage+"     pv ennemis"+health);
    }
}
