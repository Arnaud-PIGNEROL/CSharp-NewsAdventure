using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;

    private Transform target;
    private Animator animator;
    private float inverseMoveTime;
    private bool onMoove;   // Is the enemy mooving
    private int X_end;      // X coord of the point to reach
    private int Y_end;      // Y coord of the point to reach
    private float time_next_move;

    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider;
    public int speed;
    public int detection_dist;
    public float moveTime;

    private void Start()
    {
        onMoove = false;
        time_next_move = 0;
        GameManager.instance.enemy.Add(this);

        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb2D = GetComponent<Rigidbody2D>();
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

        int xMoove = 0; // vector of direction X
        int yMoove = 0; // vector of direction Y
        bool player_targeted = false;
        int[] myVectors_target = new int[2];

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

            /*
            // RaycastHit2D hit;
            bool canMove = Move(xMoove, yMoove);//, out hit);
                
            if (!canMove)
            {
                Debug.Log("Cant moove Y, moove X (even Y is technicly better)");
                yMoove = 0;
                xMoove = target.position.x > transform.position.x ? 1 : -1;
            }*/

            X_end = xMoove = myVectors_target[0];
            Y_end = yMoove = myVectors_target[1];
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

                    xMoove = myVectors_target[0];
                    yMoove = myVectors_target[1];
                }
                else
                {
                    xMoove = Random.Range(-1, 2);
                    yMoove = Random.Range(-1, 2);
                }

                X_end = (int)transform.position.x + xMoove;
                Y_end = (int)transform.position.y + yMoove;
            }    
        }

        if (onMoove)
        {
            if (Move(xMoove, yMoove))
            {
                time_next_move = Time.time + Random.Range(2, 6); //we wait bewteen 1s and 3s before to start a new move
                onMoove = false;
            }
        }
        
        player_targeted = false;
        // AttemptMove<Player>(xDir, yDir);
    }
/*
    protected virtual bool TryMove<T>(int xDir, int yDir)
     where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir)//, out hit);

        if (hit.transform == null)
            return true;

        Debug.Log("hitComponent + ctmv = " + canMove);

        T hitComponent = hit.transform.GetComponent<T>();

        if (canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
            Debug.Log("cant moove + hitComponent");
            return false;
        }

        return true;
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir)    // if ennemies -> player, if player -> walls
        where T : Component                                      // where to specify that T is a component but we don"t know yet is its a player or ennemy
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir)//, out hit);

        if (hit.transform == null)
            return;

        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent);
    }
*/

    protected bool Move(int xDir, int yDir/*, out RaycastHit2D hit*/)
    {
        //Store start position to move from, based on objects current transform position.
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

 //       boxCollider.enabled = false;
 //       hit = Physics2D.Linecast(start, end, blockingLayer);
 //       boxCollider.enabled = true;

        //Check if anything was hit
 //       if (hit.transform == null)
 //       {
            StartCoroutine(SmoothMovement(end));
            return true;
 //       }
 //       return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, speed * 2 * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }
/*
    protected void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        animator.SetTrigger("enemyAttack");
        hitPlayer.LoseFood(playerDamage);
    }
*/

    private bool out_of_range()
    {
        if (Mathf.Sqrt((Mathf.Abs(transform.position.x) - Mathf.Abs(target.position.x)) * (Mathf.Abs(transform.position.x) - Mathf.Abs(target.position.x)) + (Mathf.Abs(transform.position.y) - Mathf.Abs(target.position.y)) * (Mathf.Abs(transform.position.y) - Mathf.Abs(target.position.y))) > 20)
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

        if((Mathf.Abs(target.position.x - transform.position.x) <= 0.05) && (Mathf.Abs(target.position.y - transform.position.y) <= 0.05))
        {
            Path[0] = Path[1] = 0;
            //attaque cac IA
        }

        return Path;
    }
    private int[] ia_distance()
    {
        int[] Path = new int[2];

        // Xe = enemy posX --- Yj = player posY
        // sqrt( (Xe-Xj)² + (Ye-Yj)² )
        float dist = Mathf.Sqrt((Mathf.Abs(transform.position.x) - Mathf.Abs(target.position.x)) * (Mathf.Abs(transform.position.x) - Mathf.Abs(target.position.x)) + (Mathf.Abs(transform.position.y) - Mathf.Abs(target.position.y)) * (Mathf.Abs(transform.position.y) - Mathf.Abs(target.position.y)));
        if (dist > 3)
            return ia_cac();
        else if ( dist < 3 && dist > 2) // the Hypothénuse is >1.5 so the ennemi is safe, it can attack
        {
            //attaque ia distance
            Path[0] = 0;
            Path[1] = 0;
        }
        else
        {
            if (transform.position.x - target.position.x < 0) // the player is on the right compared to the enemy
            {
                if (transform.position.y - target.position.y < 0) // the player is above the enemy
                {
                    Path[0] = -2;
                    Path[1] = 2;
                }
                else // the player is under the enemy
                {
                    Path[0] = -2;
                    Path[1] = 2;
                }
            }
            else // the player is on the left compared to the enemy
            {
                if (transform.position.y - target.position.y < 0) // the player is above the enemy
                {
                    Path[0] = 2;
                    Path[1] = -2;
                }
                else // the player is under the enemy
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
            Debug.Log("deplacement légendaire");
        }
        else if(attack < 101 && attack > 0)
        {
            // attaque distance avec projectile
        }
        else
        {
            return ia_cac();
        }

        if ((Mathf.Abs(target.position.x - transform.position.x) <= 0.05) && (Mathf.Abs(target.position.y - transform.position.y) <= 0.05))
        {
            Path[0] = Path[1] = 0;
            //attaque cac IA boss
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
