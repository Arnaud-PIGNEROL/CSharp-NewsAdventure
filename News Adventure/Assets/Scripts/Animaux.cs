using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animaux : MonoBehaviour
{
    public int speed;
    public float moveTime = 0.1f;

    public bool handled_by_player;
    private bool isSafe;
    private bool onMoove;
    private float X_end;      // X coord of the point to reach
    private float Y_end;      // Y coord of the point to reach
    private float time_next_move;

    private Transform player;

    private Animator animator;
    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        handled_by_player = false;
        isSafe = false;
        onMoove = false;
        time_next_move = 0;

        GameManager.instance.animals.Add(this);

        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool out_of_range()
    {
        if (Mathf.Sqrt((Mathf.Abs(transform.position.x) - Mathf.Abs(player.position.x)) * (Mathf.Abs(transform.position.x) - Mathf.Abs(player.position.x)) + (Mathf.Abs(transform.position.y) - Mathf.Abs(player.position.y)) * (Mathf.Abs(transform.position.y) - Mathf.Abs(player.position.y))) > 20)
            return true;

        return false;
    }

    public void MoveAnimal()
    {
        if (out_of_range())
            return;

        float xMoove = 0; // vector of direction X
        float yMoove = 0; // vector of direction Y

        if (handled_by_player)
        {
            Move(player.position);
            onMoove = false;
        }
        else
        {
            handled_by_player = handled(); // check if the animal can be in the player's arm

            if (Time.time >= time_next_move) // time to wait bewteen 2 moves
            {
                onMoove = true;
                time_next_move = 0;

                xMoove = Random.Range(-1, 2);
                yMoove = Random.Range(-1, 2);

                X_end = transform.position.x + xMoove;
                Y_end = transform.position.y + yMoove;
            }
        }


        if (onMoove)
        {
            if (Move(xMoove, yMoove))
            {
                time_next_move = Time.time + Random.Range(2, 4); //we wait bewteen 1s and 3s before to start a new move
                onMoove = false;
            }
        }
    }

    protected bool Move(float xDir, float yDir/*, out RaycastHit2D hit*/)
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

    protected bool Move(Vector3 dir/*, out RaycastHit2D hit*/)
    {

        //       boxCollider.enabled = false;
        //       hit = Physics2D.Linecast(start, end, blockingLayer);
        //       boxCollider.enabled = true;

        //Check if anything was hit
        //       if (hit.transform == null)
        //       {
        StartCoroutine(SmoothMovement(dir));
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

    private bool handled()
    {
        if (Mathf.Sqrt(Mathf.Pow(Mathf.Abs(transform.position.x) - Mathf.Abs(player.position.x), 2) + Mathf.Pow(Mathf.Abs(transform.position.y) - Mathf.Abs(player.position.y), 2)) <= 0.5)
        {
            Debug.Log("TROUVE");
            return true;
        }

        return false;
    }
}

