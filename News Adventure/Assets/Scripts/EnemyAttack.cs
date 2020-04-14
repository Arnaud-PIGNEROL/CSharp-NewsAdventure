using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public GameObject projectileUp;
    public GameObject projectileDown;
    public GameObject projectileLeft;
    public GameObject projectileRight;

    public Transform ShotPoint;

    public Transform attackPosCac;


    public float distance;
    public float angle;
  

    public float attackRangeCac;
    public int damageCac;
    public LayerMask WhatIsEnemies;

    private void Start()
    {

      
    }
    private void Update()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKey(KeyCode.L))
            {
                
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosCac.position, attackRangeCac, WhatIsEnemies);
                if (enemiesToDamage.Length >= 2)
                {
                    
                        enemiesToDamage[0].GetComponent<Player>().takeDamage(damageCac);
                }

            }
           
            else if (Input.GetKey(KeyCode.Keypad8))
            {
                Instantiate(projectileUp, ShotPoint.position, transform.rotation);
                FindObjectOfType<AudioManager>().Play("FireAttack");

            }
            else if (Input.GetKey(KeyCode.Keypad5))
            {
                Instantiate(projectileDown, ShotPoint.position, transform.rotation);
                FindObjectOfType<AudioManager>().Play("FireAttack");

            }
            else if (Input.GetKey(KeyCode.Keypad6))
            {
                Instantiate(projectileRight, ShotPoint.position, transform.rotation);
                FindObjectOfType<AudioManager>().Play("FireAttack");

            }
            else if (Input.GetKey(KeyCode.Keypad4))
            {
                Instantiate(projectileLeft, ShotPoint.position, transform.rotation);
                FindObjectOfType<AudioManager>().Play("FireAttack");

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
        Gizmos.DrawWireSphere(attackPosCac.position, attackRangeCac);
 

    }
}
