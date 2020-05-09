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
    public void CAC() // button attack cac
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
<<<<<<< HEAD
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
=======
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
>>>>>>> 8f834dc1e7eb8a4b7fc59468b123408f1cc6a3b4
        }
    }
}
