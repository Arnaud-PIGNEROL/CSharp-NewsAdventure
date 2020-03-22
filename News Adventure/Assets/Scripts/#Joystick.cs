/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    public Transform player;
    public float speed = 5.0f;
    public float touchArea = 2.6f;
    public float button = 1.0f;
    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;

    private Vector2 pointCAC;
    private Vector2 pointMid;
    private Vector2 pointRange;
        
    public RectTransform circle;
    public RectTransform outerCircle;

    public RectTransform attackCAC;
    public RectTransform attackMid;
    public RectTransform attackRange;

    private float timeBtwAttack;
    public float startTimeBtwAttack;

    void Start()
    {
        //Crée un vecteur entre la position du cercle interne et du cercle externe
        pointA = new Vector2(circle.transform.position.x, circle.transform.position.y);
        Debug.Log("Position of a" + pointA.x + " " + pointA.y);
        pointB = pointA;
        
        circle.transform.position = pointA;
        outerCircle.transform.position = pointA;

        circle.GetComponent<SpriteRenderer>().enabled = true;
        outerCircle.GetComponent<SpriteRenderer>().enabled = true;
        Debug.Log("Out of position of a");

        /*pointCAC = new Vector2(attackCAC.transform.position.x, attackCAC.transform.position.y);
        pointMid = new Vector2(attackMid.transform.position.x, attackMid.transform.position.y);
        pointRange = new Vector2(attackRange.transform.position.x, attackRange.transform.position.y);

        attackCAC.transform.position = pointCAC;
        attackMid.transform.position = pointMid;
        attackRange.transform.position = pointRange;

        attackCAC.GetComponent<SpriteRenderer>().enabled = true;
        attackMid.GetComponent<SpriteRenderer>().enabled = true;
        attackRange.GetComponent<SpriteRenderer>().enabled = true;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Here !");
            pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            Debug.Log("The point B is : " + pointB);
            if ((pointB - pointCAC).sqrMagnitude <= button)             // cac attack
            {
                if (timeBtwAttack <= 0)
                {
                    Debug.Log("cac");
                    timeBtwAttack = startTimeBtwAttack;
                }
                else
                {
                    Debug.Log("time : " + timeBtwAttack);
                    timeBtwAttack -= Time.deltaTime;
                }
            }
            else if ((pointB - pointMid).sqrMagnitude <= button)        // mid range attack
            {
                if (timeBtwAttack <= 0)
                {
                    Debug.Log("mid");
                    timeBtwAttack = startTimeBtwAttack;
                }
                else
                {
                    Debug.Log("time : " + timeBtwAttack);
                    timeBtwAttack -= Time.deltaTime;
                }
            }
            else if ((pointB - pointRange).sqrMagnitude <= button)      // long range attack
            {
                if (timeBtwAttack <= 0)
                {
                    Debug.Log("range");
                    timeBtwAttack = startTimeBtwAttack;
                }
                else
                {
                    Debug.Log("time : " + timeBtwAttack);
                    timeBtwAttack -= Time.deltaTime;
                }
            }
            else if ((pointB - pointA).sqrMagnitude <= touchArea)     //float Vector2.sqrMagnitude
            {
                touchStart = true;
            }
            else
            {
                pointB = pointA;
                touchStart = false;
            }
        }
        else
        {
            Debug.Log("Hellow");
            pointB = pointA;
            touchStart = false;
        }
    }

    private void FixedUpdate()
    { 
        if (touchStart)
        {
            Debug.Log("Wtf");
            Vector2 direction = Vector2.ClampMagnitude(pointB - pointA, 1.0f);  // empêche le boutton de quitter le cercle
            moveCharacter(direction);
            circle.transform.position = new Vector2(pointA.x + direction.x, pointA.y + direction.x);
            pointA = new Vector2(pointA.x + player.position.x, pointA.y + player.position.y);
        }
        else
        {
            Debug.Log("inside this");
            circle.transform.position = new Vector2(pointA.x, pointA.y);
            Debug.Log("position " + pointA.x + " " + pointA.y);
        }
    }

    void moveCharacter(Vector2 direction)
    {
        player.Translate(direction * speed * Time.deltaTime);
    }
}*/