﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider;
    private Transform target;
    private Animator animator;
    private float inverseMoveTime;
    private bool onMoove;   // Is the enemy mooving
    private float time_next_move;

    public LayerMask blockingLayer;  //is the space open (no collision?)
    public LayerMask playerLayer;  
    public int speed;
    public int detection_dist;
    public float moveTime;
    public int health;

    private void Start()
    {
        onMoove = false;
        time_next_move = 0;
        GameManager.instance.enemy.Add(this);

        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        inverseMoveTime = 1f / moveTime;    
    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            int index = GameManager.instance.enemy.IndexOf(this);
            GameManager.instance.enemy.RemoveAt(index);
            Destroy(gameObject);
        }
    }


    public void MoveEnemy()
    {
        if (out_of_range())
            return;

        bool player_targeted = false;
        int[] myVectors_target = new int[2]; // 0 = xDir, 1 = yDir

        if (player_around())
        {
            player_targeted = true;
            onMoove = true;
        }

        if (player_targeted)
        {
            if (this.name == "Braize(Clone)")
                myVectors_target = ia_cac();
            else if (this.name == "Vent(Clone)")
                myVectors_target = ia_distance();
            else if (this.name == "Boss(Clone)")
                myVectors_target = ia_boss();

            bool canMove = Move(myVectors_target[0], myVectors_target[1]);
            
            if (!canMove)
            {
                if((transform.position.x <= target.position.x + 0.05 && transform.position.x >= target.position.x - 0.05) || (transform.position.y <= target.position.y + 0.05 && transform.position.y >= target.position.y - 0.05)) //to avoid infinite circular mouvement around the boxcollided
                    myVectors_target[0] = myVectors_target[1] = 0;
                else
                {
                    myVectors_target[0] = 0;
                    myVectors_target[1] = target.position.x > transform.position.x ? 1 : -1;
                }
            }
        }
        else // the player isn't in range detection of the enemy
        {            
            if (Time.time >= time_next_move) // time to wait bewteen 2 moves
            {
                onMoove = true;
                time_next_move = 0;

                if (this.name == "Boss(Clone)")
                {
                    myVectors_target = ia_boss_stroll();
                }
                else
                {
                    myVectors_target[0] = Random.Range(-1, 2);
                    myVectors_target[1] = Random.Range(-1, 2);
                }
            }    
        }

        if (onMoove)
        {
            if (Move(myVectors_target[0], myVectors_target[1]))
            {
                time_next_move = Time.time + Random.Range(2, 6); //we wait bewteen 1s and 3s before to start a new move
                onMoove = false;
            }
        }
        
        player_targeted = false;
        // AttemptMove<Player>(xDir, yDir);
    }

    protected bool Move(int xDir, int yDir)
    {
        //Store start position to move from, based on objects current transform position.
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        RaycastHit2D hitWall, hitPlayer;

        boxCollider.enabled = false;
        hitWall = Physics2D.Linecast(start, end, blockingLayer);
        hitPlayer = Physics2D.Linecast(start, end, playerLayer);
        boxCollider.enabled = true;

        //Check if anything was hit
        if (hitWall.transform == null && hitPlayer.transform == null)
        {
           StartCoroutine(SmoothMovement(end));
           return true;
        }
        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, speed * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    private bool out_of_range()
    {
        RaycastHit2D hit;
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(this.transform.position, target.position, playerLayer);
        boxCollider.enabled = true;

        if (hit.distance > 20)
            return true;
        
        return false;
    }

    private bool player_around()
    {
        for (int i = -detection_dist; i <= detection_dist; i++) //check if the player is in range around the enemy
        {
            for (int j = -detection_dist; j <= detection_dist; j++)
            {
                if (((int)target.position.x == (int)transform.position.x + i) && ((int)target.position.y == (int)transform.position.y + j))
                {
                    return true;
                }
            }
        }

        return false;
    }


    private int[] ia_cac()
    {
        int[] Path = new int[2];
        Path[0] = target.position.x > transform.position.x ? 1 : -1;
        Path[1] = target.position.y > transform.position.y ? 1 : -1;

        RaycastHit2D hit;
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(this.transform.position, target.position, playerLayer);
        boxCollider.enabled = true;

        if (hit.distance <= 0.3)
        {
            Path[0] = Path[1] = 0;
            //attaque cac IA
        }

        return Path;
    }
    private int[] ia_distance()
    {
        int[] Path = new int[2];

        RaycastHit2D hit;
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(this.transform.position, target.position, playerLayer);
        boxCollider.enabled = true;
        
        if (hit.distance > 3)
            return ia_cac();
        else if (hit.distance < 3 && hit.distance > 2) // the Hypothénuse is >1.5 so the ennemi is safe, it can attack
        {
            //attaque ia distance
            Path[0] = 0;
            Path[1] = 0; 
        }
        else
        {
            //  1 | 2 | 3
            //  4 | E | 5
            //  6 | 7 | 8

            if ((transform.position.x - target.position.x) < -0.5) // the player is in zone 3/5/8
            {
                if ((transform.position.y - target.position.y) < -1) // the player is zone 3
                {
                    Path[0] = -2;
                    Path[1] = -2;
                }
                else if ((transform.position.y - target.position.y) > -1 && (transform.position.y - target.position.y) < 0) // zone 5
                {
                    Path[0] = -2;
                    Path[1] = 0;
                }
                else // zone 8
                {
                    Path[0] = -2;
                    Path[1] = 2;
                }
            }
            else if ((transform.position.x - target.position.x) > -0.5 && (transform.position.x - target.position.x) < 0) // the player is in zone 2/7
            {
                if ((transform.position.y - target.position.y) < -1) // the player is zone 2
                {
                    Path[0] = 0;
                    Path[1] = -2;
                }
                else // zone 7
                {
                    Path[0] = 0;
                    Path[1] = 2;
                }
            }
            else // the player is in zone 1/4/6
            {
                if ((transform.position.y - target.position.y) < -1) // the player is zone 1
                {
                    Path[0] = 2;
                    Path[1] = -2;
                }
                else if ((transform.position.y - target.position.y) > -1 && (transform.position.y - target.position.y) < 0) // zone 4
                {
                    Path[0] = 2;
                    Path[1] = 0;
                }
                else // zone 6
                {
                    Path[0] = 2;
                    Path[1] = 2;
                }
            }
        }

        return Path;
    }

    private int[] ia_boss()
    {
        int[] Path = new int[2];

        int attack = Random.Range(0, 200);

        if(attack == 0)
        {
            ;
        }
        else if(attack < 101 && attack > 0)
        {
            // attaque distance avec projectile
        }
        else
        {
            return ia_cac();
        }

        return Path;
    }

    private int[] ia_boss_stroll()
    {
        int[] Path = new int[2];

        int mvmnt = Random.Range(0, 2);

        if (mvmnt == 1)
        {
            Path[0] = 0;
            Path[1] = target.position.y > transform.position.y ? 2 : -2;
        }
        else
        {
            Path[0] = target.position.x > transform.position.x ? 2 : -2;
            Path[1] = 0;
        }

        return Path;
    }
}