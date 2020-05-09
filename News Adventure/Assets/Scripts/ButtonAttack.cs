using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAttack : MonoBehaviour
{
    private static float timeBtwAttack;
    private static float startTimeBtwAttack = 0.1f;
    private Transform PLAYER;
    private Animator anim;

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

    public Joystick joy;
    public LayerMask WhatIsEnemies;

    public void Start()
    {
        PLAYER = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    public void CAC() // button attack cac
    {
        if (timeBtwAttack <= 0)
        {

            if (PLAYER.GetComponent<Player>().getDirection() == "Up") 
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosCacUp.position, attackRangeCac, WhatIsEnemies); // on recupere les ennemis touches par l'attaque

                if (enemiesToDamage.Length >= 1) // si on touche au moins 1 ennemis
                {
                    FindObjectOfType<AudioManager>().Play("PlayerCac"); // son de l'attaque
                    for (int i = 0; i < enemiesToDamage.Length; i++) // on applique les degats
                    {
                        enemiesToDamage[i].GetComponent<Enemy>().takeDamage(damageCac);
                        PLAYER.GetComponent<Player>().setDrop(true);
                    }
                }
            }
            else if (PLAYER.GetComponent<Player>().getDirection() == "Down")
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosCacDown.position, attackRangeCac, WhatIsEnemies);

                if (enemiesToDamage.Length >= 1)
                {
                    FindObjectOfType<AudioManager>().Play("PlayerCac");
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

                if (enemiesToDamage.Length >= 1)
                {
                    FindObjectOfType<AudioManager>().Play("PlayerCac");
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

                if (enemiesToDamage.Length >= 1)
                {
                    FindObjectOfType<AudioManager>().Play("PlayerCac");
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        enemiesToDamage[i].GetComponent<Enemy>().takeDamage(damageCac);
                        PLAYER.GetComponent<Player>().setDrop(true);
                    }
                }
            }

            timeBtwAttack = startTimeBtwAttack;
            timeBtwAttack -= Time.deltaTime;
        }
        else
        {
            Debug.Log("time : " + timeBtwAttack);
            timeBtwAttack -= Time.deltaTime;
        }
    }

    public void Mid() // button attack mid range
    {
        if (timeBtwAttack <= 0)
        {
            Debug.Log("Mid");
            timeBtwAttack = startTimeBtwAttack;
            timeBtwAttack -= Time.deltaTime;
        }
        else
        {
            Debug.Log("time : " + timeBtwAttack);
            timeBtwAttack -= Time.deltaTime;
        }
    }

    public void Range() // button attack range
    {
        timeBtwAttack -= (Time.time - timeBtwAttack);
        //Debug.Log("BTW : " + timeBtwAttack);
        if (timeBtwAttack < Time.time)
        {
            Transform PLAYER = GameObject.FindGameObjectWithTag("Player").transform;
            if (joy.Vertical > 0 && (Mathf.Abs(joy.Vertical) > Mathf.Abs(joy.Horizontal)))
            {
                Instantiate(projectileRight, ShotPoint.position, transform.rotation);
                Debug.Log("Right");

            }
            else if (joy.Vertical < 0 && (Mathf.Abs(joy.Vertical) > Mathf.Abs(joy.Horizontal)))
            {
                Instantiate(projectileLeft, ShotPoint.position, transform.rotation);
                Debug.Log("Left");

            }
            else if (joy.Horizontal < 0 && (Mathf.Abs(joy.Horizontal) > Mathf.Abs(joy.Vertical)))
            {
                Instantiate(projectileDown, ShotPoint.position, transform.rotation);
                Debug.Log("Down");

            }
            else
            {
                Instantiate(projectileUp, ShotPoint.position, transform.rotation);
                Debug.Log("Up");

            }
            PLAYER.GetComponent<Player>().setDrop(true);
            timeBtwAttack = Time.time + startTimeBtwAttack;
        }
    }
}
