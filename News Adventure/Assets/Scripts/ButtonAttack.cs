using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAttack : MonoBehaviour
{
    private static float timeBtwAttack;
    private static float startTimeBtwAttack = 0.1f;
    public GameObject projectileUp;
    public GameObject projectileDown;
    public GameObject projectileLeft;
    public GameObject projectileRight;
    public Transform ShotPoint;

    public Joystick joy;

    public void Start()
    {
       
    }
    public void CAC()
    {
        if (timeBtwAttack <= 0)
        {
            Debug.Log("CAC");
            timeBtwAttack = startTimeBtwAttack;
            timeBtwAttack -= Time.deltaTime;
        }
        else
        {
            Debug.Log("time : " + timeBtwAttack);
            timeBtwAttack -= Time.deltaTime;
        }
    }
    
    public void Mid()
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
    
    public void Range()
    {
        timeBtwAttack -= (Time.time - timeBtwAttack);
        Debug.Log("BTW : " + timeBtwAttack);
        if (timeBtwAttack < Time.time)
        {
            Transform PLAYER = GameObject.FindGameObjectWithTag("Player").transform;

            if (joy.Vertical > 0 && (Mathf.Abs(joy.Vertical) > Mathf.Abs(joy.Horizontal)))
            {
                Instantiate(projectileUp, ShotPoint.position, transform.rotation);
                PLAYER.GetComponent<Player>().setDrop(true);

            }
            else if (joy.Vertical < 0 && (Mathf.Abs(joy.Vertical) > Mathf.Abs(joy.Horizontal)))
            {
                Instantiate(projectileDown, ShotPoint.position, transform.rotation);
                PLAYER.GetComponent<Player>().setDrop(true);

            }
            else if (joy.Horizontal < 0 && (Mathf.Abs(joy.Horizontal) > Mathf.Abs(joy.Vertical)))
            {
                Instantiate(projectileRight, ShotPoint.position, transform.rotation);
                PLAYER.GetComponent<Player>().setDrop(true);

            }
            else
            {
                Instantiate(projectileLeft, ShotPoint.position, transform.rotation);
                PLAYER.GetComponent<Player>().setDrop(true);

            }
            timeBtwAttack = Time.time + startTimeBtwAttack;
        }
    }
}
