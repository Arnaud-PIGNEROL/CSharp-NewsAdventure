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
                Debug.Log("CAC up");
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
                Debug.Log("CAC down");
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
                Debug.Log("CAC left");
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
                Debug.Log("CAC right");
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
        timeBtwAttack -= (Time.time - timeBtwAttack); // to set a minimal time bewteen 2 attacks
        Debug.Log("BTW : " + timeBtwAttack);
        if (timeBtwAttack < Time.time) // if the time between the old attack and the new attack isn't too short
        {
            Transform PLAYER = GameObject.FindGameObjectWithTag("Player").transform;


            //if the joystick is upwards and if the joystick is more trought the vertical axis than trought the horizontal axis 
            // because the joystick can be on y=0.1 & x=0.9 so the projectile has to be to the right, even if y>0 (we consider y axis as vertical)
            if (joy.Vertical > 0 && (Mathf.Abs(joy.Vertical) > Mathf.Abs(joy.Horizontal))) // projectile up
            {
                Instantiate(projectileUp, ShotPoint.position, transform.rotation);
            }
            else if (joy.Vertical < 0 && (Mathf.Abs(joy.Vertical) > Mathf.Abs(joy.Horizontal))) // projectile down
            {
                Instantiate(projectileDown, ShotPoint.position, transform.rotation);
            }
            else if (joy.Horizontal < 0 && (Mathf.Abs(joy.Horizontal) > Mathf.Abs(joy.Vertical))) // projectile right
            {
                Instantiate(projectileRight, ShotPoint.position, transform.rotation);
            }
            else
            {
                Instantiate(projectileLeft, ShotPoint.position, transform.rotation);  // projectile left
            }
            PLAYER.GetComponent<Player>().setDrop(true); // if we attack, the animal is droped
            timeBtwAttack = Time.time + startTimeBtwAttack; // init a timer to wait before making a new attack
        }
    }
}
