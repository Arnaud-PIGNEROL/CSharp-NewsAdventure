using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    public Transform player;
    public float speed = 5.0f;
    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;
        
    public Transform circle;
    public Transform outerCircle;

    void Start()
    {
        pointA = new Vector2(circle.transform.position.x, circle.transform.position.y);
        pointB = pointA;
        //pointA = Camera.main.ScreenToWorldPoint(new Vector3(circle.transform.position.x, circle.transform.position.y, Camera.main.transform.position.z));
        circle.transform.position = pointA;
        outerCircle.transform.position = pointA;
        circle.GetComponent<SpriteRenderer>().enabled = true;
        outerCircle.GetComponent<SpriteRenderer>().enabled = true;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            touchStart = true;
            pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        }
        else
        {
            pointB = pointA;
            touchStart = false;
        }

    }

    private void FixedUpdate()
    {
        if (touchStart)
        {
            Vector2 direction = Vector2.ClampMagnitude(pointB - pointA, 1.0f);
            moveCharacter(direction);

            circle.transform.position = new Vector2(pointA.x + direction.x, pointA.y + direction.y);
        }
        else
        {
            circle.transform.position = new Vector2(pointA.x, pointA.y);
        }
    }

    void moveCharacter(Vector2 direction)
    {
        player.Translate(direction * speed * Time.deltaTime);
    }
}