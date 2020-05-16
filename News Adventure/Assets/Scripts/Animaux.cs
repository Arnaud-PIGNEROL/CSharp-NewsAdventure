using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animaux : MonoBehaviour
{
    public int ptsAtSave;
    public int speed;
    public float moveTime = 0.1f;

    public bool handled_by_player;
    public LayerMask blockingLayer;  //is the space open (no collision?)

    private bool isSafe;
    public float time_next_move;
    
    private GameObject player;

    private Animator animator;
    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        handled_by_player = false;
        isSafe = false;
        time_next_move = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        GameManager.instance.animals.Add(this);

        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!handled_by_player && Time.time >= time_next_move && !isSafe)
            handled_by_player = handled();
    }

    private bool out_of_range()
    {
        if (Mathf.Sqrt((Mathf.Abs(transform.position.x) - Mathf.Abs(player.transform.position.x)) * (Mathf.Abs(transform.position.x) - Mathf.Abs(player.transform.position.x)) + (Mathf.Abs(transform.position.y) - Mathf.Abs(player.transform.position.y)) * (Mathf.Abs(transform.position.y) - Mathf.Abs(player.transform.position.y))) > 20)
            return true;

        return false;
    }

    public void MoveAnimal()
    {
        if(FindObjectOfType<EcranFin>().end == false)
        {
            if (out_of_range() || isSafe)
                return;

            if (handled_by_player && !Safe())
            {
                if (player.GetComponent<Player>().getDrop()) // if the player has attacked last frame
                {
                    player.GetComponent<Player>().setDrop(false); // the animal stop following the player
                    player.GetComponent<Player>().setHand(false);
                    time_next_move = Time.time + 1;
                    handled_by_player = false;
                    this.boxCollider.enabled = true;
                }
                else
                {
                    Move(player.transform.position); //the player still have the animal
                }
            }
            else
            {
                if (this.name == "Koala(Clone)")
                    ia_koala();
                else if (this.name == "Walabi(Clone)")
                    ia_walabi();
            }
        }
    }

    private void ia_koala()
    {
        if (Time.time >= time_next_move) // time to wait bewteen 2 moves
        {
            time_next_move = 0;
            if (!handled_by_player)
            {
                if (Move(Random.Range(-1, 2), Random.Range(-1, 2)))
                {
                    time_next_move = Time.time + Random.Range(1, 2); //we wait bewteen 1s and 3s before to start a new move
                }   
            }
        }
    }

    private void ia_walabi()
    {
        if (Time.time >= time_next_move) // time to wait bewteen 2 moves
        {
            time_next_move = 0;
            if (!handled_by_player)
            {
                if (Move(Random.Range(-1, 2), Random.Range(-1, 2)))
                {
                    time_next_move = Time.time + 1; //we wait bewteen 1s and 3s before to start a new move
                }
            }
        }
    }

    protected bool Move(float xDir, float yDir)
    {
        //Store start position to move from, based on objects current transform position.
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        RaycastHit2D hit;
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        //Check if anything was hit
        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }

    protected bool Move(Vector3 dir)
    {
        switch (player.GetComponent<Player>().getDirection()) // the animal is following the player, not on the player so we shift a bit the position
        {
            case "Up":
                dir.y -= 0.6f;
                break;
            case "Down":
                dir.y += 0.6f;
                break;
            case "Right":
                dir.x -= 0.5f;
                break;
            case "Left":
                dir.x += 0.3f;
                break;
            default:
                break;

        }
        StartCoroutine(SmoothMovement(dir));


        return true;
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

    private bool handled()
    {
        if (!player.GetComponent<Player>().getHand())
        {
            if (Mathf.Sqrt(Mathf.Pow(Mathf.Abs(transform.position.x) - Mathf.Abs(player.transform.position.x), 2) + Mathf.Pow(Mathf.Abs(transform.position.y) - Mathf.Abs(player.transform.position.y), 2)) <= 1)
            {
                // ANIMATION DE L'ANIMAL QUI VAS SUR LE JOUEUR
                player.GetComponent<Player>().setHand(true);
                handled_by_player = true;
                this.boxCollider.enabled = false;
                return true;
            }
        }
        return false;
    }

    private bool Safe()
    {
        if (((int)this.transform.position.x < -8.25 && (int)this.transform.position.x > -10.75) && ((int)this.transform.position.y > 19.75 && (int)this.transform.position.y < 22.25))
        {
            isSafe = true;
            handled_by_player = false;
            time_next_move = Time.time + 1; // to be sure that the animal wont moove
            player.GetComponent<Player>().setHand(false);
            player.GetComponent<Player>().addScore(ptsAtSave);
            return true;
        }
        return false;
    }
}

