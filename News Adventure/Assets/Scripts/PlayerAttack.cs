using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private float timeBtwAttack;
    private Transform PLAYER;

    public float startTimeBtwAttack;
    public LayerMask WhatIsEnemies;
    public GameObject projectileUp;
    public GameObject projectileDown;
    public GameObject projectileLeft;
    public GameObject projectileRight;
    public Transform ShotPoint;
    public Transform attackPosCacUp;
    public Transform attackPosCacDown;
    public Transform attackPosCacLeft;
    public Transform attackPosCacRight;


    public Transform attackPosRangeMid;
    public float distance;
    public float angle;
    public float attackRangeCac;
    public int damageCac;
    public int damageMid;
    Vector2 size = new Vector2(2f, 0.1f);

    private void Start()
    {
        anim = GetComponent<Animator>();
        PLAYER = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {

        if (timeBtwAttack <= 0)
        {
            if (Input.GetKey(KeyCode.N))
            {
                anim.SetTrigger("PlayerHit");
                if (PLAYER.GetComponent<Player>().getDirection() == "Up")
                {
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosCacUp.position, attackRangeCac, WhatIsEnemies);

                    FindObjectOfType<AudioManager>().Play("PlayerCac");
                    if (enemiesToDamage.Length >= 1)
                    {
                        for (int i = 0; i < enemiesToDamage.Length; i++)
                        {
                            enemiesToDamage[i].GetComponent<Enemy>().takeDamage(damageCac);
                            PLAYER.GetComponent<Player>().setDrop(true);
                        }
                    }
                }
                else if (PLAYER.GetComponent<Player>().getDirection() == "Down")
                {
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosCacDown.position, attackRangeCac, WhatIsEnemies);

                    FindObjectOfType<AudioManager>().Play("PlayerCac");
                    if (enemiesToDamage.Length >= 1)
                    {
                        for (int i = 0; i < enemiesToDamage.Length; i++)
                        {                
                            enemiesToDamage[i].GetComponent<Enemy>().takeDamage(damageCac);
                            PLAYER.GetComponent<Player>().setDrop(true);
                        }
                    }
                }
                else if (PLAYER.GetComponent<Player>().getDirection() == "Left")
                {
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosCacLeft.position, attackRangeCac, WhatIsEnemies);

                    FindObjectOfType<AudioManager>().Play("PlayerCac");
                    if (enemiesToDamage.Length >= 1)
                    {
                        for (int i = 0; i < enemiesToDamage.Length; i++)
                        {
                            enemiesToDamage[i].GetComponent<Enemy>().takeDamage(damageCac);
                            PLAYER.GetComponent<Player>().setDrop(true);
                        }
                    }
                }
                else
                {
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosCacRight.position, attackRangeCac, WhatIsEnemies);

                    FindObjectOfType<AudioManager>().Play("PlayerCac");
                    if (enemiesToDamage.Length >= 1)
                    {
                        for (int i = 0; i < enemiesToDamage.Length; i++)
                        {
                            enemiesToDamage[i].GetComponent<Enemy>().takeDamage(damageCac);
                            PLAYER.GetComponent<Player>().setDrop(true);
                        }
                    }
                }


            }
            else if (Input.GetKey(KeyCode.Space))
            {
                anim.SetTrigger("PlayerHit");
                Collider2D[] hitInfo = Physics2D.OverlapBoxAll(attackPosRangeMid.position, new Vector3(1.2f, 0.4f, 1), angle, WhatIsEnemies);

                FindObjectOfType<AudioManager>().Play("PlayerMid");
                if (hitInfo.Length >= 2)
                {
                    for (int i = 1; i < hitInfo.Length; i++)
                    {
                        hitInfo[i].GetComponent<Enemy>().takeDamage(damageMid);
                        PLAYER.GetComponent<Player>().setDrop(true);
                    }
                }

            }
            else if (Input.GetKey(KeyCode.Keypad8))
            {
                Instantiate(projectileUp, ShotPoint.position, transform.rotation);
                FindObjectOfType<AudioManager>().Play("PlayerRange");
                PLAYER.GetComponent<Player>().setDrop(true);

            }
            else if (Input.GetKey(KeyCode.Keypad5))
            {
                Instantiate(projectileDown, ShotPoint.position, transform.rotation);
                FindObjectOfType<AudioManager>().Play("PlayerRange");
                PLAYER.GetComponent<Player>().setDrop(true);

            }
            else if (Input.GetKey(KeyCode.Keypad6))
            {
                Instantiate(projectileRight, ShotPoint.position, transform.rotation);
                FindObjectOfType<AudioManager>().Play("PlayerRange");
                PLAYER.GetComponent<Player>().setDrop(true);

            }
            else if (Input.GetKey(KeyCode.Keypad4))
            {
                Instantiate(projectileLeft, ShotPoint.position, transform.rotation);
                FindObjectOfType<AudioManager>().Play("PlayerRange");
                PLAYER.GetComponent<Player>().setDrop(true);

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
        Gizmos.DrawWireSphere(attackPosCacUp.position, attackRangeCac);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosCacDown.position, attackRangeCac);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosCacRight.position, attackRangeCac);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosCacLeft.position, attackRangeCac);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackPosRangeMid.position, new Vector3(1.2f, 0.4f, 1));
    }
}
