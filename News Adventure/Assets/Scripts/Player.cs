﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;
    private bool dropAnimal;
    private bool isHandling;
    public int score;
    public string direction;
    public Animaux animal;
    public Joystick joy;

    Vector2 movement;


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

        if (joy.Vertical > 0 && (Mathf.Abs(joy.Vertical) > Mathf.Abs(joy.Horizontal)))
        {
            this.direction = "Right";
            }
        if (joy.Vertical < 0 && (Mathf.Abs(joy.Vertical) > Mathf.Abs(joy.Horizontal)))
        {
            this.direction = "Left";
         }
        if (joy.Horizontal < 0 && (Mathf.Abs(joy.Horizontal) > Mathf.Abs(joy.Vertical)))
        {
            this.direction = "Down";
        }
        if (joy.Horizontal > 0 && (Mathf.Abs(joy.Horizontal) > Mathf.Abs(joy.Vertical)))
        {
            this.direction = "Up";
        }

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
        FindObjectOfType<AudioManager>().Play("PlayerHit");

        if (this.health <= 0 && FindObjectOfType<GameMan>().victory.activeInHierarchy == false && FindObjectOfType<GameMan>().defeat.activeInHierarchy == false)
        {
            FindObjectOfType<AudioManager>().Play("PlayerDeath");
            FindObjectOfType<GameMan>().Lose();
        }
    }

    public void addScore(int pts)
    {
        score += pts;
    }

    public void setDirection(string direction)
    {
        this.direction = direction;
    }

    public string getDirection()
    {
        return direction;

    }
}
