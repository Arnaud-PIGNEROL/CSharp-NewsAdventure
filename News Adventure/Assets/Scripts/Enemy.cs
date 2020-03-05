using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;

    private Transform target;
    private Animator animator;
    private float inverseMoveTime;

    public float moveTime = 0.1f;
    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider;
    public int speed;

    private void Start()
    {
        //GameManager.instance.brasiers.Add(this);
        //GameManager.instance.tornades.Add(this);
        GameManager.instance.enemy.Add(this);

        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;    
    }

    void Update()
    {
        /*  for (int i = 0; i < GameManager.instance.brasiers.Count; i++)
          {
              if (GameManager.instance.brasiers[i].health <= 0)
              {
                  Destroy(GameManager.instance.brasiers[i]);
              }
          }
             */

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Nom de lobjet attaqué : "+ this.name);/*
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
        bool same = false;
        int xDir = 0;
        int yDir = 0;

        for (int i = -2; i <= 2; i++) //check around
        {
            for (int j = -2; j <= 2; j++)
            {
                if ((target.position.x == transform.position.x + i) && (target.position.y == transform.position.y + j))
                {
                    same = true;
                }
            }
        }

        //if (same)
        if(false)
        {
            if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            {
                yDir = target.position.y > transform.position.y ? 1 : -1;

//               RaycastHit2D hit;
                bool canMove = Move(xDir, yDir);//, out hit);
                
                if (!canMove)
                {
                    Debug.Log("Cant moove Y, moove X (even Y is technicly better)");
                    yDir = 0;
                    xDir = target.position.x > transform.position.x ? 1 : -1;
                }
            }
            else
            {
                xDir = target.position.x > transform.position.x ? 1 : -1;

//                RaycastHit2D hit;
                bool canMove = Move(xDir, yDir);//, out hit);
                
                if (!canMove)
                {
                    Debug.Log("Cant moove X, moove Y (even X is technicly better)");
                    xDir = 0;
                    yDir = target.position.y > transform.position.y ? 1 : -1;
                }
            }

        }
        else
        {
            xDir = Random.Range(-4, 5);
            yDir = Random.Range(-4, 5);
            if (Mathf.Abs(xDir) >= Mathf.Abs(yDir))
                yDir = 0;
            else
                xDir = 0;
        }

        same = false;
        Move(xDir, yDir);
 //       AttemptMove<Player>(xDir, yDir);
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
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, speed * Time.deltaTime);
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
