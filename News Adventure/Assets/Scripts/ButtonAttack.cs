using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAttack : MonoBehaviour
{
    private static float timeBtwAttack;
    private static float startTimeBtwAttack = 0.1f;


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
        if (timeBtwAttack <= 0)
        {
            Debug.Log("Range");
            timeBtwAttack = startTimeBtwAttack;
            timeBtwAttack -= Time.deltaTime;
        }
        else
        {
            Debug.Log("time : " + timeBtwAttack);
            timeBtwAttack -= Time.deltaTime;
        }
    }
}
