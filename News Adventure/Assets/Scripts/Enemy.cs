using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider;
    private GameObject target;
    private Animator animator;
    private float inverseMoveTime;
    private bool onMoove;   // Is the enemy mooving
    private float time_next_move;

    public Transform attackPosCac;
    public Transform ShotPoint;
    public Transform attackPosRangeMid;
    public GameObject projectileUp;
    public GameObject projectileDown;
    public GameObject projectileLeft;
    public GameObject projectileRight;
    public LayerMask blockingLayer;  //is the space open (no collision?)
    public LayerMask playerLayer;  
    public int speed;
    public int detection_dist;
    public float moveTime;
    public int health;
    public int damage;
    public int ptsAtDeath;

    private void Start()
    {
        onMoove = false;
        time_next_move = 0;
        GameManager.instance.enemy.Add(this);

        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        inverseMoveTime = 1f / moveTime;    
    }

    void Update()
    {

    }

    public void takeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            target.GetComponent<Player>().addScore(ptsAtDeath); // we add points to the player
            int index = GameManager.instance.enemy.IndexOf(this);
            GameManager.instance.enemy.RemoveAt(index);
            Destroy(gameObject);
        }
    }


    public void MoveEnemy()
    {
        if (out_of_range() || Time.time < time_next_move)
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
                if ((transform.position.x <= target.transform.position.x + 0.05 && transform.position.x >= target.transform.position.x - 0.05) || (transform.position.y <= target.transform.position.y + 0.05 && transform.position.y >= target.transform.position.y - 0.05)) //to avoid infinite circular mouvement around the boxcollided
                    myVectors_target[0] = myVectors_target[1] = 0;
                else
                {
                    myVectors_target[0] = 0;
                    myVectors_target[1] = target.transform.position.x > transform.position.x ? 1 : -1;
                }
            }
        }
        else // the player isn't in range detection of the enemy
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

        if (onMoove)
        {
            if (Move(myVectors_target[0], myVectors_target[1]))
            {
                if(this.name=="Vent(Clone)")
                    time_next_move = Time.time + Random.Range(0, 2); //dist
                else if(this.name == "Braize(Clone)")
                    time_next_move = Time.time + Random.Range(1, 3); //cac
                else
                    time_next_move = Time.time + Random.Range(2, 4); //boss
                onMoove = false;
            }
        }
        player_targeted = false;
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
        hit = Physics2D.Linecast(this.transform.position, target.transform.position, playerLayer);
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
                if (((int)target.transform.position.x == (int)transform.position.x + i) && ((int)target.transform.position.y == (int)transform.position.y + j))
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
        Path[0] = target.transform.position.x > transform.position.x ? 1 : -1;
        Path[1] = target.transform.position.y > transform.position.y ? 1 : -1;

        RaycastHit2D hit;
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(this.transform.position, target.transform.position, playerLayer);
        boxCollider.enabled = true;

        if (hit.distance <= 1)
        {
            attack_cac();
            Path[0] = Path[1] = 0;
        }

        return Path;
    }

    private int[] ia_distance()
    {
        int[] Path = new int[2];

        RaycastHit2D hit;
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(this.transform.position, target.transform.position, playerLayer);
        boxCollider.enabled = true;
        
        if (hit.distance > 3)
            return ia_cac();
        else if (hit.distance < 3 && hit.distance > 2) // the Hypothénuse is >1.5 so the ennemi is safe, it can attack
        {
            attack_dist();
            Path[0] = 0;
            Path[1] = 0; 
        }
        else
        {
            //  1 | 2 | 3
            //  4 | E | 5
            //  6 | 7 | 8

            if ((transform.position.x - target.transform.position.x) < -0.5) // the player is in zone 3/5/8
            {
                if ((transform.position.y - target.transform.position.y) < -1) // the player is zone 3
                {
                    Path[0] = -2;
                    Path[1] = -2;
                }
                else if ((transform.position.y - target.transform.position.y) > -1 && (transform.position.y - target.transform.position.y) < 0) // zone 5
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
            else if ((transform.position.x - target.transform.position.x) > -0.5 && (transform.position.x - target.transform.position.x) < 0) // the player is in zone 2/7
            {
                if ((transform.position.y - target.transform.position.y) < -1) // the player is zone 2
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
                if ((transform.position.y - target.transform.position.y) < -1) // the player is zone 1
                {
                    Path[0] = 2;
                    Path[1] = -2;
                }
                else if ((transform.position.y - target.transform.position.y) > -1 && (transform.position.y - target.transform.position.y) < 0) // zone 4
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
            Path[1] = target.transform.position.y > transform.position.y ? 2 : -2;
        }
        else
        {
            Path[0] = target.transform.position.x > transform.position.x ? 2 : -2;
            Path[1] = 0;
        }

        return Path;
    }

    private void attack_cac()
    {
        Collider2D[] hitInfo = Physics2D.OverlapBoxAll(attackPosRangeMid.position, new Vector3(1.2f, 0.4f, 1), 0, playerLayer);
        if (hitInfo.Length >= 1)
            target.GetComponent<Player>().takeDamage(damage);

        Debug.Log("attaque cac");
        time_next_move = (Time.time + 1);
    }

    private void attack_dist()
    {
        
        Debug.Log("attaque dist");
        time_next_move = (Time.time + 1);
        if (true)
        {
            this.boxCollider.enabled = false;
            Instantiate(projectileUp, ShotPoint.position, transform.rotation);
            this.boxCollider.enabled = true;
        }
        else if (false)
        {
            Instantiate(projectileDown, ShotPoint.position, transform.rotation);

        }
        else if (false)
        {
            Instantiate(projectileRight, ShotPoint.position, transform.rotation);

        }
        else
        {
            Instantiate(projectileLeft, ShotPoint.position, transform.rotation);

        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackPosCac.position, new Vector3(1.2f, 0.4f, 1));
    }
}
