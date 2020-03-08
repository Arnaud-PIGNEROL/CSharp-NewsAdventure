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

    public float moveTime = 0.1f;
    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider;
    public int speed;

    private void Start()
    {
        //GameManager.instance.brasiers.Add(this);
        //GameManager.instance.tornades.Add(this); 
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
        Debug.Log("Nom de lobjet attaqué : "+ this.name);
        /*
        if (health <= 0)
        {
            if (string.Compare(this.name, "Braize(Clone)") == 0)
            {
                Debug.Log("C'est un brasier");
                int index = GameManager.instance.brasiers.IndexOf(this);
                GameManager.instance.brasiers.RemoveAt(index);

            }
            else if (string.Compare(this.name, "Vent(Clone)") == 0)
            {
                Debug.Log("c'est une tornade");
                int index = GameManager.instance.tornades.IndexOf(this);
                GameManager.instance.tornades.RemoveAt(index);
            }
            else
            {
                Debug.Log("euuu problème car enemis pas reconnu");
            }

            Destroy(gameObject);
        }
        */
        if (health <= 0)
        {
            int index = GameManager.instance.enemy.IndexOf(this);
            GameManager.instance.enemy.RemoveAt(index);
            Destroy(gameObject);
        }
    }


    public void MoveEnemy()
    {
        int xMoove = 0; // vector of direction X
        int yMoove = 0; // vector of direction Y
        bool player_targeted = false;

        for (int i = -2; i <= 2; i++) //check around
        {
            for (int j = -2; j <= 2; j++)
            {
                if (((int)target.position.x == (int)transform.position.x + i) && ((int)target.position.y == (int)transform.position.y + j))
                {
                    player_targeted = true;
                    onMoove = true;
                }                    
            }
        }

        if (player_targeted)
        {
            
            xMoove = target.position.x > transform.position.x ? 1 : -1;
            yMoove = target.position.y > transform.position.y ? 1 : -1;

            /*
            // RaycastHit2D hit;
            bool canMove = Move(xMoove, yMoove);//, out hit);
                
            if (!canMove)
            {
                Debug.Log("Cant moove Y, moove X (even Y is technicly better)");
                yMoove = 0;
                xMoove = target.position.x > transform.position.x ? 1 : -1;
            }*/
            
            X_end = xMoove;
            Y_end = yMoove;
        }
        else // the enemy isn't in range detection
        {
            if (!onMoove) //if enemy is at his final place
            {
                if (Time.time >= time_next_move) // time to wait bewteen 2 moves
                {
                    onMoove = true;
                    time_next_move = 0;

                    xMoove = Random.Range(-1, 2);
                    yMoove = Random.Range(-1, 2);

                    X_end = (int)transform.position.x + xMoove;
                    Y_end = (int)transform.position.y + yMoove;

                }                    
            }
            else // if enemy hasn't reach his final place yet
            {
                xMoove = X_end - (int)transform.position.x; // to transform the point into a vector
                yMoove = Y_end - (int)transform.position.y; // for ex : we're at X=12 and we want to be at X=15. So we need to make a 15-12= +3X vector
            }
            
            if ((int)this.transform.position.x == X_end || (int)this.transform.position.y == Y_end) //check if we're arrived
            {                
                if (onMoove) // if its the first frame since the enemy has reach the final point
                    time_next_move = Time.time + Random.Range(1, 3); //we wait bewteen 1s and 3s before to start a new move
                    
                onMoove = false;
            }
        }

        if(onMoove)
            Move(xMoove, yMoove);

        onMoove = false;
        player_targeted = false;
        //     AttemptMove<Player>(xDir, yDir);
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
}
