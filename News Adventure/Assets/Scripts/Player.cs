using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;
    private bool dropAnimal;
    private bool isHandling;
    private int score;
    public Animaux animal;

    // Start is called before the first frame update
    void Start()
    {
        dropAnimal = false;
        isHandling = false;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool getDrop()
    {
        return dropAnimal;
    }

    public void setDrop(bool re_set)
    {
        dropAnimal = re_set;
    }

    public bool getHand()
    {
        return isHandling;
    }

    public void setHand(bool boolean)
    {
        isHandling = boolean;
    }

    public void takeDamage(int dmg)
    {
        this.health -= dmg;
    }

    public void addScore(int pts)
    {
        score += pts;
    }
}
