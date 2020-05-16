using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private GameObject target;
    private float dashTime;
    private float timeBtwAttack;
    private int direction;

    public LayerMask playerLayer;
    public float dashSpeed;
    public float startDashTime;
    public float startTimeBtwAttack;
    public float maxRange_dash;
    public float minRrange_dash;


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        timeBtwAttack = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_usefull_to_dash() && timeBtwAttack <= 0)
        {
            float Dx = transform.position.x - target.transform.position.x;
            float Dy = transform.position.y - target.transform.position.y;
            float DDp = Dx + Dy;
            float DDm = Dx - Dy;

            if (direction == 0)
            {
                /*
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    direction = 1;
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    direction = 2;
                }
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    direction = 3;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    direction = 4;
                }
                */
                if (DDp < 0) // Up or right
                {
                    if (DDm > 0)
                        direction = 3; //haut
                    else
                        direction = 2; // Droite
                }
                else
                {
                    if (DDm > 0)
                        direction = 1; // Gauche
                    else
                        direction = 4; // Bas
                }
            }
            else
            {
                if (dashTime <= 0)
                {
                    direction = 0;
                    dashTime = startDashTime;
                    rb.velocity = Vector2.zero;
                    timeBtwAttack = startTimeBtwAttack;
                }
                else
                {
                    dashTime -= Time.time;
                    if (direction == 1)
                        rb.velocity = Vector2.left * dashSpeed;
                    else if (direction == 2)
                        rb.velocity = Vector2.right * dashSpeed;
                    else if (direction == 3)
                        rb.velocity = Vector2.up * dashSpeed;
                    else if (direction == 4)
                        rb.velocity = Vector2.down * dashSpeed;
                }
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }

    }

    private bool is_usefull_to_dash()
    {
        RaycastHit2D hit;
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(this.transform.position, target.transform.position, playerLayer);
        boxCollider.enabled = true;
        if (hit.distance <= maxRange_dash && hit.distance >= minRrange_dash)
            return true;

        return false;
    }
}