using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask WhatIsEnemies;
    public float attackRange;
    public int damage;

    private void Update()
    {
        if(timeBtwAttack <= 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, WhatIsEnemies);
                if(enemiesToDamage.Length >= 2) 
                {
                    for (int i = 1; i < enemiesToDamage.Length; i++)
                    {
                        enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                    }
                }
               
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }

       
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
