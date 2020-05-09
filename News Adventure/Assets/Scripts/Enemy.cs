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
    public float shootingError; // the number of cell in (X and Y difference) that the ia can ignore before shooting, like the dispertion

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
        FindObjectOfType<AudioManager>().Play("FireHit");
        Debug.Log("health"+ health);
        if (health <= 0)
        {
            FindObjectOfType<AudioManager>().Play("FireDeath");
            target.GetComponent<Player>().addScore(ptsAtDeath); // we add points to the player
            int index = GameManager.instance.enemy.IndexOf(this);
            GameManager.instance.enemy.RemoveAt(index);
            Destroy(gameObject);

            if (this.name == "Boss(Clone)" && this.health <= 0 && FindObjectOfType<GameMan>().victory.activeInHierarchy == false && FindObjectOfType<GameMan>().defeat.activeInHierarchy == false)
            {
                FindObjectOfType<GameMan>().Win();
            }

        }
    }


    public void MoveEnemy()
    {
        if(FindObjectOfType<GameMan>().end == false)
        {
            /*
            animator.SetFloat("xAxis", 0);
            animator.SetFloat("yAxis", 0);
            animator.SetFloat("Magnitude", 0);*/
            if (out_of_range() || Time.time < time_next_move)
                return;

            bool player_targeted = false, moved = false;
            int[] myVectors_target = new int[2]; // 0 = xDir, 1 = yDir

            if (player_around()) // if the player is in range detection
            {
                player_targeted = true;
                onMoove = true;
            }

            if (player_targeted) 
            {
                // each AI has its proprer way of thinking
                if (this.name == "Braize(Clone)")
                    myVectors_target = ia_cac();
                else if (this.name == "Vent(Clone)")
                    myVectors_target = ia_distance();
                else if (this.name == "Boss(Clone)")
                    myVectors_target = ia_boss();

                moved = Move(myVectors_target[0], myVectors_target[1]); // we take the supposed moved

                if (!moved) // if the check moove says that we can't moove for any reason
                {
                    if ((transform.position.x <= target.transform.position.x + 0.05 && transform.position.x >= target.transform.position.x - 0.05) || (transform.position.y <= target.transform.position.y + 0.05 && transform.position.y >= target.transform.position.y - 0.05)) //to avoid infinite circular mouvement around the boxcollided
                        myVectors_target[0] = myVectors_target[1] = 0;
                    else // because in move we try the X direction first, here we try Y
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
                // we moove randomly
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

            if (onMoove && !moved) // if the AI has finished its move and if it couldn't moove
            {
                if (Move(myVectors_target[0], myVectors_target[1]))
                {
                    if (this.name == "Vent(Clone)")
                        time_next_move = Time.time + Random.Range(0, 2); //dist
                    else if (this.name == "Braize(Clone)")
                        time_next_move = Time.time + Random.Range(1, 3); //cac
                    else
                        time_next_move = Time.time + Random.Range(2, 4); //boss
                    onMoove = false;
                }
            }
            player_targeted = false;
        }
    }

    protected bool Move(int xDir, int yDir)
    {
        //Store start position to move from, based on objects current transform position.
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        /*
        animator.SetFloat("xAxis", xDir);
        animator.SetFloat("yAxis", yDir);
        animator.SetFloat("Magnitude", end.magnitude);*/
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

    protected IEnumerator SmoothMovement(Vector3 end) // in order to make the AI going forward step by step and not by +50 pixel in one step
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

    private bool out_of_range() // check if it is interesting to call the Ai function of the AI is too far from the player
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

        RaycastHit2D hit;
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(this.transform.position, target.transform.position, playerLayer);
        boxCollider.enabled = true;  
        if (hit.distance <= 0.8)
        {
            attack_cac();
            Path[0] = Path[1] = 0;
        }
        else
        {
            Path = ia_Escape(); // because run down is the inverse of run out
            Path[0] = (-Path[0])/2; // we /2 because we don't want that the ia CAC goes too fast
            Path[1] = (-Path[1])/2;

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

        float Dx = transform.position.x - target.transform.position.x;
        float Dy = transform.position.y - target.transform.position.y;

        if (((Dx > -shootingError && Dx < shootingError) || (Dy > -shootingError && Dy < shootingError)) && hit.distance > 1.5) // the distance is >2 so the ennemi is safe, it can attack
        {
            attack_dist();
            Path[0] = 0;
            Path[1] = 0;
        }
        else if (hit.distance > 1.5) // if the distance ai is a bit too far from the player
        {
            return iaDist_MinimizeGap(Dx, Dy);
        }
        else // if the ai is too close from the player
        {
            return ia_Escape();
        }
        return Path;
    }

    private int[] ia_Escape()
    {
        int[] Path = new int[2];

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
        return Path;
    }

    private int[] iaDist_MinimizeGap(float Dx, float Dy)
    {
        int[] Path = new int[2];

        if (Mathf.Abs(Dx) < Mathf.Abs(Dy)) // if the shortest distance (so the one to be minimized) is Dx
        {
            if (Dx > 0) // now check if the player is on the right in regards to the enemy
            {
                Path[0] = -1;
                Path[1] = 0;
            }
            else
            {
                Path[0] = 1;
                Path[1] = 0;
            }
        }
        else
        {
            if (Dy > 0)
            {
                Path[0] = 0;
                Path[1] = -1;
            }
            else
            {
                Path[0] = 0;
                Path[1] = 1;
            }
        }
        time_next_move = Time.time + 1;

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
        FindObjectOfType<AudioManager>().Play("FireAttack");
        if (hitInfo.Length >= 1)
            target.GetComponent<Player>().takeDamage(damage);

        time_next_move = (Time.time + 1);
    }

    private void attack_dist()
    {

        // the zone where DDm<0 and DDp<0 is the zone on the top of the ia (the player is on the top compare to the ia)
        // the zone where DDm>0 and DDp<0 is the zone on the right of the ia (the player is on the right compare to the ia)
        // etc...

        //             /                             \14  14/
        //   1 DDm<0  IA  DDm>0  2                 13 \    / 24
        //           /                                  IA
        //            \                            13 /    \ 24
        //   3 DDp>0   IA   DDp<0 4                  /23  23\
        //              \ 

        float Dx = transform.position.x - target.transform.position.x;
        float Dy = transform.position.y - target.transform.position.y;
        float DDp = Dx + Dy;
        float DDm = Dx - Dy;

        this.boxCollider.enabled = false; // to avoid the destruction of the projetcile at the frame of the instantiation
        if (DDp < 0) // Up or right
        {
            if(DDm > 0)
            {
                Instantiate(projectileUp, ShotPoint.position, transform.rotation);
            }
            else
            {
                Instantiate(projectileRight, ShotPoint.position, transform.rotation);
            }
        }
        else
        {
            if (DDm > 0)
            {
                Instantiate(projectileLeft, ShotPoint.position, transform.rotation);
            }
            else
            {
                Instantiate(projectileDown, ShotPoint.position, transform.rotation);
            }
        }

        this.boxCollider.enabled = true;
        time_next_move = (Time.time + 1);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackPosCac.position, new Vector3(1.2f, 0.4f, 1));
    }
}
