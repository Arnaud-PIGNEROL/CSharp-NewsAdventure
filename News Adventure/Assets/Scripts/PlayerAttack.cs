using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public GameObject projectile;
    public Transform ShotPoint;

    public Transform attackPosCac;
    public Transform attackPosRangeMid;
 

    Vector2 size = new Vector2(2f, 0.1f);

    public float distance;
    public float angle;
    

    public float attackRangeCac;



    public int damageCac;
    public int damageMid;

    private Animator anim;

    public LayerMask WhatIsEnemies;

    private void Start()
    {
        anim = GetComponent<Animator>();
        
    }
    private void Update()
    {
        if(timeBtwAttack <= 0)
        {
            if (Input.GetKey(KeyCode.N))
            {
                anim.SetTrigger("PlayerHit");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosCac.position, attackRangeCac, WhatIsEnemies);
                if (enemiesToDamage.Length >= 2)
                {
                    for (int i = 1; i < enemiesToDamage.Length; i++)
                    {
                        enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damageCac);
                       
                    }
                }

            }
            else if (Input.GetKey(KeyCode.Space))
            {
                anim.SetTrigger("PlayerHit");
                Collider2D[] hitInfo = Physics2D.OverlapBoxAll(attackPosRangeMid.position, new Vector3(1.2f, 0.4f, 1), angle, WhatIsEnemies);
                if (hitInfo.Length >= 2)
                {
                    for (int i = 1; i < hitInfo.Length; i++)
                    {
                        hitInfo[i].GetComponent<Enemy>().TakeDamage(damageMid);
                    }
                }

            }
            else if (Input.GetKey(KeyCode.B))
            {
                Instantiate(projectile, ShotPoint.position, transform.rotation);

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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackPosRangeMid.position, new Vector3(1.2f, 0.4f, 1));

    }
}
